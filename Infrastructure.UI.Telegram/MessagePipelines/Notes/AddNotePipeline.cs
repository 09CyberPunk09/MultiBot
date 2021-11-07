using Domain.Features.Notes;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using System.ComponentModel;

namespace Infrastructure.UI.TelegramBot.MessagePipelines
{
    [Route("/create-note")]
	[Description("Use this command for creating notes")]
	//TODO: REQWRITE TO: FIRST WE TELL THE USER TO ADD A MESSAGE AND THEN WE SAVE IT
	public class AddNotePipeline : MessagePipelineBase
	{
		private readonly MediatR.IMediator _mediator;
        public AddNotePipeline(MediatR.IMediator mediator)
        {
            (_mediator) = (mediator);
        }

        public override void RegisterPipelineStages()
		{
			Stages.Add(SuggestToEnterANote);
			Stages.Add(SaveNote);
			IsLooped = true;
		}

		private ContentResult SuggestToEnterANote(MessageContext ctx)
        {
			return Text("Enter Note text:");
		}

		private ContentResult SaveNote(MessageContext ctx)
		{

			_mediator.Send(new CreateNoteRequest() { Text = ctx.Message.Text });

			return Text("✅ Note saved");
		}


	}
}
