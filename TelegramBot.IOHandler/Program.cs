using System;

namespace TelegramBot.IOHandler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IOHandler handler = new();
            handler.Start();
            Console.WriteLine("Handler started successfully");
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