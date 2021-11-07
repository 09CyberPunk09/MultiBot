using Domain.Features.Sets;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using Infrastructure.UI.TelegramBot.ResponseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.UI.TelegramBot.MessagePipelines.Sets
{
    [Route("/get-set")]
	public class GetSetDataPipeline : MessagePipelineBase
	{
		private readonly MediatR.IMediator _mediator;
        public GetSetDataPipeline(MediatR.IMediator mediator)
        {
            (_mediator) = (mediator);
        }

        public override void RegisterPipelineStages()
		{
			Stages.Add(AskForSetName);
			Stages.Add(ReturnASet);
		}

		public ContentResult AskForSetName(MessageContext ctx)
		{
			var userSets = _mediator.Send(new GetUserSetsRequest());
			var markups = new List<InlineKeyboardButton>();
			foreach (var item in userSets.Result.Result)
			{
				markups.Add(InlineKeyboardButton.WithCallbackData(item.Name, item.Id.ToString()));
			}

			return new BotMessage()
			{
				Text = "Choose the set you want to open:",
				Buttons = new InlineKeyboardMarkup(markups.ToArray())
			};
		}

		private ContentResult ReturnASet(MessageContext ctx)
        {
			string set = ctx.Message.Text;
			var datas = _mediator.Send(new GetSetDataRequest() {  SetId = Guid.Parse(set) });
			if(datas != null)
            {
				var newLine = Environment.NewLine;
				StringBuilder messageBuilder = new StringBuilder();

				messageBuilder.Append(set + newLine + newLine);

				var resultingSet = datas.Result.Result;
                if (resultingSet.Count > 0)
                {
					//sort the set by creationdate
					resultingSet.OrderBy(x => x.ModificationTime);

					for (int i = 0; i < resultingSet.Count; i++)
					{
						messageBuilder.Append($"{i}.{resultingSet[i]}{newLine}");
					}
				}

				return Text(messageBuilder.ToString());
            }
            else
            {
				return Text("The set with the specified name is not found.");
            }
        }
	}
}