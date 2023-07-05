namespace PSPlusMonthlyGames_Notifier.Strings {
	internal class ParseString {
		#region PSBlog XPaths
		internal const string PSBlogPostcardXPath = @".//article[contains(@class, 'post-card')]//div[@class='post-card__content']";
		internal const string PSBlogTitleXPath = @".//h2[@class='post-card__title']//a";
		internal const string PSBlogSubHeaderXPath = @".//p[@class='post-single__sub-header-text']";
		#endregion

		#region PSNine XPaths
		internal const string PSNineEntryXPath = @".//div[contains(@class, 'min-inner')]//div[contains(@class, 'box')]//ul[contains(@class, 'list')]//li";
		internal const string PSNineTitleXPath = @".//div[contains(@class, 'ml64')]//div[contains(@class, 'title')]//a";
		internal const string PSNineContentXPath = @".//div[contains(@class, 'content') and contains(@itemprop, 'articleBody')]";
		internal const string PSNineContentBoldXPath = @".//b";
		#endregion

		#region Post Page Related
		internal static readonly List<string> PSBlogTitleKeywords = new() { "PlayStation Plus", "月", "遊戲" };
		internal static readonly List<string> PSNineTitleKeyWords = new() { "港服", "PLUS", "限免" };
		internal static readonly List<string> PSNineContentBreakKeywords = new() { "二档" };
		#endregion

		internal const string removeSpecialCharsRegex = @"[^0-9a-zA-Z]+";

		#region debug strings
		internal const string debugHtmlParser = "Parse";
		internal const string debugParsePSBlog = "Parsing ps blog";
		internal const string debugParsePSNine = "Parsing ps nine";

		internal const string debugGetPostContent = "Get Post Content - URL: {url}";

		internal const string debugNoRecordDetected = "No record detected!";
		internal const string debugSkipBlog = "Non Monthly-Games-related article, skip: {title}";
		internal const string debugArticleFound = "Found new article: {title}";
		internal const string debugFoundInPreviousRecords = "{title} is found in previous records, stop adding in list";

		internal const string infoAddArticleToList = "Added article {title} to push list";
		#endregion
	}
}
