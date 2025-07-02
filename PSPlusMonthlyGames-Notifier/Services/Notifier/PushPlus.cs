using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Models.Record;
using PSPlusMonthlyGames_Notifier.Strings;
using System.Text;
using System.Web;

namespace PSPlusMonthlyGames_Notifier.Services.Notifier {
    internal class PushPlus(ILogger<PushPlus> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<PushPlus> _logger = logger;
		private readonly Config config = config.Value;

		public async Task SendMessage(List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(NotifierString.debugPushPlusSendMessage);

				var title = HttpUtility.UrlEncode(new StringBuilder().AppendFormat(NotifyFormatString.pushPlusTitleFormat, records.Count).ToString());
				var url = new StringBuilder().AppendFormat(NotifyFormatString.pushPlusUrlFormat, config.PushPlusToken, title);
				var message = CreateMessage(records);

				var resp = await new HtmlWeb().LoadFromWebAsync(
					new StringBuilder()
						.Append(url)
						.Append(message)
						.ToString()
				);
				_logger.LogDebug(resp.Text);

				_logger.LogDebug($"Done: {NotifierString.debugPushPlusSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {NotifierString.debugPushPlusSendMessage}");
				throw;
			} finally {
				Dispose();
			}
		}

		private string CreateMessage(List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(NotifierString.debugPushPlusCreateMessage);

				var sb = new StringBuilder();

				records.ForEach(record => {
					sb.AppendFormat(NotifyFormatString.pushPlusBodyFormat, record.ToPushPlusMessage());
				});

				sb.Append("<br>").Append(NotifyFormatString.projectLinkHTML);

				_logger.LogDebug($"Done: {NotifierString.debugPushPlusCreateMessage}");
				return HttpUtility.UrlEncode(sb.ToString());
			} catch (Exception) {
				_logger.LogError($"Error: {NotifierString.debugPushPlusCreateMessage}");
				throw;
			}
		}


		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
