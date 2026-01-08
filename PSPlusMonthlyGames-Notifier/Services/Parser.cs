using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using PSPlusMonthlyGames_Notifier.Strings;
using PSPlusMonthlyGames_Notifier.Models.Record;
using PSPlusMonthlyGames_Notifier.Models.Config;
using System.Text;
using Microsoft.Extensions.Options;
using System.Text.Json;
using PSPlusMonthlyGames_Notifier.Models.GraphQL;

namespace PSPlusMonthlyGames_Notifier.Services {
	internal class Parser(ILogger<Parser> logger, IOptions<Config> config, Scraper scraper) : IDisposable {
		private readonly ILogger<Parser> _logger = logger;
		private readonly Config config = config.Value;

		public async Task<Tuple<List<FreeGameRecord>, List<FreeGameRecord>>> Parse(string source, List<FreeGameRecord> oldRecords) {
			try {
				_logger.LogDebug(ParseString.debugHtmlParser);

				Tuple<List<FreeGameRecord>, List<FreeGameRecord>> result = null;

				if (config.InfoSource.ToLower() == "psblog")
					result = await ParsePSBlog(source, oldRecords);
				else if (config.InfoSource.ToLower() == "psnine")
					result = await ParsePSNine(source, oldRecords);
				else if (config.InfoSource.ToLower() == "storeapi")
					result = ParseAPI(source, oldRecords);

				_logger.LogDebug($"Done: {ParseString.debugHtmlParser}");
				return result;
			} catch (Exception) {
				_logger.LogError($"Error: {ParseString.debugHtmlParser}");
				throw;
			}
		}

		private async Task<Tuple<List<FreeGameRecord>, List<FreeGameRecord>>> ParsePSBlog(string source, List<FreeGameRecord> oldRecords) {
			try {
				_logger.LogDebug(ParseString.debugParsePSBlog);

				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(source);

				var resultList = new List<FreeGameRecord>();
				var pushList = new List<FreeGameRecord>();

				var postcards = htmlDoc.DocumentNode.SelectNodes(ParseString.PSBlogPostcardXPath);

				foreach (var postcard in postcards) {
					var blogLinkTag = postcard.SelectSingleNode(ParseString.PSBlogTitleXPath);
					var title = blogLinkTag.InnerText.Trim();
					var link = blogLinkTag.Attributes["href"].Value;

					_logger.LogDebug(ParseString.debugArticleFound, title);

					if (ParseString.PSBlogTitleKeywords.Any(words => !title.Contains(words)))
						_logger.LogDebug(ParseString.debugSkipBlog, title);
					else if (oldRecords.Any(record => record.Title == title)) {
						resultList.Add(oldRecords.First(record => record.Title == title));
						_logger.LogDebug(ParseString.debugFoundInPreviousRecords, title);
					} else {
						var postContentSource = await GetPostContent(link);
						var subTitle = postContentSource.DocumentNode.SelectSingleNode(ParseString.PSBlogSubHeaderXPath).InnerText.Trim();

						var freeGame = new FreeGameRecord() {
							Url = link,
							Title = title,
							SubTitle = subTitle
						};

						resultList.Add(freeGame);

						if (!oldRecords.Any(record => record == freeGame)) {
							_logger.LogInformation(ParseString.infoAddArticleToList, title);
							pushList.Add(freeGame);
						} else _logger.LogDebug(ParseString.debugFoundInPreviousRecords, title);
					}
					_logger.LogDebug(Environment.NewLine);
				}

				_logger.LogDebug($"Done: {ParseString.debugParsePSBlog}");
				return new Tuple<List<FreeGameRecord>, List<FreeGameRecord>>(resultList, pushList);
			} catch (Exception) {
				_logger.LogError($"Error: {ParseString.debugParsePSBlog}");
				throw;
			}
		}

