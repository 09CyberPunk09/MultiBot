using Infrastructure.TelegramBot.IOInstances;
using System;

namespace MultiBot
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var telegramBot = new TelegramBotHandlerInstance();
            telegramBot.Start();
            Console.WriteLine("The bot started succesfully");

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