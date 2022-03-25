using Infrastructure.TelegramBot.IOInstances;
using NLog;
using System;

namespace MultiBot
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Starting...");
            var telegramBot = new TelegramBotHandlerInstance();
            telegramBot.Start();
            logger.Info("The message handler started succesfully");

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