using Domain.Features.Sets;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using Infrastructure.UI.TelegramBot.ResponseTypes;
using System;

namespace Infrastructure.UI.TelegramBot.MessagePipelines.Sets
{
    [Route("/new-set")]
    public class AddSetPipeline : MessagePipelineBase
    {
		private readonly MediatR.IMediator _mediator;
        public AddSetPipeline(MediatR.IMediator mediator)
        {
            (_mediator) = (mediator);
        }

        public override void RegisterPipelineStages()
		{
			Stages.Add(AskForSetName);
			Stages.Add(AcceptSetName);
		}

		private ContentResult AskForSetName(MessageContext ctx)
        {
			return new TextResult()
			{
				Text = "You are creating a new set.Enter it's name:"
			};
		}
		private ContentResult AcceptSetName(MessageContext ctx)
		{
            try
            {
				_mediator.Send(new CreateSetRequest() { Name = ctx.Message.Text });
				return Text("✅ Set created.");
			}
            catch (Exception)
            {
				return Text("Invalid set name or something else went wrong.");
				throw;
            }
			
		}
	}
}
