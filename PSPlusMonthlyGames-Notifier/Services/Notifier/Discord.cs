using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Models.PostContent;
using PSPlusMonthlyGames_Notifier.Models.Record;
using PSPlusMonthlyGames_Notifier.Services.Notifier;
using PSPlusMonthlyGames_Notifier.Strings;
using System.Text;

namespace IndiegalaFreebieNotifier.Notifier {
	public class Discord : INotifiable {
		private readonly ILogger<Discord> _logger;

		#region debug strings
		private readonly string debugSendMessage = "Send notification to Discord";
		#endregion

		public Discord(ILogger<Discord> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(debugSendMessage);

				var url = config.DiscordWebhookURL;
				var content = new DiscordPostContent() {
					Content = "New PlayStation Monthly Free Game"
				};

				foreach (var record in records) {
					content.Embeds.Add(
						new Embed() {
							Title = record.Title,
							Url = record.Url,
							Description = record.ToDiscordMessage(),
							Footer = new Footer() { Text = NotifyFormatString.projectLink }
						}
					);
				}

				var data = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
				var resp = await new HttpClient().PostAsync(url, data);
				_logger.LogDebug(await resp.Content.ReadAsStringAsync());

				_logger.LogDebug($"Done: {debugSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {debugSendMessage}");
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