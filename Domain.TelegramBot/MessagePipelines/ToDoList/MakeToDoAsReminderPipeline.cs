using Application.Services;
using Autofac;
using Domain.TelegramBot.MessagePipelines.Scheduling.Chunks;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;

namespace Domain.TelegramBot.MessagePipelines.ToDoList
{
    [Route("/create_reminder_from_todo","Make a Rmeinder from ToDo")]
    public class MakeToDoAsReminderPipeline : MessagePipelineBase
    {
        private readonly NoteAppService _noteService;
        private readonly ReminderAppService _reminderService;

        public MakeToDoAsReminderPipeline(ILifetimeScope scope) : base(scope)
        {
            _noteService = _scope.Resolve<NoteAppService>();
            _reminderService = _scope.Resolve<ReminderAppService>();

            RegisterStage(AskAboutToDoItem);
            RegisterStage(AcceptNumberForReminder);
            IntegrateChunkPipeline<ScheduleReminderChunkPipeline>();
        }

        public ContentResult AskAboutToDoItem(MessageContext ctx)
            => Text("Enter a number near a ToDo item which you want to make reminder:");

        public ContentResult AcceptNumberForReminder(MessageContext ctx)
        {
            var numbers = GetCachedValue<Dictionary<int, Guid>>(GetToDoListPipeline.NOTESORDER_CACHEKEY);
            if(!int.TryParse(ctx.Message.Text,out var t) || 
                !numbers.TryGetValue(t,out var _))
            {
                ForbidMovingNext();
                return Text($"You must to enter a number which is in the range of todo items. If the list disaperared, enter {GetRoute<GetToDoListPipeline>().Route}.");
            }

            var noteId = numbers[t];
            var note = _noteService.Get(noteId);
            SetCachedValue(ScheduleReminderChunkPipeline.REMINDERTEXT_CACHEKEY, note.Text);
            return new()
            {
                InvokeNextImmediately = true
            };
        }
    }
}
