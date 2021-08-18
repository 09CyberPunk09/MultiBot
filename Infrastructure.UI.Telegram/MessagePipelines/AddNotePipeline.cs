using Domain.Features.Notes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using Infrastructure.UI.TelegramBot.ResponseTypes;
using System.Linq;

namespace Infrastructure.UI.TelegramBot.MessagePipelines
{
	public class AddNotePipeline : MessagePipelineBase
	{
		private readonly MediatR.IMediator _mediator;
		public AddNotePipeline(MediatR.IMediator mediator) =>
			(_mediator) = (mediator);
		public override void RegisterPipelineStages()
		{
			InitBaseComponents();
			Stages.Add(SaveNote);
			Current = Stages.First();
			CurrentActionIndex = 0;
			IsLooped = true;
		}

		private IContentResult SaveNote(IMessageContext ctx)
		{

			_mediator.Send(new CreateNoteRequest() { Text = ctx.Message as string });
			//new CreateNoteRequest() { Text = (ctx.Message as Telegram.Bot.Types.Message).Text }
			return new TextResult()
			{
				TextMessage = new Message()
				{
					Text = "Yeah"
				}
			};
		}


	}
}
