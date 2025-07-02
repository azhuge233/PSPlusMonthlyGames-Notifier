using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Models.Record;
using PSPlusMonthlyGames_Notifier.Strings;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace PSPlusMonthlyGames_Notifier.Services.Notifier {
    internal class TgBot(ILogger<TgBot> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<TgBot> _logger = logger;
		private readonly Config config = config.Value;

		public async Task SendMessage(List<FreeGameRecord> records) {
			var BotClient = new TelegramBotClient(token: config.TelegramToken ?? string.Empty);

			try {
				foreach (var record in records) {
					_logger.LogDebug($"{NotifierString.debugTelegramSendMessage} : {record.Title}");
					await BotClient.SendMessage(
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
