namespace PSPlusMonthlyGames_Notifier.Strings {
    internal class ScrapeString {
        #region debug strings
        internal const string debugGetPageSource = "Get source: {0} ";
        #endregion

        internal const string PSBlogUrl = "https://blog.zh-hant.playstation.com/category/ps-plus/";
        internal const string PSNineUrl = "https://psnine.com/psnid/sakauenachi/topic";

        internal static readonly Dictionary<string, string> UrlMap = new() {
            { "psblog", PSBlogUrl }, { "psnine", PSNineUrl }
        };
	}
}
