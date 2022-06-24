using Application.Services;
using Autofac;
using Common.Entites;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Domain.TelegramBot.MessagePipelines.ToDoList
{
    [Route("/get_todo", "📋 My ToDo List")]
    public class GetToDoListPipeline : MessagePipelineBase
    {
        public const string TODOSORDER_CACHEKEY = "ToDosOrder";
        public GetToDoListPipeline(ILifetimeScope scope) : base(scope)
        {
            var todoService = scope.Resolve<ToDoAppService>();
            RegisterStage(ctx =>
            {
                var userId = GetCurrentUser().Id;
                var categories = todoService.GetAllCategories(userId, true);
                var sb = new StringBuilder();
                int counter = 0;
                var orders = new Dictionary<int, Guid>();
                if (categories.Any())
                {
                    sb.AppendLine("Yor TODOs:");
                    categories.ToList()
                    .ForEach(c =>
                    {
                        //TODO: Додати щоб одна категорія була синя,друга - зелена
                        sb.AppendLine($"🔶{ c.Name}");
                        c.ToDoItems.ForEach(ti =>
                        {
                            sb.AppendLine($"   🔸 {++counter}.{ ti.Text}");
                            orders.Add(counter, ti.Id);
                        });
                    });
                }
                else
                {
                    sb.Append("No ToDos yet");
                }
                SetCachedValue(TODOSORDER_CACHEKEY, orders);
                return new()
                {
                    Text = sb.ToString(),
                    Menu = new(new KeyboardButton[][]
                    {
                        MenuButtonRow(
                            MenuButton(GetAlternativeRoute<CreateToDoItempipeline>()),
                            MenuButton(GetAlternativeRoute<MakeToDoAsReminderPipeline>())),
                        MenuButtonRow(GetAlternativeRoute<RemoveToDoItemPipeline>())
                    })
                };
            });
        }

    }
}
