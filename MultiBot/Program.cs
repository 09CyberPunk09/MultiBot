using Infrastructure.UI.TelegramBot;
using System;

namespace MultiBot
{
	class Program
	{
		static void Main(string[] args)
		{
			var telegramBot = new TelegramBotHandlerInstance();
			telegramBot.Start();

			Console.WriteLine("The bot started succesfully");
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
