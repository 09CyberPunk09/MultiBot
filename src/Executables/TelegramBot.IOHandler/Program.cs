using Application;
using Integration.Applications;
using NLog;
using SimpleScheduler;
using System;
using System.Threading.Tasks;

namespace LifeTracker.TelegramBot.IOHandler
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            IOHandler handler = new();
            handler.Start();
            logger.Info("Logger started successfully");

            // TODO: Code repeat
            // TODO: Implement a applicaitonBuilder
            var jobExecutor = new SimpleJobExecutor();
            await jobExecutor.ScheduleJob(new ApplicationAccessibilityReporterJobConfiguration("LifeTracker.TelegramBot.IOHandler", InstanceIdentifier.Identifier));
            await jobExecutor.StartExecuting();

            // TODO: Code repeat
            LoopConsoleClosing();
        }

        private static void LoopConsoleClosing()
        {
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            { }
            Console.WriteLine("");
        }
    }
}