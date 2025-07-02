using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Models.Record;
using PSPlusMonthlyGames_Notifier.Strings;
using System.Text;

namespace PSPlusMonthlyGames_Notifier.Services.Notifier {
    internal class Email(ILogger<Email> logger, IOptions<Config> config) : INotifiable {
		private readonly ILogger<Email> _logger = logger;
		private readonly Config config = config.Value;

		public async Task SendMessage(List<FreeGameRecord> records) {
			try {
				_logger.LogDebug(NotifierString.debugEmailSendMessage);

				var message = CreateMessage(records, config.FromEmailAddress ?? String.Empty, config.ToEmailAddress ?? String.Empty);

				using var client = new SmtpClient();
				client.Connect(config.SMTPServer, config.SMTPPort, true);
				client.Authenticate(config.AuthAccount, config.AuthPassword);
				await client.SendAsync(message);
				client.Disconnect(true);

				_logger.LogDebug($"Done: {NotifierString.debugEmailSendMessage}");
			} catch (Exception) {
				_logger.LogError($"Error: {NotifierString.debugEmailSendMessage}");
				throw;
			} finally {
				Dispose();
			}
		}

		private MimeMessage CreateMessage(List<FreeGameRecord> pushList, string fromAddress, string toAddress) {
			try {
				_logger.LogDebug(NotifierString.debugEmailCreateMessage);

				var message = new MimeMessage();

				message.From.Add(new MailboxAddress("PSPlusMonthlyGamesNotifier", fromAddress));
				message.To.Add(new MailboxAddress("Receiver", toAddress));

				var sb = new StringBuilder();

				message.Subject = sb.AppendFormat(NotifyFormatString.emailTitleFormat, pushList.Count).ToString();
				sb.Clear();

				pushList.ForEach(record => {
					sb.AppendFormat(NotifyFormatString.emailBodyFormat, record.ToEmailMessage());
				});

				message.Body = new TextPart("html") {
					Text = sb.Append("<br>")
						.Append(NotifyFormatString.projectLinkHTML)
						.ToString()
				};

				_logger.LogDebug($"Done: {NotifierString.debugEmailCreateMessage}");
				return message;
			} catch (Exception) {
				_logger.LogError($"Error: {NotifierString.debugEmailCreateMessage}");
				throw;
			}
		}
		public void Dispose() {
			GC.SuppressFinalize(this);
		}
	}
}
