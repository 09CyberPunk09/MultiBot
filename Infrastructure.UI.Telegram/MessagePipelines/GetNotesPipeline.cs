using Domain.Features.Notes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.TelegramBot.ResponseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UI.TelegramBot.MessagePipelines
{
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
			var messagesToSend = new List<Message>()
			{
				new Message(){ Text = "Here are your notes:"}
			};

			messagesToSend.AddRange(result.Result.Select(x => new Message() { Text = x.Text }));

			return new MultiMessageResult() 
			{ 
			 Messages = messagesToSend
			};
		}
	}
}