		private async Task<Tuple<List<FreeGameRecord>, List<FreeGameRecord>>> ParsePSNine(string source, List<FreeGameRecord> oldRecords) {
			try {
				_logger.LogDebug(ParseString.debugParsePSNine);

				var htmlDoc = new HtmlDocument();
				htmlDoc.Load(source);

				var resultList = new List<FreeGameRecord>();
				var pushList = new List<FreeGameRecord>();

				var posts = htmlDoc.DocumentNode.SelectNodes(ParseString.PSNineEntryXPath);

				foreach (var post in posts) {
					var linkTag = post.SelectSingleNode(ParseString.PSNineTitleXPath);
					var title = linkTag.InnerText.Trim();
					var link = linkTag.Attributes["href"].Value;

					_logger.LogDebug(ParseString.debugArticleFound, title);

					if (ParseString.PSNineTitleKeyWords.Any(words => !title.Contains(words))) {
						_logger.LogDebug(ParseString.debugSkipBlog, title);
					} else if (oldRecords.Any(record => record.Title == title)) {
						resultList.Add(oldRecords.First(record => record.Title == title));
						_logger.LogDebug(ParseString.debugFoundInPreviousRecords, title);
					} else {
						var postContentSource = await GetPostContent(link);
						var contentDiv = postContentSource.DocumentNode.SelectSingleNode(ParseString.PSNineContentXPath);
						var contents = contentDiv.SelectSingleNode(ParseString.PSNineContentBoldXPath);
						var sb = new StringBuilder();

						if (contents != null && !contents.InnerText.StartsWith('*') &&
							!ParseString.PSNineContentBreakKeywords.All(contents.InnerText.Contains)) {
							foreach (var content in contents.ChildNodes) {
								var text = content.InnerText;
								if (content.NodeType == HtmlNodeType.Text)
									sb.Append($"{content.InnerText.Trim()}\n");
							}
						} else {
							foreach (var content in contentDiv.ChildNodes) {
								var text = content.InnerText;
								if (text.StartsWith('*') && ParseString.PSNineContentBreakKeywords.All(text.Contains)) break;
								if (content.NodeType == HtmlNodeType.Text)
									sb.Append($"{content.InnerText.Trim()}\n");
							}
						}

						var freeGame = new FreeGameRecord() {
							Url = link,
							Title = title,
							SubTitle = sb.ToString(),
						};

						resultList.Add(freeGame);

						if (!oldRecords.Any(record => record == freeGame)) {
							_logger.LogInformation(ParseString.infoAddArticleToList, title);
							pushList.Add(freeGame);
						} else _logger.LogDebug(ParseString.debugFoundInPreviousRecords, title);
					}
					_logger.LogDebug(Environment.NewLine);
				}

				_logger.LogDebug($"Done: {ParseString.debugParsePSNine}");
				return new Tuple<List<FreeGameRecord>, List<FreeGameRecord>>(resultList, pushList);
			} catch (Exception) {
				_logger.LogError($"Error: {ParseString.debugParsePSNine}");
				throw;
			}
		}

		private Tuple<List<FreeGameRecord>, List<FreeGameRecord>> ParseAPI(string data, List<FreeGameRecord> oldRecords) {
			try {
				_logger.LogDebug(ParseString.debugParseAPI);

				var queryResp = JsonSerializer.Deserialize<GraphQLResponse>(data);
				var catagoryRetrieve = queryResp.Data.CategoryGridRetrieve;
				var monthlyFreeGames = catagoryRetrieve.Concepts.Where(c => c.Price.UpsellText.ToLower() == ParseString.PSPlusUpSellText).ToList();

				_logger.LogInformation(ParseString.infoFoundFreeGames, catagoryRetrieve.PageInfo.TotalCount, catagoryRetrieve.Concepts.Count, monthlyFreeGames.Count);

				var resultList = new List<FreeGameRecord>();
				var pushList = new List<FreeGameRecord>();

				if (monthlyFreeGames.Count > 0) {
					foreach (var game in monthlyFreeGames) {
						var gameName = game.Name;
						var gamePrice = $"{ParseString.GamePricePrefix}{game.Price.BasePrice}";
						var gameUrl = $"{ParseString.PSStoreGameUrlPrefix}{game.ID}";

						var newRecord = new FreeGameRecord() {
							Title = gameName,
							SubTitle = gamePrice,
							Url = gameUrl
						};

						resultList.Add(newRecord);

						if (!oldRecords.Any(r => r == newRecord)) {
							_logger.LogInformation(ParseString.infoAddArticleToList, newRecord.Title);
							pushList.Add(newRecord);
						} else _logger.LogDebug(ParseString.debugFoundInPreviousRecords, newRecord.Title);
					}
				} else _logger.LogDebug(ParseString.debugNoFreeGamesDetected);

				_logger.LogDebug($"Done: {ParseString.debugParseAPI}");
				return new Tuple<List<FreeGameRecord>, List<FreeGameRecord>>(resultList, pushList);
			} catch (Exception) {
				_logger.LogError($"Error: {ParseString.debugParseAPI}");
				throw;
			}
		}

		private async Task<HtmlDocument> GetPostContent(string url) {
			try {
				_logger.LogDebug(ParseString.debugGetPostContent, url);

				var source = await scraper.GetHtmlSource(url);

				var htmlDoc = new HtmlDocument();
				htmlDoc.LoadHtml(source);

				_logger.LogDebug($"Done: {ParseString.debugGetPostContent}", url);
				return htmlDoc;
			} catch (Exception) {
				_logger.LogError($"Error: {ParseString.debugGetPostContent}", url);
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
