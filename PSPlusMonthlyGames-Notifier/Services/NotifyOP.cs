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
					var notifyTasks = new List<Task>();

					// Telegram notifications
					if (config.EnableTelegram) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "Telegram");
						notifyTasks.Add(services.GetRequiredService<TgBot>().SendMessage(config, pushList));
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "Telegram");

					// Bark notifications
					if (config.EnableBark) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "Bark");
						notifyTasks.Add(services.GetRequiredService<Barker>().SendMessage(config, pushList));
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "Bark");

					// QQ Http notifications
					if (config.EnableQQHttp) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "QQ Http");
						notifyTasks.Add(services.GetRequiredService<QQHttp>().SendMessage(config, pushList));
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "QQ Http");

					// QQ WebSocket notifications
					if (config.EnableQQWebSocket) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "QQ WebSocket");
						notifyTasks.Add(services.GetRequiredService<QQWebSocket>().SendMessage(config, pushList));
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "QQ WebSocket");

					// PushPlus notifications
					if (config.EnablePushPlus) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "PushPlus");
						notifyTasks.Add(services.GetRequiredService<PushPlus>().SendMessage(config, pushList));
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "PushPlus");

					// DingTalk notifications
					if (config.EnableDingTalk) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "DingTalk");
						notifyTasks.Add(services.GetRequiredService<DingTalk>().SendMessage(config, pushList));
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "DingTalk");

					// PushDeer notifications
					if (config.EnablePushDeer) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "PushDeer");
						notifyTasks.Add(services.GetRequiredService<PushDeer>().SendMessage(config, pushList));
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "PushDeer");

					// Discord notifications
					if (config.EnableDiscord) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "Discord");
						notifyTasks.Add(services.GetRequiredService<Discord>().SendMessage(config, pushList));
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "Discord");

					// Email notifications
					if (config.EnableEmail) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "Email");
						notifyTasks.Add(services.GetRequiredService<Email>().SendMessage(config, pushList));
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "Email");

					// Meow notifications
					if (config.EnableMeow) {
						_logger.LogInformation(NotifyOPString.debugEnabledFormat, "Meow");
						notifyTasks.Add(services.GetRequiredService<Meow>().SendMessage(config, pushList));
					} else _logger.LogInformation(NotifyOPString.debugDisabledFormat, "Meow");

					await Task.WhenAll(notifyTasks);
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
