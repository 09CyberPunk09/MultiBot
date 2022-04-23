using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Telegram.Bot.Types.ReplyMarkups;

namespace Domain.TelegramBot.MessagePipelines.ToDoList
{
    [Route("/todo_info", "📋 ToDo List")]
    public class ToDoinfoPipeline : MessagePipelineBase
    {
        public ToDoinfoPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStage(ctx =>
            {
                return new()
                {
                    Text = "ToDo menu",
                    Menu = new(new KeyboardButton[][]
                {
                    MenuButtonRow(GetRoute<CreateToDoItempipeline>().AlternativeRoute),
                    MenuButtonRow(GetRoute<GetToDoListPipeline>().AlternativeRoute)
                })
                };
            });
        }
    }
}
