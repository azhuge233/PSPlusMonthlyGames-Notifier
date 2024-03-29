﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using PSPlusMonthlyGames_Notifier.Modules;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Models.Record;
using PSPlusMonthlyGames_Notifier.Strings;
using PSPlusMonthlyGames_Notifier.Services.Notifier;
using IndiegalaFreebieNotifier.Notifier;

namespace PSPlusMonthlyGames_Notifier.Services {
	internal class NotifyOP : IDisposable {
		private readonly ILogger<NotifyOP> _logger;
		private readonly IServiceProvider services = DI.BuildDiNotifierOnly();

		public NotifyOP(ILogger<NotifyOP> logger) {
			_logger = logger;
		}

		public async Task Notify(NotifyConfig config, List<FreeGameRecord> pushList) {
			if (pushList.Count == 0) {
				_logger.LogInformation(NotifyOPString.debugNoNewNotifications);
				return;
			}

			try {
				_logger.LogDebug(NotifyOPString.debugNotify);
				using (services as IDisposable) {
					// Telegram notifications
					if (config.EnableTelegram) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "Telegram");
						await services.GetRequiredService<TgBot>().SendMessage(config, pushList);
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "Telegram");

					// Bark notifications
					if (config.EnableBark) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "Bark");
						await services.GetRequiredService<Barker>().SendMessage(config, pushList);
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "Bark");

					// QQ notifications
					if (config.EnableQQ) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "QQ");
						await services.GetRequiredService<QQPusher>().SendMessage(config, pushList);
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "QQ");

					//QQ Red (Chronocat) notifications
					if (config.EnableRed) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "QQ Red (Chronocat)");
						await services.GetRequiredService<QQRed>().SendMessage(config, pushList);
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "QQ Red (Chronocat)");

					// PushPlus notifications
					if (config.EnablePushPlus) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "PushPlus");
						await services.GetRequiredService<PushPlus>().SendMessage(config, pushList);
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "PushPlus");

					// DingTalk notifications
					if (config.EnableDingTalk) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "DingTalk");
						await services.GetRequiredService<DingTalk>().SendMessage(config, pushList);
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "DingTalk");

					// PushDeer notifications
					if (config.EnablePushDeer) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "PushDeer");
						await services.GetRequiredService<PushDeer>().SendMessage(config, pushList);
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "PushDeer");

					// Discord notifications
					if (config.EnableDiscord) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "Discord");
						await services.GetRequiredService<Discord>().SendMessage(config, pushList);
					} else _logger.LogInformation(NotifyOPString.debugEnabledFormat, "Discord");

					// Email notifications
					if (config.EnableEmail) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "Email");
						await services.GetRequiredService<Email>().SendMessage(config, pushList);
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "Email");
				}

				_logger.LogDebug($"Done: {NotifyOPString.debugNotify}");
			} catch (Exception) {
				_logger.LogError($"Error: {NotifyOPString.debugNotify}");
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
