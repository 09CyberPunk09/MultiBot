using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.ToDoList.ToDo
{
    [Route("/new_todo", "📋➕ Add ToDo")]
    public class CreateToDoItempipeline : MessagePipelineBase
    {
        private const string NOTE_TEXT_CACHEKEY = "NoteText";
        private const string CATEGORIES_LIST_CACHEKEY = "CategoriesList";
        private readonly ToDoAppService _todoService;
        public CreateToDoItempipeline(ILifetimeScope scope) : base(scope)
        {
            _todoService = scope.Resolve<ToDoAppService>();

            RegisterStage(AskText);
            RegisterStage(AccpetPriorityAndSave);
            RegisterStage(AcceptCategoryAndSaveToDo);
        }

        public ContentResult AskText()
            => Text("Enter note text:");

        public ContentResult AccpetPriorityAndSave()
        {
            SetCachedValue(NOTE_TEXT_CACHEKEY, MessageContext.Message.Text);

            var categories = _todoService.GetAllCategories(MessageContext.User.Id);
            var b = new StringBuilder();
            b.AppendLine("Your Categories: ");

            int counter = 0;
            var dictionary = new Dictionary<int, Guid>();

            foreach (var item in categories)
            {
                ++counter;
                b.AppendLine($"🔷 {counter}. {item.Name}");
                dictionary[counter] = item.Id;
            }

            SetCachedValue(CATEGORIES_LIST_CACHEKEY, dictionary);

            return new()
            {
                Text = b.ToString(),
                MultiMessages = new()
                {
                    new()
                    {
                        Text = "Enter a number near the category of the ToDo:"
                    }
                }
            };
        }

        public ContentResult AcceptCategoryAndSaveToDo()
        {
            var dict = GetCachedValue<Dictionary<int, Guid>>(CATEGORIES_LIST_CACHEKEY, true);
            if (!(int.TryParse(MessageContext.Message.Text, out var number) && number >= 0 && number <= dict.Count))
            {
                Response.ForbidNextStageInvokation();
                return Text("⛔️ Enter a number form the suggested list");
            }

            var categoryId = dict[number];
            var todoItemText = GetCachedValue<string>(NOTE_TEXT_CACHEKEY, true);
            _todoService.CreateItem(MessageContext.User.Id, categoryId, todoItemText);

            return Text("✅ Done.");
        }

    }
}
