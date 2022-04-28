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

        private readonly NoteAppService _noteService;
        private readonly TagAppService _tagService;
        public RemoveToDoItemPipeline(ILifetimeScope scope) : base(scope)
        {
            _noteService = scope.Resolve<NoteAppService>();
            _tagService = scope.Resolve<TagAppService>();

            RegisterStage(AskAboutToDoItem);
            RegisterStage(AskAboutAction);
            RegisterStage(AcceptChoice);
        }

        public ContentResult AskAboutToDoItem(MessageContext ctx)
        => Text("Enter a number near a ToDo item which you want to delete:");

        public ContentResult AskAboutAction(MessageContext ctx)
        {
            var numbers = GetCachedValue<Dictionary<int, Guid>>(GetToDoListPipeline.NOTESORDER_CACHEKEY,true);
            if (!int.TryParse(ctx.Message.Text, out var t) ||
                !numbers.TryGetValue(t, out var noteId))
            {
                ForbidMovingNext();
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

        public ContentResult AcceptChoice(MessageContext ctx)
        {
            if (!Enum.TryParse<Choice>(ctx.Message.Text, out var ch))
            {
                ForbidMovingNext();
                return Text("Please, select a value from the menu");
            }

            var noteId = GetCachedValue<Guid>(SELECTEDITEMID_CACHEKEY,true);
            var note = _noteService.Get(noteId);
            var user = GetCurrentUser();
            var doneTag = _tagService.Get(Tag.DoneToDoTagName, user.Id);

            string noteText = note.Text;

            _noteService.RemovePhysically(note);

            if (ch == Choice.MarkAsDone)
            {
                var newNote = _noteService.Create(noteText, user.Id);
                doneTag.Notes.Add(newNote);
                _tagService.Update(doneTag);
            }

            return Text("✅ Done");
        }
    }
}
