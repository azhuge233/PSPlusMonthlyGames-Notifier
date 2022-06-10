using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Models.Record;
using PSPlusMonthlyGames_Notifier.Strings;

namespace PSPlusMonthlyGames_Notifier.Services.Notifier {
    internal class TgBot: INotifiable {
		private readonly ILogger<TgBot> _logger;

		public TgBot(ILogger<TgBot> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<FreeGameRecord> records) {
			var BotClient = new TelegramBotClient(token: config.TelegramToken ?? string.Empty);

			try {
				foreach (var record in records) {
					_logger.LogDebug($"{NotifierString.debugTelegramSendMessage} : {record.Title}");
					await BotClient.SendTextMessageAsync(
						chatId: config.TelegramChatID ?? string.Empty,
						text: $"{record.ToTelegramMessage()}{NotifyFormatString.projectLinkHTML.Replace("<br>", "\n")}",
						parseMode: ParseMode.Html
					);
				}

				_logger.LogDebug($"Done: {NotifierString.debugTelegramSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {NotifierString.debugTelegramSendMessage}");
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
