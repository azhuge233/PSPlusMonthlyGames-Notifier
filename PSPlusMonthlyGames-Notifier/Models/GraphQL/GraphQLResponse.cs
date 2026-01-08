using System.Text.Json.Serialization;

namespace PSPlusMonthlyGames_Notifier.Models.GraphQL {
	public class Price {
		[JsonPropertyName("basePrice")]
		public string BasePrice { get; set; }

		[JsonPropertyName("includesBundleOffer")]
		public bool IncludesBundleOffer { get; set; }

		[JsonPropertyName("isExclusive")]
		public bool IsExclusive { get; set; }

		[JsonPropertyName("isFree")]
		public bool IsFree { get; set; }

		[JsonPropertyName("isTiedToSubscription")]
		public bool IsTiedToSubscription { get; set; }

		[JsonPropertyName("upsellServiceBranding")]
		public List<string> UpsellServiceBranding { get; set; }

		[JsonPropertyName("upsellText")]
		public string UpsellText { get; set; }
	}

	public class Concept {
		[JsonPropertyName("id")]
		public string ID { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("price")]
		public Price Price { get; set; }
	}

	public class PageInfo {
		[JsonPropertyName("offset")]
		public int Offset { get; set; }

		[JsonPropertyName("size")]
		public int Size { get; set; }

		[JsonPropertyName("totalCount")]
		public int TotalCount { get; set; }
	}

	public class CategoryGridRetrieve {
		[JsonPropertyName("concepts")]
		public List<Concept> Concepts { get; set; }

		[JsonPropertyName("pageInfo")]
		public PageInfo PageInfo { get; set; }
	}

	public class Data {
		[JsonPropertyName("categoryGridRetrieve")]
		public CategoryGridRetrieve CategoryGridRetrieve { get; set; }
	}

	public class GraphQLResponse {
		[JsonPropertyName("data")]
		public Data Data { get; set; }
	}
}
