using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services.Questionaires;
using Application.Services.Reminders;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Commands.Pipelines.Reminders;
using Common.Entites.Questionaires;
using System;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires.Options;

[Route("/delete_questionaire", "Delete Questionaire")]
public class DeleteQuestionaireCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptDeleteQuestionaireDesicion>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.New(new()
        {
            Text = "Do you want to delete the questionaire?",
            Menu = new Menu(Menu.MenuType.MessageMenu, new[]
            {
                new[] { new Button("Yes", true.ToString() ), new Button("No", false.ToString() )}
            })
        });
    }
}

public class AcceptDeleteQuestionaireDesicion : ITelegramStage
{
    private readonly QuestionaireService _service;
    public AcceptDeleteQuestionaireDesicion(QuestionaireService service)
    {
        _service = service;
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

        var reminderId = ctx.Cache.Get<Guid>(SelectQuestionaireCommand.SELECTEDQUESTIONAIREID, true);
        _service.Delete(reminderId);

        return ContentResponse.Text("Done");
    }
}
