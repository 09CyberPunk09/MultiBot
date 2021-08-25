using Infrastructure.UI.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.UI.TelegramBot.ResponseTypes
{
	public class BotMessage : IMessage
	{
		public string Text { get; set; }
		public InlineKeyboardMarkup Buttons { get; set; }
	}
}
