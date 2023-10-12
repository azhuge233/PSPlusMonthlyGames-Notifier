﻿using Microsoft.Extensions.Logging;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Strings;

namespace PSPlusMonthlyGames_Notifier.Services {
    internal class ConfigValidator: IDisposable {
		private readonly ILogger<ConfigValidator> _logger;
		public ConfigValidator(ILogger<ConfigValidator> logger) {
			_logger = logger;
		}

		internal void CheckValid(Config config) {
			try {
				_logger.LogDebug(ConfigValidatorString.debugCheckValid);

				if (!ConfigValidatorString.ValidInfoSources.Contains(config.InfoSource.ToLower()))
					throw new Exception(message: "Please select a valid info source: PSBlog, PSNine");

				//Telegram
				if (config.EnableTelegram) {
					if (string.IsNullOrEmpty(config.TelegramToken))
						throw new Exception(message: "No Telegram Token provided!");
					if (string.IsNullOrEmpty(config.TelegramChatID))
						throw new Exception(message: "No Telegram ChatID provided!");
				}

				//Bark
				if (config.EnableBark) {
					if (string.IsNullOrEmpty(config.BarkAddress))
						throw new Exception(message: "No Bark Address provided!");
					if (string.IsNullOrEmpty(config.BarkToken))
						throw new Exception(message: "No Bark Token provided!");
				}

				//Email
				if (config.EnableEmail) {
					if (string.IsNullOrEmpty(config.FromEmailAddress))
						throw new Exception(message: "No from email address provided!");
					if (string.IsNullOrEmpty(config.ToEmailAddress))
						throw new Exception(message: "No to email address provided!");
					if (string.IsNullOrEmpty(config.SMTPServer))
						throw new Exception(message: "No SMTP server provided!");
					if (string.IsNullOrEmpty(config.AuthAccount))
						throw new Exception(message: "No email auth account provided!");
					if (string.IsNullOrEmpty(config.AuthPassword))
						throw new Exception(message: "No email auth password provided!");
				}

				//QQ
				if (config.EnableQQ) {
					if (string.IsNullOrEmpty(config.QQAddress))
						throw new Exception(message: "No QQ address provided!");
					if (string.IsNullOrEmpty(config.QQPort))
						throw new Exception(message: "No QQ port provided!");
					if (string.IsNullOrEmpty(config.ToQQID))
						throw new Exception(message: "No QQ ID provided!");
				}

				//QQ Red (Chronocat)
				if (config.EnableRed) {
					if (string.IsNullOrEmpty(config.RedAddress))
						throw new Exception(message: "No Red address provided!");
					if (string.IsNullOrEmpty(config.RedPort))
						throw new Exception(message: "No Red port provided!");
					if (string.IsNullOrEmpty(config.RedToken))
						throw new Exception(message: "No Red token provided!");
					if (string.IsNullOrEmpty(config.ToQQID))
						throw new Exception(message: "No QQ ID provided!");
				}

				//PushPlus
				if (config.EnablePushPlus) {
					if (string.IsNullOrEmpty(config.PushPlusToken))
						throw new Exception(message: "No PushPlus token provided!");
				}

				//DingTalk
				if (config.EnableDingTalk) {
					if (string.IsNullOrEmpty(config.DingTalkBotToken))
						throw new Exception(message: "No DingTalk token provided!");
				}

				//PushDeer
				if (config.EnablePushDeer) {
					if (string.IsNullOrEmpty(config.PushDeerToken))
						throw new Exception(message: "No PushDeer token provided!");
				}

				//Discord
				if (config.EnableDiscord) {
					if (string.IsNullOrEmpty(config.DiscordWebhookURL))
						throw new Exception(message: "No Discord Webhook provided!");
				}

				_logger.LogDebug($"Done: {ConfigValidatorString.debugCheckValid}");
			} catch (Exception) {
				_logger.LogError($"Error: {ConfigValidatorString.debugCheckValid}");
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
