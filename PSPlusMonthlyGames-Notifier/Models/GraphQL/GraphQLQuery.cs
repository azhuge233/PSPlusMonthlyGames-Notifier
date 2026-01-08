using System.Text.Json.Serialization;

namespace PSPlusMonthlyGames_Notifier.Models.GraphQL {
	public class PersistedQuery {
		[JsonPropertyName("version")]
		public int Version { get; set; } = 1;

		[JsonPropertyName("sha256Hash")]
		public string Sha256Hash { get; set; }
	}

	public class Extensions {
		[JsonPropertyName("persistedQuery")]
		public PersistedQuery PersistedQuery { get; set; } = new();
	}

	public class PageArgs {
		[JsonPropertyName("size")]
		public int Size { get; set; } = 10;

		[JsonPropertyName("offset")]
		public int Offset { get; set; } = 0;
	}

	public class SortBy {
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("isAscending")]
		public bool IsAscending { get; set; } = false;
	}

	public class Variables {
		[JsonPropertyName("id")]
		public string ID { get; set; }

		[JsonPropertyName("pageArgs")]
		public PageArgs PageArgs { get; set; } = new();

		[JsonPropertyName("sortBy")]
		public SortBy SortBy { get; set; } = new();
	}

	public class GraphQLQuery {
		[JsonPropertyName("operationName")]
		public string OperationName { get; set; }

		[JsonPropertyName("extensions")]
		public Extensions Extensions { get; set; } = new();

		[JsonPropertyName("variables")]
		public Variables Variables { get; set; } = new();

		public GraphQLQuery() { }

		public GraphQLQuery(string operationName, string hash, string variableID, string sorting) {
			OperationName = operationName;
			Extensions.PersistedQuery.Sha256Hash = hash;
			Variables.ID = variableID;
			Variables.SortBy.Name = sorting;
		}
	}
}
