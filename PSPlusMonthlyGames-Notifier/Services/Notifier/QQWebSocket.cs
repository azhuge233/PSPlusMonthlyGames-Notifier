using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Models.Record;
using PSPlusMonthlyGames_Notifier.Models.WebSocketContent;
using PSPlusMonthlyGames_Notifier.Strings;
using System.Net.WebSockets;
using Websocket.Client;

namespace PSPlusMonthlyGames_Notifier.Services.Notifier {
	internal class QQWebSocket(ILogger<QQWebSocket> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<QQWebSocket> _logger = logger;
		private readonly Config config = config.Value;

		public async Task SendMessage(List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(NotifierString.debugQQWebSocketSendMessage);

				var packets = GetSendPacket(config, records);

				using var client = GetWSClient(config);

				await client.Start();

				foreach (var packet in packets) {
					await client.SendInstant(JsonConvert.SerializeObject(packet));
					await Task.Delay(600);
				}

				await client.Stop(WebSocketCloseStatus.NormalClosure, string.Empty);

				_logger.LogDebug($"Done: {NotifierString.debugQQWebSocketSendMessage}");
			} catch (Exception) {
				_logger.LogDebug($"Error: {NotifierString.debugQQWebSocketSendMessage}");
				throw;
			} finally {
				Dispose();
			}
		}

		private WebsocketClient GetWSClient(NotifyConfig config) {
			var url = new Uri(string.Format(NotifyFormatString.qqWebSocketUrlFormat, config.QQWebSocketAddress, config.QQWebSocketPort, config.QQWebSocketToken));

			#region new websocket client
			var client = new WebsocketClient(url);
			client.ReconnectionHappened.Subscribe(info => _logger.LogDebug(NotifierString.debugWSReconnection, info.Type));
			client.MessageReceived.Subscribe(msg => _logger.LogDebug(NotifierString.debugWSMessageRecieved, msg));
			client.DisconnectionHappened.Subscribe(msg => _logger.LogDebug(NotifierString.debugWSDisconnected, msg));
			#endregion

			return client;
		}

		private static List<WSPacket> GetSendPacket(NotifyConfig config, List<FreeGameRecord> records) {
			return records.Select(record => new WSPacket() {
				Action = NotifyFormatString.qqWebSocketSendAction,
				Params = new Param {
					UserID = config.ToQQID,
					Message = $"{record.ToQQMessage()}{NotifyFormatString.projectLink}"
				}
			}).ToList();
		}

		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
