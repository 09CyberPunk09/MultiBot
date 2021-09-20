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
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.UI.TelegramBot.MessagePipelines.Sets
{
    [Route("/get-set")]
    public class GetSetDataPipeline : MessagePipelineBase
	{
		private readonly MediatR.IMediator _mediator;
		public GetSetDataPipeline(MediatR.IMediator mediator) =>
			(_mediator) = (mediator);
		public override void RegisterPipelineStages()
		{
			InitBaseComponents();
		//	Stages.Add(AskForSetName);
		//	Stages.Add(AcceptSetName);

			Current = Stages.First();
			CurrentActionIndex = 0;
		}

		public IContentResult ChooseASet(IMessageContext ctx)
        {
			var userSets =  _mediator.Send(new GetUserSetsRequest());
			var markups = new List<InlineKeyboardButton>();
            foreach (var item in userSets.Result.Result)
            {
				markups.Add(InlineKeyboardButton.WithCallbackData(item.Name, item.Id.ToString()));
            }
			//		 var cts = db.Categories.ToList().Where(x => x.UserID == ch.Id);

			//foreach (var item in cts)
			//{
			//	List<InlineKeyboardButton> categoryNames = new List<InlineKeyboardButton>();
			//	categoryNames.Add(InlineKeyboardButton.WithCallbackData("Нотатки", $"/get_categorynotes {item.ID}"));
			//	categoryNames.Add(InlineKeyboardButton.WithCallbackData("Видалити", $"/delete_category {item.ID}"));
			//	var markup = new InlineKeyboardMarkup(categoryNames.ToArray());

			//}
			return new BotMessage()
			{
				Text = "Choose the set you want to open:",
				Buttons = new InlineKeyboardMarkup(markups.ToArray())
			};

		}
}
