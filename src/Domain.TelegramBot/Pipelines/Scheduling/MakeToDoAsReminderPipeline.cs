using Application.Services;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Reminder.Chunks;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.ToDoList.ToDo
{
    [Route("/create_reminder_from_todo", "Make a Rmeinder from ToDo")]
    public class MakeToDoAsReminderPipeline : MessagePipelineBase
    {
        private readonly ToDoAppService _todoService;
        private readonly ReminderAppService _reminderService;

        public MakeToDoAsReminderPipeline(ILifetimeScope scope) : base(scope)
        {
            _todoService = _scope.Resolve<ToDoAppService>();
            _reminderService = _scope.Resolve<ReminderAppService>();

            RegisterStage(AskAboutToDoItem);
            RegisterStage(AcceptNumberForReminder);
            IntegrateChunkPipeline<ScheduleReminderChunkPipeline>();
        }

        public ContentResult AskAboutToDoItem()
            => Text("Enter a number near a ToDo item which you want to make reminder:");

        public ContentResult AcceptNumberForReminder()
        {
            var numbers = GetCachedValue<Dictionary<int, Guid>>(GetToDoListPipeline.TODOSORDER_CACHEKEY);
            if (!int.TryParse(MessageContext.Message.Text, out var t) ||
                !numbers.TryGetValue(t, out var _))
            {
                Response.ForbidNextStageInvokation();
                return Text($"You must to enter a number which is in the range of todo items. If the list disaperared, enter {GetRoute<GetToDoListPipeline>().Route}.");
            }

            var todoItemId = numbers[t];
            var todoItem = _todoService.GetToDoItem(todoItemId);
            todoItem.IsDeleted = true;
            _todoService.UpdateToDoItem(todoItem);
            SetCachedValue(ScheduleReminderChunkPipeline.REMINDERTEXT_CACHEKEY, todoItem.Text);
            return new()
            {
                InvokeNextImmediately = true
            };
        }
    }
}
