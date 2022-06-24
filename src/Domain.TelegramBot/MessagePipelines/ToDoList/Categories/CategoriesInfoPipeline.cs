using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Collections.Generic;
using System.Text;
using System;
using Telegram.Bot.Types.ReplyMarkups;
using Application.Services;
using System.Linq;

namespace Domain.TelegramBot.MessagePipelines.ToDoList.Categories
{
    [Route("/manage_todo_categories", "Manage ToDo Categories")]
    public class CategoriesInfoPipeline : MessagePipelineBase
    {
        public const string TODO_CATEGORIES_CACHEKEY = "TODO_CATEGORIES_CACHEKEY";
        public CategoriesInfoPipeline(ILifetimeScope scope) : base(scope)
        {
            var todoService = scope.Resolve<ToDoAppService>();
            RegisterStage(ctx =>
            {
                var userId = GetCurrentUser().Id;
                var categories = todoService.GetAllCategories(userId, true);
                var sb = new StringBuilder();
                sb.AppendLine("Your TODO items: ");
                int counter = 0;
                var orders = new Dictionary<int, Guid>();
                if (categories.Any())
                {
                    categories.ToList()
                    .ForEach(c =>
                    {
                        sb.AppendLine($"🔸 {++counter}.{ c.Name}");
                        orders.Add(counter, c.Id);
                    });
                }
                else
                {
                    sb.AppendLine("No ToDos yet");
                }
                SetCachedValue(TODO_CATEGORIES_CACHEKEY, orders);
                return new()
                {
                    Text = sb.ToString(),
                    Menu = new(new KeyboardButton[][]
                {
                    MenuButtonRow(GetRoute<CreateToDoCategoryPipeline>().AlternativeRoute),
                    MenuButtonRow(GetRoute<RemoveToDoCategoryPipeline>().AlternativeRoute),
                })
                };
            });
        }
    }
}
