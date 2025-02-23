using IndiegalaFreebieNotifier.Notifier;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PSPlusMonthlyGames_Notifier.Services;
using PSPlusMonthlyGames_Notifier.Services.Notifier;

namespace PSPlusMonthlyGames_Notifier.Modules {
    internal static class DI {
        private static readonly IConfigurationRoot logConfig = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
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
               .AddTransient<QQPusher>()
			   .AddTransient<QQRed>()
			   .AddTransient<PushPlus>()
               .AddTransient<DingTalk>()
               .AddTransient<PushDeer>()
               .AddTransient<Discord>()
               .AddTransient<Meow>()
               .AddLogging(loggingBuilder => {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(logConfig);
               })
               .BuildServiceProvider();
        }

        internal static IServiceProvider BuildDiNotifierOnly() {
            return new ServiceCollection()
               .AddTransient<TgBot>()
               .AddTransient<Barker>()
               .AddTransient<Email>()
               .AddTransient<QQPusher>()
			   .AddTransient<QQRed>()
			   .AddTransient<PushPlus>()
               .AddTransient<DingTalk>()
               .AddTransient<PushDeer>()
			   .AddTransient<Discord>()
			   .AddTransient<Meow>()
			   .AddLogging(loggingBuilder => {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(logConfig);
               })
               .BuildServiceProvider();
        }

        internal static IServiceProvider BuildDiScraperOnly() {
            return new ServiceCollection()
               .AddTransient<Scraper>()
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
