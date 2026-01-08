using System.Text.Json.Serialization;

namespace PSPlusMonthlyGames_Notifier.Models.PostContent {
	public class MeowPostContent {
		[JsonPropertyName("title")]
		public string Title { get; set; }
		[JsonPropertyName("msg")]
		public string Message { get; set; }
		[JsonPropertyName("url")]
		public string Url { get; set; }
	}
}