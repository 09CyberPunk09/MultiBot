using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.TelegramBot;
using System;

namespace MultiBot
{
	class Program
	{
		static void Main(string[] args)
		{
			IHandlerInstance telegramBot = new TelegramBotHandlerInstance();

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
