namespace PSPlusMonthlyGames_Notifier.Strings {
    internal class ScrapeString {
        #region debug strings
        internal const string debugGetHtmlSource = "Get html source: {0} ";
        internal const string debugGetGraphQLSource = "Get GraphQL source";
		internal const string debugGetResponseWithDefaultHashFailed = "Get response with default hash failed, fetching new hash";
		internal const string debugGetResponseWithNewHashFailed = "Get response with new hash failed, abort.";
		internal const string debugGetResponseWithNewHashSuccess = "Get response with new hash success.";
		internal const string debugGetHashUrlWithPlayright = "Get hash url with playwright";
		internal const string debugGetHashFromUrlSuccess = "Get hash from url success, hash: {0}";
		internal const string debugGetHashFromUrlFailed = "Get hash from url failed, abort.";
		#endregion

		#region URL strings
		internal const string PSBlogUrl = "https://blog.zh-hant.playstation.com/category/ps-plus/";
        internal const string PSNineUrl = "https://psnine.com/psnid/sakauenachi/topic";
		internal const string PSStoreLandingPageUrl = "https://store.playstation.com/zh-hans-hk/pages/latest";
		internal const string PSAPIGraphQLUrl = "https://web.np.playstation.com/api/graphql/v1/op";
		#endregion

		#region graphql query strings
		internal const string GraphQLCategoryGridRetrieveOperationName = "categoryGridRetrieve";
		internal const string GraphQLCategoryGridRetrieveHashDefault = "257713466fc3264850aa473409a29088e3a4115e6e69e9fb3e061c8dd5b9f5c6";
        internal const string GraphQLCategoryGridRetrieveVariablesID = "28c9c2b2-cecc-415c-9a08-482a605cb104";
        internal const string GraphQLCategoryGridRetrieveSortName = "contentCollections.contentCollectionStartDate";
		#endregion

		#region http client header strings
		internal const string UserAgentKey = "User-Agent";
		internal const string RefererKey = "Referer";
        internal const string XPSNLocaleKey = "x-psn-store-locale-override";
        internal const string XPSNAppVersionKey = "x-psn-app-ver";

		internal const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/143.0.0.0 Safari/537.36 Edg/143.0.0.0";
		internal const string Referer = "https://store.playstation.com/";
        internal const string XPSNLocale = "zh-Hans-HK";
        internal const string XPSNAppVersion = "/0.0.0-";
		#endregion

		#region selector strings
		internal const string BannerDeclineButtonSelector = "button";
		internal const string BannerDeclineButtonText = "Decline";
		internal const string BrowseLinkSelector = "ul#tertiary-menu-toggle > li > a";
		internal const string BrowseLinkText = "浏览";
		#endregion

		#region regex patterns
		internal const string HashRegexPattern = @"""sha256Hash""\s*:\s*""([a-fA-F0-9]{64})""";
		#endregion

		internal static readonly Dictionary<string, string> UrlMap = new() {
            { "psblog", PSBlogUrl }, { "psnine", PSNineUrl }
        };
	}
}
