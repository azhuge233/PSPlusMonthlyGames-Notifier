namespace PSPlusMonthlyGames_Notifier.Strings {
	internal class ParseString {
		#region XPaths
		#endregion

		#region Blog Page Related
		#endregion

		internal const string removeSpecialCharsRegex = @"[^0-9a-zA-Z]+";

		#region debug strings
		internal const string debugHtmlParser = "Parse";
		internal const string debugNoRecordDetected = "No record detected!";
		internal const string infoGameFound = "Found game: {gameName}.";
		internal const string infoAddToList = "Added game {gameName} to list";
		internal const string infoFoundInPreviousRecords = "{gameName} is found in previous records, stop adding in list";
		#endregion
	}
}
