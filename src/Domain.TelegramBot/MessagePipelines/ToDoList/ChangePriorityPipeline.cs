using Application.Services;
using Autofac;
using Common.Entites;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.TelegramBot.MessagePipelines.ToDoList
{
    [Route("/change_proprity", "🔛Change Priority")]
    public class ChangePriorityPipeline : MessagePipelineBase
    {
        private readonly NoteAppService _noteService;
        private readonly TagAppService _tagService;

        public ChangePriorityPipeline(ILifetimeScope scope) : base(scope)
        {
            _noteService = scope.Resolve<NoteAppService>();
            _tagService = scope.Resolve<TagAppService>();

            RegisterStage(AskAboutToDoItem);
            RegisterStage(AcceptPriorityChange);
        }

        public ContentResult AskAboutToDoItem(MessageContext ctx)
            => Text("Enter a number near a ToDo item which you want to make reminder:");

        public ContentResult AcceptPriorityChange(MessageContext ctx)
        {
            var numbers = GetCachedValue<Dictionary<int, Guid>>(GetToDoListPipeline.NOTESORDER_CACHEKEY);
            if (!int.TryParse(ctx.Message.Text, out var t) ||
                !numbers.TryGetValue(t, out var _))
            {
                ForbidMovingNext();
                return Text($"You must to enter a number which is in the range of todo items. If the list disaperared, enter {GetRoute<GetToDoListPipeline>().Route}.");
            }

            var noteId = numbers[t];
            var note = _noteService.Get(noteId);
            var firstProioriteTag = _tagService.Get(Tag.FirstPriorityToDoTagName, GetCurrentUser().Id);
            var secondProioriteTag = _tagService.Get(Tag.SecondPriorityToDoTagName, GetCurrentUser().Id);

            var newNote = _noteService.Create(note.Text, note.UserId);

            if (firstProioriteTag.Notes.Any(x => x.Id == note.Id))
            {
                secondProioriteTag.Notes.Add(newNote);
            }
            else
            {
                firstProioriteTag.Notes.Add(newNote);
            }
            _noteService.RemovePhysically(note);

            _tagService.Update(firstProioriteTag);
            _tagService.Update(secondProioriteTag);
            return Text("✅ Done.");
        }
    }
}
