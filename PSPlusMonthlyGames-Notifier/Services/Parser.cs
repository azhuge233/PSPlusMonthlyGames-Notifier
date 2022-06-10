using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using HtmlAgilityPack;
using PSPlusMonthlyGames_Notifier.Strings;
using PSPlusMonthlyGames_Notifier.Models.Record;
using PSPlusMonthlyGames_Notifier.Modules;

namespace PSPlusMonthlyGames_Notifier.Services {
	internal class Parser : IDisposable {
		private readonly ILogger<Parser> _logger;
		private readonly IServiceProvider services = DI.BuildDiScraperOnly();

		public Parser(ILogger<Parser> logger) {
			_logger = logger;
		}

		public Tuple<List<FreeGameRecord>, List<FreeGameRecord>> Parse(HtmlDocument htmlDoc, List<FreeGameRecord> oldRecords) {
			try {
				_logger.LogDebug(ParseString.debugHtmlParser);

				var resultList = new List<FreeGameRecord>();
				var pushList = new List<FreeGameRecord>();

				var postcards = htmlDoc.DocumentNode.SelectNodes(ParseString.postcardXPath);

				foreach (var postcard in postcards) {
					var blogLinkTag = postcard.SelectSingleNode(ParseString.titleXPath);
					var title = blogLinkTag.InnerText.Trim();
					var link = blogLinkTag.Attributes["href"].Value;

					_logger.LogDebug(ParseString.debugArticleFound, title);

					if (ParseString.TitleKeywords.Any(words => !title.Contains(words))) _logger.LogDebug(ParseString.debugSkipBlog, title);
					else {
						var freeGame = new FreeGameRecord() {
							Url = link,
							Title = title,
							SubTitle = GetBlogContent(link).DocumentNode.SelectSingleNode(ParseString.subHeaderXPath).InnerText.Trim()
						};

						resultList.Add(freeGame);

						if (!oldRecords.Any(record => record == freeGame)) {
							_logger.LogInformation(ParseString.infoAddArticleToList, title);
							pushList.Add(freeGame);
						} else _logger.LogDebug(ParseString.debugFoundInPreviousRecords, title);
					}
					_logger.LogDebug(Environment.NewLine);
				}

				_logger.LogDebug($"Done: {ParseString.debugHtmlParser}");
				return new Tuple<List<FreeGameRecord>, List<FreeGameRecord>>(resultList, pushList);
			} catch (Exception) {
				_logger.LogError($"Error: {ParseString.debugHtmlParser}");
				throw;
			} finally {
				Dispose();
			}
		}

		private HtmlDocument GetBlogContent(string url) {
			try {
				_logger.LogDebug(ParseString.debugGetBlogContent, url);

				var source = services.GetRequiredService<Scraper>().GetHtmlSource(url);

				_logger.LogDebug($"Done: {ParseString.debugGetBlogContent}", url);
				return source;
			} catch (Exception) {
				_logger.LogError($"Error: {ParseString.debugGetBlogContent}", url);
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
