using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Commands.Pipelines.Questionaires.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires;

[Route("/remove_last_question_from_questionaire", "🗑 Remove last question")]
public class RemoveLastQuestionFromQuestionaireCommand : ITelegramCommand
{
    private readonly ShowQuestionaireStage _showQuestionaireStage;
    public RemoveLastQuestionFromQuestionaireCommand(ShowQuestionaireStage showQuestionaireStage)
    {
        _showQuestionaireStage = showQuestionaireStage;
    }
    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var questionaire = ctx.Cache.Get<NewQuestionaireDto>();
        var last = questionaire.Questions.LastOrDefault();
        questionaire.Questions.Remove(last);
        ctx.Cache.Set(questionaire);
        return _showQuestionaireStage.Execute(ctx);
    }
}
