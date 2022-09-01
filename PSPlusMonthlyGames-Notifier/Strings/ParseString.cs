namespace PSPlusMonthlyGames_Notifier.Strings {
	internal class ParseString {
		#region XPaths
		internal const string postcardXPath = @".//article[contains(@class, 'post-card')]//div[@class='post-card__content']";
		internal const string titleXPath = @".//h2[@class='post-card__title']//a";
		internal const string subHeaderXPath = @".//p[@class='post-single__sub-header-text']";
		#endregion

		#region Blog Page Related
		internal static readonly List<string> TitleKeywords = new() { "PlayStation Plus", "月", "遊戲" };
		#endregion

		internal const string removeSpecialCharsRegex = @"[^0-9a-zA-Z]+";

		#region debug strings
		internal const string debugHtmlParser = "Parse";
		internal const string debugNoRecordDetected = "No record detected!";
		internal const string debugGetBlogContent = "Get Blog Content - URL: {url}";
		internal const string debugSkipBlog = "Non Monthly-Games-related article, skip: {title}";
		internal const string debugArticleFound = "Found new article: {title}";
		internal const string infoAddArticleToList = "Added article {title} to push list";
		internal const string debugFoundInPreviousRecords = "{title} is found in previous records, stop adding in list";
		#endregion
	}
}
