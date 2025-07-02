using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Models.PostContent;
using PSPlusMonthlyGames_Notifier.Models.Record;
using PSPlusMonthlyGames_Notifier.Strings;
using System.Text;

namespace PSPlusMonthlyGames_Notifier.Services.Notifier {
    internal class QQHttp(ILogger<QQHttp> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<QQHttp> _logger = logger;
		private readonly Config config = config.Value;

		public async Task SendMessage(List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(NotifierString.debugQQHttpSendMessage);

				string url = string.Format(NotifyFormatString.qqHttpUrlFormat, config.QQHttpAddress, config.QQHttpPort, config.QQHttpToken);

				var client = new HttpClient();

				var content = new QQHttpPostContent {
					UserID = config.ToQQID
				};

				var data = new StringContent(string.Empty);
				var resp = new HttpResponseMessage();

				foreach (var record in records) {
					_logger.LogDebug($"{NotifierString.debugQQHttpSendMessage} : {record.Title}");

					content.Message = $"{record.ToQQMessage()}{NotifyFormatString.projectLink}";

					data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
					resp = await client.PostAsync(url, data);

					_logger.LogDebug(await resp.Content.ReadAsStringAsync());
				}

				_logger.LogDebug($"Done: {NotifierString.debugQQHttpSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {NotifierString.debugQQHttpSendMessage}");
				throw;
			} finally {
				Dispose();
			}
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
