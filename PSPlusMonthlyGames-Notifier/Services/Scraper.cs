using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using PSPlusMonthlyGames_Notifier.Strings;

namespace PSPlusMonthlyGames_Notifier.Services {
    internal class Scraper: IDisposable {
		private readonly ILogger<Scraper> _logger;

		public Scraper(ILogger<Scraper> logger) {
			_logger = logger;
		}

		public HtmlDocument GetPSBlogSource() {
			try {
				_logger.LogDebug(ScrapeString.debugGetPSBlogSource);
				var htmlDoc = GetHtmlSource(ScrapeString.PSBlogUrl);
				_logger.LogDebug($"Done: {ScrapeString.debugGetPSBlogSource}");
				return htmlDoc;
			} catch (Exception) {
				_logger.LogError($"Error: {ScrapeString.debugGetPSBlogSource}");
				throw;
			}
		}

		public HtmlDocument GetHtmlSource(string url) {
			try {
				_logger.LogDebug(ScrapeString.debugGetPageSource, url);
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
