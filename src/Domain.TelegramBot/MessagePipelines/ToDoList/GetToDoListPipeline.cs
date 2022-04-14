using Application.Services;
using Autofac;
using Common.Entites;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Domain.TelegramBot.MessagePipelines.ToDoList
{
    [Route("/get_todo", "📋 ToDo List")]
    public class GetToDoListPipeline : MessagePipelineBase
    {
        public const string NOTESORDER_CACHEKEY = "NotesOrder";
        public GetToDoListPipeline(ILifetimeScope scope) : base(scope)
        {
            var tagService = scope.Resolve<TagAppService>();
            RegisterStage(ctx =>
            {
                var userId = GetCurrentUser().Id;
                var firstPriorityNotes = tagService.Get(Tag.FirstPriorityToDoTagName, userId);
                var secondPriorityNotes = tagService.Get(Tag.SecondPriorityToDoTagName, userId);
                var sb = new StringBuilder();
                sb.AppendLine("First Priorite ToDo List:");
                sb.AppendLine(GenerateList(firstPriorityNotes.Notes));
                sb.AppendLine();
                sb.AppendLine("Second Priorite ToDo List:");
                sb.AppendLine(GenerateList(secondPriorityNotes.Notes));
                SetCachedValue(NOTESORDER_CACHEKEY, orders);

                return new()
                {
                    Text = sb.ToString(),
                    Menu = new(new KeyboardButton[][]
                    {
                        MenuButtonRow(
                            MenuButton(GetAlternativeRoute<CreateToDoItempipeline>()),
                            MenuButton(GetAlternativeRoute<MakeToDoAsReminderPipeline>())),
                        MenuButtonRow(GetAlternativeRoute<ChangePriorityPipeline>()),
                        MenuButtonRow(GetAlternativeRoute<RemoveToDoItemPipeline>())
                    })
                };
            });
        }

        private int counter = 0;
        private Dictionary<int, Guid> orders = new();
        private string GenerateList(List<Note> notes)
        {
            var t = new StringBuilder();
            notes.ForEach(x =>
            {
                t.AppendLine($"{++counter}. {x.Text}");
                orders.Add(counter, x.Id);
            });
            return t.ToString();
        }

    }
}
