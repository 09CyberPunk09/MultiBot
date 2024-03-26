using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services.Questions;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires.Options;

[Route("/delete_question", "Delete Question")]
public class DeleteQuestion : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptDeleteQuestionDesicion>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.New(new()
        {
            Text = "Do you want to delete the question?",
            Menu = new Menu(Menu.MenuType.MessageMenu, new[]
            {
                new[] { new Button("Yes", true.ToString() ), new Button("No", false.ToString() )}
            })
        });
    }
}

public class AcceptDeleteQuestionDesicion : ITelegramStage
{
    private readonly QuestionService _service;
    public AcceptDeleteQuestionDesicion(QuestionService reminderService)
    {
        _service = reminderService;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        if (!bool.TryParse(ctx.Message.Text, out var result))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("Select a menu item");
        }

        if (!result)
        {
            return ContentResponse.Text("okay");
        }

        var questionId = ctx.Cache.Get<Guid>(SelectQuestionCommand.QUESTIONID_CACHEKEY, true);
        _service.DeleteQuestion(questionId);

        return ContentResponse.Text("Done");
    }
}
