using Application.Services.Questions;
using System;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires.Options;

[Route("/edit_question_text", "Edit Question Text")]
public class EditQuestionText : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptNewQuestionNameStage>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Enter new question text:");
    }
}

public class AcceptNewQuestionNameStage : ITelegramStage
{
    private readonly QuestionService _service;
    public AcceptNewQuestionNameStage(QuestionService service)
    {
        _service = service;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var questionId = ctx.Cache.Get<Guid>(SelectQuestionCommand.QUESTIONID_CACHEKEY);
        var question = _service.Get(questionId);
        question.Text = ctx.Message.Text;
        _service.Update(question);

        return ContentResponse.Text("Done");
    }
}
