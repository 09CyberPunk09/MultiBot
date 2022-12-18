using Application.TelegramBot.Pipelines.Old.MessagePipelines.ToDoList.Categories;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.ToDoList.ToDo
{
    [Route("/todo_info", "📋 ToDo List")]
    public class ToDoinfoPipeline : MessagePipelineBase
    {
        public ToDoinfoPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStage(() =>
            {
                return new()
                {
                    Text = "ToDo menu",
                    Menu = new(new KeyboardButton[][]
                {
                    MenuButtonRow(GetRoute<CreateToDoItempipeline>().AlternativeRoute),
                    MenuButtonRow(GetRoute<CategoriesInfoPipeline>().AlternativeRoute),
                    MenuButtonRow(GetRoute<GetToDoListPipeline>().AlternativeRoute)
                })
                };
            });
        }
    }
}
