using Domain.Features.Notes;
using Domain.Features.Sets;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.TelegramBot.ResponseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UI.TelegramBot.MessagePipelines.Sets
{
    [Route("/new-set")]
    public class AddSetPipeline : MessagePipelineBase
    {
		private readonly MediatR.IMediator _mediator;
		public AddSetPipeline(MediatR.IMediator mediator) =>
			(_mediator) = (mediator);

		public override void RegisterPipelineStages()
		{
			InitBaseComponents();
			Stages.Add(AskForSetName);
			Stages.Add(AcceptSetName);

			Current = Stages.First();
			CurrentActionIndex = 0;
		}

		private IContentResult AskForSetName(IMessageContext ctx)
        {
			return new TextResult()
			{
				TextMessage = new BotMessage()
				{
					Text = "You are creating a new set.Enter it's name:"
				}
			};
		}
		private IContentResult AcceptSetName(IMessageContext ctx)
		{
            try
            {
				_mediator.Send(new CreateSetRequest() { Name = ctx.Message as string });
				return new TextResult()
				{
					TextMessage = new BotMessage()
					{
						Text = "✅ Set created."
					}
				};
			}
            catch (Exception)
            {
				return new TextResult()
				{
					TextMessage = new BotMessage()
					{
						Text = "Invalid set name or something else went wrong."
					}
				};
				throw;
            }
			
		}
	}
}
