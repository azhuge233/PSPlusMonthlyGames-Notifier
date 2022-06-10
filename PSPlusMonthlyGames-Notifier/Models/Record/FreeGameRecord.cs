using System.Text;
using PSPlusMonthlyGames_Notifier.Strings;

namespace PSPlusMonthlyGames_Notifier.Models.Record {
    public class FreeGameRecord {
        public string Url { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }

        internal string ToTelegramMessage() {
            return new StringBuilder().AppendFormat(NotifyFormatString.telegramPushFormat, Title, SubTitle, Url).ToString();
        }

        internal string ToBarkMessage() {
            return new StringBuilder().AppendFormat(NotifyFormatString.barkPushFormat, Title, SubTitle, Url).ToString();
        }

        internal string ToEmailMessage() {
            return new StringBuilder().AppendFormat(NotifyFormatString.emailPushHtmlFormat, Title, SubTitle, Url).ToString();
        }

        internal string ToQQMessage() {
            return new StringBuilder().AppendFormat(NotifyFormatString.qqPushFormat, Title, SubTitle, Url).ToString();
        }

        internal string ToPushPlusMessage() {
            return new StringBuilder().AppendFormat(NotifyFormatString.pushPlusPushHtmlFormat, Title, SubTitle, Url).ToString();
        }

        internal string ToDingTalkMessage() {
            return new StringBuilder().AppendFormat(NotifyFormatString.dingTalkPushFormat, Title, SubTitle, Url).ToString();
        }

        internal string ToPushDeerMessage() {
            return new StringBuilder().AppendFormat(NotifyFormatString.pushDeerPushFormat, Title, SubTitle, Url).ToString();
        }

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
	}
}
