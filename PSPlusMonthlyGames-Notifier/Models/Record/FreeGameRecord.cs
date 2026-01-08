using System.Text;
using PSPlusMonthlyGames_Notifier.Strings;

namespace PSPlusMonthlyGames_Notifier.Models.Record {
    public class FreeGameRecord {
        public string Url { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }

        #region ToMessage()
        internal string ToTelegramMessage() {
            return string.Format(NotifyFormatString.telegramPushFormat, Title, SubTitle, Url, RemoveSpecialCharactersForTag(Title));
        }

        internal string ToBarkMessage() {
            return string.Format(NotifyFormatString.barkPushFormat, Title, SubTitle, Url);
        }

        internal string ToEmailMessage() {
            return string.Format(NotifyFormatString.emailPushHtmlFormat, Title, SubTitle, Url);
        }

        internal string ToQQMessage() {
            return string.Format(NotifyFormatString.qqPushFormat, Title, SubTitle, Url);
        }

        internal string ToPushPlusMessage() {
            return string.Format(NotifyFormatString.pushPlusPushHtmlFormat, Title, SubTitle, Url);
        }

        internal string ToDingTalkMessage() {
            return string.Format(NotifyFormatString.dingTalkPushFormat, Title, SubTitle, Url);
        }

        internal string ToPushDeerMessage() {
            return string.Format(NotifyFormatString.pushDeerPushFormat, Title, SubTitle, Url);
        }

        internal string ToDiscordMessage() {
            return string.Format(NotifyFormatString.discordPushFormat, SubTitle, Url);
        }

		internal string ToMeowMessage() {
			return string.Format(NotifyFormatString.meowPushFormat, Title, SubTitle, Url);
		}
		#endregion

		#region other functions
		public static string RemoveSpecialCharactersForTag(string input) {
			if (string.IsNullOrEmpty(input)) return input;

			var sb = new StringBuilder();

			foreach (char c in input) {
				if (char.IsLetterOrDigit(c) || c == '_') sb.Append(c);
			}

			return sb.ToString();
		}
		#endregion

		#region compare methods
		public static bool operator ==(FreeGameRecord a, FreeGameRecord b) { 
            if(ReferenceEquals(a, b)) return true;
            if(a is null || b is null) return false;

            return a.Url == b.Url && a.Title == b.Title && a.SubTitle == b.SubTitle;
        }

        public static bool operator !=(FreeGameRecord a, FreeGameRecord b) {
            if (ReferenceEquals(a, b)) return false;
            if (a is null || b is null) return false;

            return !(a.Url == b.Url && a.Title == b.Title && a.SubTitle == b.SubTitle);
        }

		public override bool Equals(object obj) {
			return Equals(obj as FreeGameRecord);
		}

        public bool Equals(FreeGameRecord freegamerecord) {
            return freegamerecord is not null && Title == freegamerecord.Title && Url == freegamerecord.Url && SubTitle == freegamerecord.SubTitle;
        }

		public override int GetHashCode() {
			return Title.GetHashCode();
		}
		#endregion
	}
}
