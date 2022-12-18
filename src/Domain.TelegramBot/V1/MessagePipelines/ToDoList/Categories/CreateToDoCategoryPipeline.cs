using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.ToDoList.Categories
{
    [Route("/new_todo_category", "➕ Create Category")]
    //For Bug Reproduce
    //[Route("/create_todo_category", "➕ Create Category")]
    public class CreateToDoCategoryPipeline : MessagePipelineBase
    {
        private readonly ToDoAppService _todoService;
        public CreateToDoCategoryPipeline(ILifetimeScope scope) : base(scope)
        {
            _todoService = scope.Resolve<ToDoAppService>();
            //TODO: BUG: If there are two or more lambda stages in a pipleine the system cannot find by name
            RegisterStage(AskName);
            RegisterStage(AcceptNameAndSave);
        }
        public ContentResult AskName()
        {
            return Text("Enter new ToDo category name:");
        }
        public ContentResult AcceptNameAndSave()
        {
            _todoService.CreateCategory(MessageContext.User.Id, MessageContext.Message.Text);
            return Text("✅ Done");
        }

    }
}
