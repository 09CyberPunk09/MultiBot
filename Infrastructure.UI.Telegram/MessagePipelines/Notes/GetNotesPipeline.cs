using Application.Services;
using Autofac;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using Infrastructure.UI.TelegramBot.ResponseTypes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.UI.TelegramBot.MessagePipelines
{
    [Route("/get-notes")]
	[Description("Get's all your notes")]
	public class GetNotesPipeline : MessagePipelineBase
	{
		private readonly NoteAppService _noteService;
        public GetNotesPipeline(NoteAppService noteService, ILifetimeScope scope) : base(scope)
		{
			_noteService = noteService;
		}

        public override void RegisterPipelineStages()
		{
			RegisterStage(GetNotes);
			IsLooped = true;
		}

		private ContentResult GetNotes(MessageContext ctx)
		{
			var result = _noteService.GetByUserId(GetCurrentUser().Id);
			//todo: add markup
			var messagesToSend = new List<BotMessage>()
			{
				new BotMessage(){ Text = "Here are your notes:"}
			};

			messagesToSend.AddRange(result.Select(x => new BotMessage() { 
				Text = x.Text,
				Buttons = new InlineKeyboardMarkup(
						new[]
						{
							new[]
							{
								InlineKeyboardButton.WithCallbackData("Delete","")
							}
						})
			}));

			return new MultiMessageResult()
			{
				Messages = messagesToSend
			};
		}
	}
}
