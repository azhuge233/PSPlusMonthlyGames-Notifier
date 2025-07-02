using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Strings;

namespace PSPlusMonthlyGames_Notifier.Services {
    internal class Scraper(ILogger<Scraper> logger, IOptions<Config> config) : IDisposable {
		private readonly ILogger<Scraper> _logger = logger;
		private readonly Config config = config.Value;

		public HtmlDocument GetSource() {
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
