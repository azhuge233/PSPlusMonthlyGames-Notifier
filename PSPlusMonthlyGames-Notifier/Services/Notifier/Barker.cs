﻿using System.Text;
using System.Web;
using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Models.Record;
using PSPlusMonthlyGames_Notifier.Strings;

namespace PSPlusMonthlyGames_Notifier.Services.Notifier {
    internal class Barker: INotifiable {
		private readonly ILogger<Barker> _logger;

		public Barker(ILogger<Barker> logger) {
			_logger = logger;
		}

		public async Task SendMessage(NotifyConfig config, List<FreeGameRecord> records) {
			try {
				var sb = new StringBuilder();
				string url = new StringBuilder().AppendFormat(NotifyFormatString.barkUrlFormat, config.BarkAddress, config.BarkToken).ToString();
				var webGet = new HtmlWeb();
				var resp = new HtmlDocument();

				foreach (var record in records) {
					_logger.LogDebug($"{NotifierString.debugBarkerSendMessage} : {record.Title}");
					resp = await webGet.LoadFromWebAsync(
						new StringBuilder()
							.Append(url)
							.Append(NotifyFormatString.barkUrlTitle)
							.Append(HttpUtility.UrlEncode(record.ToBarkMessage()))
							.Append(HttpUtility.UrlEncode(NotifyFormatString.projectLink))
							.Append(NotifyFormatString.barkUrlArgs)
							.ToString()
					);
					_logger.LogDebug(message: resp.Text);
				}

				_logger.LogDebug($"Done: {NotifierString.debugBarkerSendMessage}");
			} catch (Exception) {
				_logger.LogDebug($"Error: {NotifierString.debugBarkerSendMessage}");
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
