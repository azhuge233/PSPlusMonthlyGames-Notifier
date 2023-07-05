using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Strings;

namespace PSPlusMonthlyGames_Notifier.Services {
    internal class Scraper: IDisposable {
		private readonly ILogger<Scraper> _logger;

		public Scraper(ILogger<Scraper> logger) {
			_logger = logger;
		}

		public HtmlDocument GetSource(Config config) {
			string url = ScrapeString.UrlMap[config.InfoSource.ToLower()];
			_logger.LogDebug(ScrapeString.debugGetPageSource, url);

			try {
				var webGet = new HtmlWeb();
				var htmlDoc = webGet.Load(url);
				_logger.LogDebug($"Done: {ScrapeString.debugGetPageSource}", url);
				return htmlDoc;
			} catch (Exception) {
				_logger.LogError($"Error: {ScrapeString.debugGetPageSource}", url);
				throw;
			}
		}

		public HtmlDocument GetPostContent(string url) {
			_logger.LogDebug(ScrapeString.debugGetPageSource, url);

			try {
				var webGet = new HtmlWeb();
				var htmlDoc = webGet.Load(url);
				_logger.LogDebug($"Done: {ScrapeString.debugGetPageSource}", url);
				return htmlDoc;
			} catch (Exception) {
				_logger.LogError($"Error: {ScrapeString.debugGetPageSource}", url);
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
