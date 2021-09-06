using Domain.Features.Notes;
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
		private readonly MediatR.IMediator _mediator;
		public GetNotesPipeline(MediatR.IMediator mediator) =>
			(_mediator) = (mediator);
		public override void RegisterPipelineStages()
		{
			InitBaseComponents();
			Stages.Add(GetNotes);
			Current = Stages.First();
			CurrentActionIndex = 0;
			IsLooped = true;
		}

		private IContentResult GetNotes(IMessageContext ctx)
		{
			var result = _mediator.Send(new GetNoteRequest()).Result;
			//todo: add markup
			var messagesToSend = new List<BotMessage>()
			{
				new BotMessage(){ Text = "Here are your notes:"}
			};

			messagesToSend.AddRange(result.Result.Select(x => new BotMessage() { 
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
