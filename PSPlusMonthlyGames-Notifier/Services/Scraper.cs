// using HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Models.GraphQL;
using PSPlusMonthlyGames_Notifier.Strings;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;

namespace PSPlusMonthlyGames_Notifier.Services {
    internal class Scraper(ILogger<Scraper> logger, IOptions<Config> config) : IDisposable {
		private readonly ILogger<Scraper> _logger = logger;
		private readonly Config config = config.Value;

		public async Task<string> GetSource() {
			if (config.InfoSource.ToLower() == "storeapi") return await GetAPISource();
			else return await GetHtmlSource();
		}

		public async Task<string> GetHtmlSource() {
			string url = ScrapeString.UrlMap[config.InfoSource.ToLower()];
			_logger.LogDebug(ScrapeString.debugGetHtmlSource, url);

			try {
				var client = GetHttpClient();

				var resp = await client.GetAsync(url);

				var result = await resp.Content.ReadAsStringAsync();

				_logger.LogDebug($"Done: {ScrapeString.debugGetHtmlSource}", url);
				return result;
			} catch (Exception) {
				_logger.LogError($"Error: {ScrapeString.debugGetHtmlSource}", url);
				throw;
			}
		}

		public async Task<string> GetHtmlSource(string url) {
			_logger.LogDebug(ScrapeString.debugGetHtmlSource, url);

			try {
				var client = GetHttpClient();

				var resp = await client.GetAsync(url);

				var result = await resp.Content.ReadAsStringAsync();

				_logger.LogDebug($"Done: {ScrapeString.debugGetHtmlSource}", url);
				return result;
			} catch (Exception) {
				_logger.LogError($"Error: {ScrapeString.debugGetHtmlSource}", url);
				throw;
			}
		}

		public async Task<string> GetAPISource() {
			string url = ScrapeString.PSAPIGraphQLUrl;
			_logger.LogDebug(ScrapeString.debugGetGraphQLSource);

			try {
				var client = GetHttpClient(true);

				var query = new GraphQLQuery(
					ScrapeString.GraphQLCategoryGridRetrieveOperationName,
					ScrapeString.GraphQLCategoryGridRetrieveHashDefault,
					ScrapeString.GraphQLCategoryGridRetrieveVariablesID,
					ScrapeString.GraphQLCategoryGridRetrieveSortName
				);

				var stringContent = new StringContent(JsonSerializer.Serialize(query), System.Text.Encoding.UTF8, "application/json");
				var resp = await client.PostAsync(url, stringContent);

				if (!resp.IsSuccessStatusCode) {
					_logger.LogDebug(ScrapeString.debugGetResponseWithDefaultHashFailed);

					var newHash = await GetHash();
					query.Extensions.PersistedQuery.Sha256Hash = newHash;

					stringContent = new StringContent(JsonSerializer.Serialize(query), System.Text.Encoding.UTF8, "application/json");

					resp = await client.PostAsync(url, stringContent);

					if (!resp.IsSuccessStatusCode) throw new Exception(ScrapeString.debugGetResponseWithNewHashFailed);

					_logger.LogDebug(ScrapeString.debugGetResponseWithNewHashSuccess);
				}

				var result = await resp.Content.ReadAsStringAsync();

				_logger.LogDebug($"Done: {ScrapeString.debugGetGraphQLSource}");
				return result;
			} catch (Exception) {
				_logger.LogError($"Error: {ScrapeString.debugGetGraphQLSource}");
				throw;
			}
		}

		private HttpClient GetHttpClient(bool isForAPI = false) { 
			var client = new HttpClient();

			client.DefaultRequestHeaders.Add(ScrapeString.UserAgentKey, ScrapeString.UserAgent);

			if (isForAPI) {
				client.DefaultRequestHeaders.Add(ScrapeString.RefererKey, ScrapeString.Referer);
				client.DefaultRequestHeaders.Add(ScrapeString.XPSNLocaleKey, ScrapeString.XPSNLocale);
				client.DefaultRequestHeaders.Add(ScrapeString.XPSNAppVersionKey, ScrapeString.XPSNAppVersion);
			}

			return client;
		}

		private async Task<string> GetHash() { 
			var urlContainsHash = await GetHashUrlWithPlaywright();
			return ExtractSha256HashFromUrl(urlContainsHash);
		}

		private async Task<string> GetHashUrlWithPlaywright() {
			_logger.LogDebug(ScrapeString.debugGetHashUrlWithPlayright);
			try {
				// install firefox if not installed
				Microsoft.Playwright.Program.Main(["install", "firefox"]);

				// create playwright
				using var playwright = await Playwright.CreateAsync();
				await using var browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions {
					Headless = true
				});

				var context = await browser.NewContextAsync();
				var page = await context.NewPageAsync();

				await page.GotoAsync(ScrapeString.PSStoreLandingPageUrl);

				// click accept cookie banner
				var acceptButton = page.Locator(ScrapeString.BannerDeclineButtonSelector, new PageLocatorOptions { 
					HasTextRegex = new Regex(ScrapeString.BannerDeclineButtonText) 
				});
				await acceptButton.WaitForAsync();
				await acceptButton.ClickAsync();

				// navigate to browse page, triggering graphql request
				var button = page.Locator(ScrapeString.BrowseLinkSelector, new PageLocatorOptions { 
					HasTextRegex = new Regex(ScrapeString.BrowseLinkText) 
				});
				await button.WaitForAsync();
				await button.ClickAsync();

				await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
				// detect the graphql request
				var request = await page.WaitForRequestAsync(request => request.Url.Contains(ScrapeString.GraphQLCategoryGridRetrieveOperationName));

				string matchedUrl = request.Url;

				await browser.CloseAsync();

				_logger.LogDebug($"Done: {ScrapeString.debugGetHashUrlWithPlayright}");
				return matchedUrl;
			} catch (Exception ex) {
				_logger.LogError(ex, $"Error: {ScrapeString.debugGetHashUrlWithPlayright}");
				throw;
			}
		}

		private string ExtractSha256HashFromUrl(string url) {
			url = HttpUtility.UrlDecode(url);

			_logger.LogDebug(url);

			var match = Regex.Match(url, ScrapeString.HashRegexPattern);

			if (match.Success && match.Groups.Count > 1) {
				var value = match.Groups[1].Value;
				_logger.LogDebug(ScrapeString.debugGetHashFromUrlSuccess, value);
				return value;
			}

			throw new Exception(ScrapeString.debugGetHashFromUrlFailed);
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
