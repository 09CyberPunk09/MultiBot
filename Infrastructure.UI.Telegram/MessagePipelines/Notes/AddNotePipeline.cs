using Domain.Features.Notes;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.TelegramBot.ResponseTypes;
using System.ComponentModel;
using System.Linq;

namespace Infrastructure.UI.TelegramBot.MessagePipelines
{
	[Route("/create-note")]
	[Description("Use this command for creating notes")]
	//TODO: REQWRITE TO: FIRST WE TELL THE USER TO ADD A MESSAGE AND THEN WE SAVE IT
	public class AddNotePipeline : MessagePipelineBase
	{
		private readonly MediatR.IMediator _mediator;
		public AddNotePipeline(MediatR.IMediator mediator) =>
			(_mediator) = (mediator);
		public override void RegisterPipelineStages()
		{
			InitBaseComponents();
			Stages.Add(SuggestToEnterANote);
			Stages.Add(SaveNote);
			Current = Stages.First();
			CurrentActionIndex = 0;
			IsLooped = true;
		}

		private IContentResult SuggestToEnterANote(IMessageContext ctx)
        {
			return new TextResult()
			{
				TextMessage = new BotMessage()
				{
					Text = "Enter Note text:"
				}
			};
		}

		private IContentResult SaveNote(IMessageContext ctx)
		{

			_mediator.Send(new CreateNoteRequest() { Text = ctx.Message as string });

			return new TextResult()
			{
				TextMessage = new BotMessage()
				{
					Text = "✅ Note saved"
				}
			};
		}


	}
}
