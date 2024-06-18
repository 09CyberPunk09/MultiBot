using Application.Services.Questionaires;
using Common;
using Common.Entites.Scheduling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Commands.Routing;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires.Options;

[Route("/questionaire_iotion","Questionaire Options")]
public class SelectQuestionaireCommand : ITelegramCommand
{
    public const string SELECTEDQUESTIONAIREID = "SelectedQuestionaireId";
    public const string USERQUESTIONAIRE = "UserQuestionaire";
    private readonly QuestionaireService _service;
    public SelectQuestionaireCommand(QuestionaireService service)
    {
        _service = service;
    }
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptQuesitonaireIdStage>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var userQuestionaires = _service.GetAll(ctx.User.Id);
        Dictionary<int, Guid> questionaireDict = new();
        int index = 1;
        StringBuilder sb = new();
        sb.AppendLine("Enter a number near the item you want to select");
        foreach (var quesitonaire in userQuestionaires)
        {
            questionaireDict[index] = quesitonaire.Id;
            sb.AppendLine();
            sb.AppendLine($"🔸 {index}. {quesitonaire.Name}");
            index++;
        }

        ctx.Cache.Set(USERQUESTIONAIRE, questionaireDict);

        return ContentResponse.Text(sb.ToString());
    }
}

public class AcceptQuesitonaireIdStage : ITelegramStage
{
    private readonly QuestionaireService _service;
    private readonly RoutingTable _routingTable;
    public AcceptQuesitonaireIdStage(QuestionaireService service, RoutingTable routingTable)
    {
        _service = service;
        _routingTable = routingTable;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var questionaireDict = ctx.Cache.Get<Dictionary<int, Guid>>(SelectQuestionaireCommand.USERQUESTIONAIRE);
        if (!int.TryParse(ctx.Message.Text, out int number) || !(questionaireDict.Count + 1 >= number && number > 0))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("Please enter a valid number");
        }
        var questionaireId = questionaireDict[number];
        var questionaire = _service.Get(questionaireId);
        ctx.Cache.Set(SelectQuestionaireCommand.SELECTEDQUESTIONAIREID, questionaireId);

        ctx.Cache.Remove(SelectQuestionaireCommand.USERQUESTIONAIRE);

        StringBuilder sb = new();

        sb.AppendLine($"🔶 {questionaire.Name}");
        var schedulerExpr = questionaire.SchedulerExpression.FromJson<ScheduleExpressionDto>();
        sb.AppendLine($"Fires: {schedulerExpr.Description}");
        string activeOrNot = questionaire.IsActive ? "Enabled" : "Disabled";
        sb.AppendLine($"🟢🔴 {activeOrNot}");
        sb.AppendLine();
        sb.AppendLine("Select an option for this questionaire:");

        return ContentResponse.New(new()
        {
            Text = sb.ToString(),
            Menu = new(Menu.MenuType.MessageMenu, new[]
            {
                new[]{ new Button(_routingTable.AlternativeRoute<EditQuestionaireText>(), _routingTable.Route<EditQuestionaireText>()) },
                new[]{ new Button(_routingTable.AlternativeRoute<EditQuestionaireSchedule>(), _routingTable.Route<EditQuestionaireSchedule>()) },
                new[]{ new Button(_routingTable.AlternativeRoute<DeleteQuestionaireCommand>(), _routingTable.Route<DeleteQuestionaireCommand>()) },
                new[]{ new Button(_routingTable.AlternativeRoute<SelectQuestionCommand>(), _routingTable.Route<SelectQuestionCommand>()) },
            })
        });

    }
}
