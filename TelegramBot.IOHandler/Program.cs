using System;

namespace TelegramBot.IOHandler
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IOHandler handler = new();
            handler.Start();
            Console.WriteLine("Handler started successfully");
            LoopConsoleClosing();
        }

        static void LoopConsoleClosing()
        {
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            { }
            Console.WriteLine("");
        }
    }
}
