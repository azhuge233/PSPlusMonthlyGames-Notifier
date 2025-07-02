using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Models.Record;

namespace PSPlusMonthlyGames_Notifier.Services.Notifier {
	internal interface INotifiable: IDisposable {
		internal Task SendMessage(List<FreeGameRecord> records);
	}
}
