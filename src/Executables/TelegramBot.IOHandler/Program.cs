using NLog;
using System;

namespace TelegramBot.IOHandler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            IOHandler handler = new();
            handler.Start();
            logger.Info("Logger started successfully");
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