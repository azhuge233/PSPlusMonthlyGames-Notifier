namespace PSPlusMonthlyGames_Notifier.Strings {
	internal class NotifyFormatString {
		#region Push Format Strings
		internal const string telegramPushFormat =
			"<b>PS Plus Monthly Games</b>\n\n" +
			"<b>{0}</b>\n\n" +
			"<i>{1}</i>\n\n" +
			"Link：<a href=\"{2}\">{0}</a>\n\n" +
			"#PlayStation #PSPlus";

		internal const string barkPushFormat =
			"{0}\n\n" +
			"{1}\n\n" +
			"Link：{2}";

		internal const string emailPushHtmlFormat =
			"<b>{0}</b><br><br>" +
			"{1}<br><br>" +
			"Link: <a href=\"{2}\" >{0}</a><br>";

		internal const string qqPushFormat =
			imTitle +
			"{0}\n\n" +
			"{1}\n\n" +
			"Link: {2}";

		internal const string pushPlusPushHtmlFormat = emailPushHtmlFormat;

		internal const string dingTalkPushFormat = qqPushFormat;

		internal const string pushDeerPushFormat = qqPushFormat;

		internal const string discordPushFormat =
			"***{0}***\n\n" +
			"Link: {1}";

		internal const string meowPushFormat =
			"{0}\n\n" +
			"{1}\n\n" +
			"Link：{2}";
		#endregion

		#region url, title format string
		internal const string imTitle = "PS Plus Monthly Games\n\n";

		internal const string barkUrlFormat = "{0}/{1}/";
		internal const string barkUrlTitle = "PSPlusMonthlyGamesNotifier/";
		internal const string barkUrlArgs =
			"?group=psplusmonthlygamesnotifier" +
			"&isArchive=1" +
			"&sound=calypso";

		internal const string htmlTitleFormat = "PS Plus Monthly Games";
		internal const string htmlBodyFormat = "<br>{0}";

		internal const string emailTitleFormat = htmlTitleFormat;
		internal const string emailBodyFormat = htmlBodyFormat;

		internal const string qqHttpUrlFormat = "http://{0}:{1}/send_private_msg?access_token={2}";
		internal const string qqWebSocketUrlFormat = "ws://{0}:{1}/?access_token={2}";
		internal const string qqWebSocketSendAction = "send_private_msg";

		internal const string pushPlusTitleFormat = htmlTitleFormat;
		internal const string pushPlusBodyFormat = htmlBodyFormat;
		internal const string pushPlusUrlFormat = "http://www.pushplus.plus/send?token={0}&template=html&title={1}&content=";

		internal const string dingTalkUrlFormat = "https://oapi.dingtalk.com/robot/send?access_token={0}";

		internal const string pushDeerUrlFormat = "https://api2.pushdeer.com/message/push?pushkey={0}&&text={1}";

		internal const string meowUrlFormat = "{0}/{1}";
		internal const string meowUrlTitle = "PSPlusMonthlyGamesNotifier";
		#endregion

		internal const string projectLink = "\n\nFrom https://github.com/azhuge233/PSPlusMonthlyGames-Notifier";
		internal const string projectLinkHTML = "<br><br>From <a href=\"https://github.com/azhuge233/PSPlusMonthlyGames-Notifier\">PSPlusMonthlyGames-Notifier</a>";
	}
}
