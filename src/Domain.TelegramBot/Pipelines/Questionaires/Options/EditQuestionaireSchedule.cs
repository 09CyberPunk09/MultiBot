using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services.Questionaires;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Commands.Pipelines.SChedulingV2.Helpers;
using Common.Entites.Scheduling;
using System;
using System.Linq;
using System.Threading.Tasks;
using Common;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires.Options;


[Route("/edit_questionaire_schedule", "Edit Questionaire Schedule")]
public class EditQuestionaireSchedule : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<ApplySelectedSchedulingModeaNDeDIT>();
        builder.Stage<AccepteDITScheduleExpressionAndSaveQuestionaire>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var menuItems = ScheduleTypeListingHelper
            .Modes
            .Select(x => new[] { new Button(x.Item1.Name, x.Item1.Name) })
            .ToArray();
        var menu = new Menu(Menu.MenuType.MessageMenu, menuItems);
        return ContentResponse.New(
            new()
            {
                Text = "Now,select a schedule for your questionaire",
                Menu = menu
            });
    }
}

public class ApplySelectedSchedulingModeaNDeDIT : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var isValidSelection = ScheduleTypeListingHelper.Modes.Any(x => x.Item1.Name == ctx.Message.Text);
        if (!isValidSelection)
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("Select an option from the menu");
        }
        var type = ScheduleTypeListingHelper.Modes.First(x => x.Item1.Name == ctx.Message.Text).Item2;
        ctx.Response.InvokeNextImmediately = true;
        return Task.FromResult(new StageResult()
        {
            NextStage = type.FullName
        });
    }
}

public class AccepteDITScheduleExpressionAndSaveQuestionaire : ITelegramStage
{
    private readonly QuestionaireService _service;
    public AccepteDITScheduleExpressionAndSaveQuestionaire(QuestionaireService service)
    {
        _service = service;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var schedulerExpression = ctx.Cache.Get<ScheduleExpressionDto>(ScheduleExpressionDto.CACHEKEY, true);
        var questionaireId = ctx.Cache.Get<Guid>(SelectQuestionaireCommand.SELECTEDQUESTIONAIREID);
        var quesitonaire = _service.Get(questionaireId);
        quesitonaire.SchedulerExpression = schedulerExpression.ToJson();
        _service.Update(quesitonaire);

        return ContentResponse.Text("✅ Done");
    }
}
