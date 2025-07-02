using Microsoft.Extensions.DependencyInjection;
using NLog;
using PSPlusMonthlyGames_Notifier.Modules;
using PSPlusMonthlyGames_Notifier.Services;

namespace PSPlusMonthlyGames_Notifier {
    internal class Program {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static async Task Main() {
            try {
                var servicesProvider = DI.BuildDiAll();

                logger.Info(" - Start Job -");

                using (servicesProvider as IDisposable) {
                    var jsonOp = servicesProvider.GetRequiredService<JsonOP>();

                    var oldRecord = jsonOp.LoadData();
                    servicesProvider.GetRequiredService<ConfigValidator>().CheckValid();

                    // Get page source
                    var source = servicesProvider.GetRequiredService<Scraper>().GetSource();

                    // Parse page source
                    var parseResult = servicesProvider.GetRequiredService<Parser>().Parse(source, oldRecord);

                    // Notify first, then write records
                    await servicesProvider.GetRequiredService<NotifyOP>().Notify(parseResult.Item2);

                    // Write new records
                    jsonOp.WriteData(parseResult.Item1);
                }

                logger.Info(" - Job End -\n");
            } catch (Exception ex) {
                logger.Error(ex.Message);
                if (ex.InnerException != null) logger.Error(ex.InnerException.Message);
            } finally {
                LogManager.Shutdown();
            }
        }
    }
}