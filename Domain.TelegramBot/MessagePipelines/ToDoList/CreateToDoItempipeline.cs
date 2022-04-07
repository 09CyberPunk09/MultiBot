using Application.Services;
using Autofac;
using Common.Entites;
using Infrastructure.TextUI.Core.PipelineBaseKit;

namespace Domain.TelegramBot.MessagePipelines.ToDoList
{
    [Route("/create_todo", "📋➕ Add ToDo")]
    //TODO: Add remove todo pipeline
    public class CreateToDoItempipeline : MessagePipelineBase
    {
        private const string NOTE_TEXT_CACHEKEY = "NoteText";
        private readonly NoteAppService _noteService;
        private readonly TagAppService _tagService;
        public CreateToDoItempipeline(ILifetimeScope scope) : base(scope)
        {
            _noteService = scope.Resolve<NoteAppService>();
            _tagService = scope.Resolve<TagAppService>();

            RegisterStage(AskText);
            RegisterStage(AccptNewToDo);
            RegisterStage(AccpetPriorityAndSave);
        }

        public ContentResult AskText(MessageContext ctx)
            => Text("Enter note text:");

        public ContentResult AccptNewToDo(MessageContext ctx)
        {
            SetCachedValue(NOTE_TEXT_CACHEKEY, ctx.Message.Text);
            return new()
            {
                Text = "Select priority:",
                Buttons = new(new[] { Button("First", (1).ToString()), Button("Second", (2).ToString()) })
            };
        }

        public ContentResult AccpetPriorityAndSave(MessageContext ctx)
        {
            if (int.TryParse(ctx.Message.Text, out int t))
            {
                string tagName = "";
                switch (t)
                {
                    case 1:
                        tagName = Tag.FirstPriorityToDoTagName;
                        break;
                    case 2:
                        tagName = Tag.SecondPriorityToDoTagName;
                        break;
                    default:
                        break;
                }

                var tag = _tagService.Get(tagName, GetCurrentUser().Id);
                _tagService.CreateNoteUnderTag(tag.Id, GetCachedValue<string>(NOTE_TEXT_CACHEKEY), GetCurrentUser().Id);
                return Text("✅ Done.");
            }
            else
            {
                ForbidMovingNext();
                return Text("Please, select one form menu");
            }
        }

    }
}
