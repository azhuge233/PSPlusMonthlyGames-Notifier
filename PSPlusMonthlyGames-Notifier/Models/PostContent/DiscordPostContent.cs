using System.Text.Json.Serialization;

namespace PSPlusMonthlyGames_Notifier.Models.PostContent {
	internal class Footer {
		[JsonPropertyName("text")]
		public string Text { get; set; }

	}
	internal class Embed {
		[JsonPropertyName("title")]
		public string Title { get; set; }
		[JsonPropertyName("url")]
		public string Url { get; set; }
		[JsonPropertyName("description")]
		public string Description { get; set; }
		[JsonPropertyName("color")]
		public int Color { get; set; } = 28881;
		[JsonPropertyName("footer")]
		public Footer Footer { get; set; }
	}
	internal class DiscordPostContent {
		[JsonPropertyName("content")]
		public string Content { get; set; }
		[JsonPropertyName("embeds")]
		public List<Embed> Embeds { get; set; } = new List<Embed>();
	}
}
