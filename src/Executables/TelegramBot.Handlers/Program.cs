using Integration.Applications;
using NLog;
using SimpleScheduler;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace LifeTracker.TelegramBot.Handlers;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var logger = LogManager.GetCurrentClassLogger();
        logger.Info("Starting...");

        //_host = await BuildHost();
        //await _host.Start();

        var jobExecutor = new SimpleJobExecutor();
        await jobExecutor.ScheduleJob(new ApplicationAccessibilityReporterJobConfiguration("LifeTracker.TelegramBot.Handlers", InstanceIdentifier.Identifier));
        await jobExecutor.StartExecuting();

        LoopConsoleClosing();
    }

    //private static Task<IHost> BuildHost()
    //{
    //    var configuration = ConfigurationHelper.GetConfiguration();

    //    IServiceCollection services = new ServiceCollection();
    //    services.AddDomain();
    //    services.AddMappers();
    //    services.AddConfiguration(configuration);
    //    services.AddSettings();
    //    services.AddScoped<IMessageHandler, TelegramBotMessageHandler>();
    //    services.AddScoped<IMessageSender<SentTelegramMessage>, TelegramMessageSender>();

    //    string botToken = configuration["Telegram:BotAPIKey"];
    //    services.AddTelegramClient(botToken);

    //    var provider = services.BuildServiceProvider();
    //    var host = provider.GetService<IHost>();
    //    return Task.FromResult(host);
    //}


    private static void LoopConsoleClosing()
    {
        var ended = new ManualResetEventSlim();
        var starting = new ManualResetEventSlim();

        AssemblyLoadContext.Default.Unloading += ctx =>
        {
            System.Console.WriteLine("Unloding fired");
            starting.Set();
            System.Console.WriteLine("Waiting for completion");
            ended.Wait();
        };

        System.Console.WriteLine("Waiting for signals");
        starting.Wait();

        System.Console.WriteLine("Received signal gracefully shutting down");
        Thread.Sleep(5000);
        ended.Set();
    }
}