using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using HtmlAgilityPack;
using PSPlusMonthlyGames_Notifier.Strings;
using PSPlusMonthlyGames_Notifier.Models.Record;
using PSPlusMonthlyGames_Notifier.Modules;
using PSPlusMonthlyGames_Notifier.Models.Config;
using System.Text;

namespace PSPlusMonthlyGames_Notifier.Services {
	internal class Parser : IDisposable {
		private readonly ILogger<Parser> _logger;
		private readonly IServiceProvider services = DI.BuildDiScraperOnly();

		public Parser(ILogger<Parser> logger) {
			_logger = logger;
		}

		public Tuple<List<FreeGameRecord>, List<FreeGameRecord>> Parse(HtmlDocument htmlDoc, List<FreeGameRecord> oldRecords, Config config) {
			try {
				_logger.LogDebug(ParseString.debugHtmlParser);

				Tuple<List<FreeGameRecord>, List<FreeGameRecord>> result = null;

				if (config.InfoSource.ToLower() == "psblog")
					result = ParsePSBlog(htmlDoc, oldRecords);
				else if (config.InfoSource.ToLower() == "psnine")
					result = ParsePSNine(htmlDoc, oldRecords);

				_logger.LogDebug($"Done: {ParseString.debugHtmlParser}");
				return result;
			} catch (Exception) {
				_logger.LogError($"Error: {ParseString.debugHtmlParser}");
				throw;
			}
		}

		private Tuple<List<FreeGameRecord>, List<FreeGameRecord>> ParsePSBlog(HtmlDocument htmlDoc, List<FreeGameRecord> oldRecords) {
			try {
				_logger.LogDebug(ParseString.debugParsePSBlog);

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
						var postContentSource = GetPostContent(link);
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

		private Tuple<List<FreeGameRecord>, List<FreeGameRecord>> ParsePSNine(HtmlDocument htmlDoc, List<FreeGameRecord> oldRecords) {
			try {
				_logger.LogDebug(ParseString.debugParsePSNine);

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
						var contentDiv = GetPostContent(link).DocumentNode.SelectSingleNode(ParseString.PSNineContentXPath);
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

		private HtmlDocument GetPostContent(string url) {
			try {
				_logger.LogDebug(ParseString.debugGetPostContent, url);

				var source = services.GetRequiredService<Scraper>().GetPostContent(url);

				_logger.LogDebug($"Done: {ParseString.debugGetPostContent}", url);
				return source;
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
