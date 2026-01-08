using Microsoft.Extensions.Logging;
using PSPlusMonthlyGames_Notifier.Models.Record;
using PSPlusMonthlyGames_Notifier.Strings;
using System.Text.Json;

namespace PSPlusMonthlyGames_Notifier.Services {
	internal class JsonOP(ILogger<JsonOP> logger) : IDisposable {
		private readonly ILogger<JsonOP> _logger = logger;

		public void WriteData(List<FreeGameRecord> data) {
			try {
				if (data.Count > 0) {
					_logger.LogDebug(JsonOPString.debugWrite);
					string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
					File.WriteAllText(JsonOPString.recordsPath, string.Empty);
					File.WriteAllText(JsonOPString.recordsPath, json);
					_logger.LogDebug($"Done: {JsonOPString.debugWrite}");
				} else _logger.LogDebug("No records detected, quit writing records");
			} catch (Exception) {
				_logger.LogError($"Error: {JsonOPString.debugWrite}");
				throw;
			} finally {
				Dispose();
			}
		}

		public List<FreeGameRecord> LoadData() {
			try {
				_logger.LogDebug(JsonOPString.debugLoadRecords);
				var content = JsonSerializer.Deserialize<List<FreeGameRecord>>(File.ReadAllText(JsonOPString.recordsPath));
				_logger.LogDebug($"Done: {JsonOPString.debugLoadRecords}");
				return content ?? [];
			} catch (Exception) {
				_logger.LogError($"Error: {JsonOPString.debugLoadRecords}");
				throw;
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
