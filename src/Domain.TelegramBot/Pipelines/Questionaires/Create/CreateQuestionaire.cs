using Application.TelegramBot.Commands.Pipelines.Questionaires.Dto;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires;

[Route("/create_questionaire", "➕📋 Create Questionaire")]
public class CreateQuestionaireCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptQuestionaireNameStage>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        ctx.Cache.Set(new NewQuestionaireDto());
        return ContentResponse.Text("Enter Questionaire name:");
    }
}

public class AcceptQuestionaireNameStage : ITelegramStage
{
    private readonly ShowQuestionaireStage _showQuestionaireStage;
    public AcceptQuestionaireNameStage(ShowQuestionaireStage showQuestionaireStage)
    {
        _showQuestionaireStage = showQuestionaireStage;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var infoDto = ctx.Cache.Get<NewQuestionaireDto>();
        infoDto.Name = $"{ctx.Message.Text}";
        ctx.Cache.Set(infoDto);
        return _showQuestionaireStage.Execute(ctx);
    }
}
