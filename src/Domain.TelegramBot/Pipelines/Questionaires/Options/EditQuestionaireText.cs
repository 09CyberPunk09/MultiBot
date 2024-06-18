using Application.Services.Questionaires;
using System;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires.Options;

[Route("/edit_questionaire_text", "Edit Questionaire Text")]
public class EditQuestionaireText : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptNewQuestionaireName>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Enter a new name for the quesitonaire");
    }
}

public class AcceptNewQuestionaireName : ITelegramStage
{
    private readonly QuestionaireService _service;
    public AcceptNewQuestionaireName(QuestionaireService service)
    {
        _service = service;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var text = ctx.Message.Text;
        var questionaireId = ctx.Cache.Get<Guid>(SelectQuestionaireCommand.SELECTEDQUESTIONAIREID);
        var questionaire = _service.Get(questionaireId);
        questionaire.Name= text;
        _service.Update(questionaire);

        return ContentResponse.Text("Done");
    }
}
