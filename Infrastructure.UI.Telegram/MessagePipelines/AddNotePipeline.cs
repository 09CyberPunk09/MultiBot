using Domain.Features.Notes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			Current = Stages.First();
			CurrentActionIndex = 0;
			IsLooped = true;
		}

		private IContentResult SaveNote(IMessageContext ctx)
		{

			_mediator.Send(new CreateNoteRequest());
			//new CreateNoteRequest() { Text = (ctx.Message as Telegram.Bot.Types.Message).Text }
			return null;
		}


	}
}
