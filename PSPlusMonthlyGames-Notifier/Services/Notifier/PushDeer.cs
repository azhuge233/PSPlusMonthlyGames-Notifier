using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Models.Record;
using PSPlusMonthlyGames_Notifier.Strings;
using System.Text;
using System.Web;

namespace PSPlusMonthlyGames_Notifier.Services.Notifier {
    internal class PushDeer(ILogger<PushDeer> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<PushDeer> _logger = logger;
		private readonly Config config = config.Value;

		public async Task SendMessage(List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(NotifierString.debugPushDeerSendMessage);
				var webGet = new HtmlWeb();
				var resp = new HtmlDocument();

				foreach (var record in records) {
					_logger.LogDebug($"{NotifierString.debugPushDeerSendMessage} : {record.Title}");
					resp = await webGet.LoadFromWebAsync(
						new StringBuilder()
						.AppendFormat(NotifyFormatString.pushDeerUrlFormat,
									config.PushDeerToken,
									HttpUtility.UrlEncode(record.ToPushDeerMessage()))
						.Append(HttpUtility.UrlEncode(NotifyFormatString.projectLink))
						.ToString()
					);
					_logger.LogDebug(resp.Text);
				}

				_logger.LogDebug($"Done: {NotifierString.debugPushDeerSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {NotifierString.debugPushDeerSendMessage}");
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
