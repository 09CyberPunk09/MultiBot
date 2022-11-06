using Application.Services;
using Autofac;
using Common.Entites;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;

namespace Domain.TelegramBot.MessagePipelines.ToDoList
{
    enum Choice
    {
        Remove,
        MarkAsDone
    }

    [Route("/remove_todo", "🗑 Remove")]
    public class RemoveToDoItemPipeline : MessagePipelineBase
    {
        public const string SELECTEDITEMID_CACHEKEY = "Selected Item Number To Delete";

        private readonly ToDoAppService _todoService;
        public RemoveToDoItemPipeline(ILifetimeScope scope) : base(scope)
        {
            _todoService = scope.Resolve<ToDoAppService>();

            RegisterStageMethod(AskAboutToDoItem);
            RegisterStageMethod(AskAboutAction);
            RegisterStageMethod(AcceptChoice);
        }

        public ContentResult AskAboutToDoItem()
        => Text("Enter a number near a ToDo item which you want to delete:");

        public ContentResult AskAboutAction()
        {
            var numbers = GetCachedValue<Dictionary<int, Guid>>(GetToDoListPipeline.TODOSORDER_CACHEKEY);
            if (!int.TryParse(MessageContext.Message.Text, out var t) ||
                !numbers.TryGetValue(t, out var noteId))
            {
                Response.ForbidNextStageInvokation();
                return Text($"You must to enter a number which is in the range of todo items. If the list disaperared, enter {GetRoute<GetToDoListPipeline>().Route}.");
            }
            SetCachedValue(SELECTEDITEMID_CACHEKEY, noteId);
            return new()
            {
                Text = "Noew,Select in what way you want to remove ToDo item from the list:",
                Buttons = new(new[]{ Button("✅Mark as Done",((int)(Choice.MarkAsDone)).ToString()),
                                     Button("❌Remove",((int)(Choice.Remove)).ToString())
                })
            };
        }

        public ContentResult AcceptChoice()
        {
            if (!Enum.TryParse<Choice>(MessageContext.Message.Text, out var ch))
            {
                Response.ForbidNextStageInvokation();
                return Text("Please, select a value from the menu");
            }

            var todoItemId = GetCachedValue<Guid>(SELECTEDITEMID_CACHEKEY,true);
            var todoItem = _todoService.GetToDoItem(todoItemId);
            if (ch == Choice.MarkAsDone)
            {
                todoItem.IsDone = true;
                _todoService.UpdateToDoItem(todoItem);
            }
            else
            {
                _todoService.DeleteToDoItem(todoItem);
            }
            return Text("✅ Done");
        }
    }
}
