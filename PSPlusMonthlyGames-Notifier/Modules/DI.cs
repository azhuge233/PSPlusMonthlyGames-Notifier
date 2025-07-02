using IndiegalaFreebieNotifier.Notifier;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PSPlusMonthlyGames_Notifier.Models.Config;
using PSPlusMonthlyGames_Notifier.Services;
using PSPlusMonthlyGames_Notifier.Services.Notifier;

namespace PSPlusMonthlyGames_Notifier.Modules {
    internal static class DI {
        private static readonly IConfigurationRoot logConfig = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .Build();
		private static readonly IConfigurationRoot configuration = new ConfigurationBuilder()
		   .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("Config File/config.json", optional: false, reloadOnChange: true)
		   .Build();

		internal static IServiceProvider BuildDiAll() {
            return new ServiceCollection()
               .AddTransient<JsonOP>()
               .AddTransient<ConfigValidator>()
               .AddTransient<Scraper>()
               .AddTransient<Parser>()
               .AddTransient<NotifyOP>()
               .AddTransient<Barker>()
               .AddTransient<TgBot>()
               .AddTransient<Email>()
               .AddTransient<QQHttp>()
			   .AddTransient<QQWebSocket>()
			   .AddTransient<PushPlus>()
               .AddTransient<DingTalk>()
               .AddTransient<PushDeer>()
               .AddTransient<Discord>()
               .AddTransient<Meow>()
               .Configure<Config>(configuration)
               .AddLogging(loggingBuilder => {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(logConfig);
               })
               .BuildServiceProvider();
        }
    }
}
