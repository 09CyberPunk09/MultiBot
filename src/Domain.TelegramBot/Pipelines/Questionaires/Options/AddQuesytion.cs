using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using System.Threading.Tasks;

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
