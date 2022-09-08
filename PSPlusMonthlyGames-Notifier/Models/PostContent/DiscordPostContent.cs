using Newtonsoft.Json;

namespace PSPlusMonthlyGames_Notifier.Models.PostContent {
	internal class Footer {
		[JsonProperty("text")]
		public string Text { get; set; }

	}
	internal class Embed {
		[JsonProperty("title")]
		public string Title { get; set; }
		[JsonProperty("url")]
		public string Url { get; set; }
		[JsonProperty("description")]
		public string Description { get; set; }
		[JsonProperty("color")]
		public int Color { get; set; } = 28881;
		[JsonProperty("footer")]
		public Footer Footer { get; set; }
	}
	internal class DiscordPostContent {
		[JsonProperty("content")]
		public string Content { get; set; }
		[JsonProperty("embeds")]
		public List<Embed> Embeds { get; set; } = new List<Embed>();
	}
}
