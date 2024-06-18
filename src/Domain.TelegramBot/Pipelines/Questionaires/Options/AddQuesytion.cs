using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires.Options
{
    [Route("/edit_questionaire_add_question","Add a new Question")]
    //TODO IMPLEMENT
    public class AddQuestion : ITelegramCommand
    {
        public void DefineStages(StageMapBuilder builder)
        {
        }

        public Task<StageResult> Execute(TelegramMessageContext ctx)
        {
            throw new System.NotImplementedException();
        }
    }
}
