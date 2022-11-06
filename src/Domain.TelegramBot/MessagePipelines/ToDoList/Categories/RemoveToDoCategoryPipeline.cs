using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Collections.Generic;
using System.Text;
using System;
using Telegram.Bot.Types.ReplyMarkups;

namespace Domain.TelegramBot.MessagePipelines.ToDoList.Categories
{
    [Route("/delete_todo_category", "❌ Remove Category")]
    public class RemoveToDoCategoryPipeline : MessagePipelineBase
    {
        private const string CATEGORY_ID_CACHEKEY = "CategoryIdToRemove";
        private readonly ToDoAppService _service;
        public RemoveToDoCategoryPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<ToDoAppService>();
            RegisterStage(AskForActivity);
            RegisterStage(Confirm);
            RegisterStage(Remove);
        }

        public ContentResult AskForActivity()
        {
            return Text("Now, enter a number near your desicion to delete the activity");
        }

        public ContentResult Confirm()
        {
            var dict = GetCachedValue<Dictionary<int, Guid>>(CategoriesInfoPipeline.TODO_CATEGORIES_CACHEKEY);
            if (!(int.TryParse(MessageContext.Message.Text, out var number) && (number >= 0 && number <= dict.Count)))
            {
                Response.ForbidNextStageInvokation();
                return Text("⛔️ Enter a number form the suggested list");
            }

            var id = dict[number];
            SetCachedValue(CATEGORY_ID_CACHEKEY, id.ToString());

            return new()
            {
                Text = "Are you sure you want to delete?",
                Buttons = new(new[]
                {
                    InlineKeyboardButton.WithCallbackData("🟩 Yes",true.ToString()),
                    InlineKeyboardButton.WithCallbackData("🟥 No",false.ToString()),
                })
            };
        }

        public ContentResult Remove()
        {
            if (bool.TryParse(MessageContext.Message.Text, out var result))
            {
                if (result)
                {
                    _service.DeleteToDoCategory(GetCachedValue<Guid>(CATEGORY_ID_CACHEKEY, true));
                    return Text("✅ Done");
                }
            }
            return Text("Canceled");
        }
    }
}
