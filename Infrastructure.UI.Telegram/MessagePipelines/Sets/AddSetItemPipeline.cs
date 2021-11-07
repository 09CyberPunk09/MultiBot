using Domain.Features.Sets;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using Infrastructure.UI.TelegramBot.ResponseTypes;
using Persistence.Caching.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.UI.TelegramBot.MessagePipelines.Sets
{
    [Route("/create-setitem")]
	public class AddSetItemPipeline : MessagePipelineBase
	{
		private readonly MediatR.IMediator _mediator;
        public AddSetItemPipeline(MediatR.IMediator mediator, ICache cache)
        {
            (_mediator, _cache) = (mediator, cache);
        }

        public override void RegisterPipelineStages()
		{
			Stages.Add(AskForSetName);
			Stages.Add(ShowExistingListAndAllowAdding);
			Stages.Add(AddASetItem);
		}

		//todo: there is in a get set pipeline the same piece of code.Refactor it later.
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
				Text = "Choose the set you want to add an element to:",
				Buttons = new InlineKeyboardMarkup(markups.ToArray())
			};
		}

		public ContentResult ShowExistingListAndAllowAdding(MessageContext ctx)
        {
			string setId = ctx.Message.Text;
			var datas = _mediator.Send(new GetSetDataRequest() { SetId = Guid.Parse(setId) });
			
			if (datas != null)
			{
				var newLine = Environment.NewLine;
				StringBuilder messageBuilder = new();
				
				var set = _mediator.Send(new GetUserSetsRequest()).Result.Result.FirstOrDefault(x => x.Id == Guid.Parse(setId));

				messageBuilder.Append(set.Name + newLine + newLine);

				var resultingSet = datas.Result.Result;
				if (resultingSet.Count > 0)
				{
					resultingSet = resultingSet.OrderBy(x => x.CreationDate).ToList();

					for (int i = 0; i < resultingSet.Count; i++)
					{
						messageBuilder.Append($"{i}.{resultingSet[i]}{newLine}");
					}

				}
				messageBuilder.Append(newLine + "Enter a new item to the set:");

				SetValueToCache("SelectedSet", set.Id);

				return Text(messageBuilder.ToString());
			}
			else
			{
				return Text("The set with the specified name is not found.");
			}
		}

		public ContentResult AddASetItem(MessageContext ctx)
        {
			var setId = GetCachedValue<Guid>("SelectedSet");
			var itemData = ctx.Message.Text;
			_mediator.Send(new AddSetItemRequest() { Content = itemData, SetId = setId });
			return Text("Done.");
        }

	}
}
