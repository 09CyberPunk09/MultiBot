using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services.Questionaires;
using Application.Services.Questionaires.Dto;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Commands.Pipelines.Questionaires.Dto;
using Application.TelegramBot.Commands.Pipelines.SChedulingV2.Helpers;
using Common.Entites.Scheduling;
using System.Linq;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires.Create;

[Route("/save_questionaire", "☑️ Save questionaire")]
public class SaveQuestionaireCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<ApplySelectedSchedulingMode>();
        builder.Stage<AcceptScheduleExpressionAndSaveQuestionaire>();
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

public class ApplySelectedSchedulingMode : ITelegramStage
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

public class AcceptScheduleExpressionAndSaveQuestionaire : ITelegramStage
{
    private readonly QuestionaireService _service;
    public AcceptScheduleExpressionAndSaveQuestionaire(QuestionaireService service)
    {
        _service = service;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var schedulerExpression = ctx.Cache.Get<ScheduleExpressionDto>(ScheduleExpressionDto.CACHEKEY, true);
        var questionaire = ctx.Cache.Get<NewQuestionaireDto>(true);
        _service.Create(new()
        {
            Text = questionaire.Name,
            UserId = ctx.User.Id,
            SchedulerExpression = schedulerExpression,
        },
        questionaire.Questions.Select(x => new CreateQuestionDto()
        {
            AnswerType = x.AnswerType,
            Text = x.Text,
            RangeMax = x.NumericRange.Item2,
            RangeMin = x.NumericRange.Item1,
            PredefinedAnswers = x.PredefinedAnswers.Select(y => new CreatePredefinedAnswerDto()
            {
                Text = y.Text,
            }).ToList()

        }).ToList());
        return ContentResponse.Text("✅ Done");
    }
}
