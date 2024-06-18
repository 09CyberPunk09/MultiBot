using Application.Services.Questionaires;
using Application.Services.Questions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Commands.Routing;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires.Options;

[Route("/question_options", "Question options")]
public class SelectQuestionCommand : ITelegramCommand
{
    public const string QUESTIONID_CACHEKEY = "SelectedQuestionId";
    public const string QUESTIONDICT_CACHEKEY = "QuestionsDictionary";
    private readonly QuestionaireService _service;

    public SelectQuestionCommand(QuestionaireService service)
    {
        _service = service;
    }
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptQuestioNnumberAndReturnMenu>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var quesiotnaireId = ctx.Cache.Get<Guid>(SelectQuestionaireCommand.SELECTEDQUESTIONAIREID);
        var questionaire = _service.Get(quesiotnaireId);
        StringBuilder sb = new();
        sb.AppendLine("Select a number near the question you need:");
        sb.AppendLine();
        Dictionary<int,Guid> questionsDict = new();
        int index = 1;
        questionaire.Questions.ForEach(x =>
        {
            sb.AppendLine($"🔹{index}. {x.Text}");
            questionsDict[index] = x.Id;
            index++;
        });

        return ContentResponse.Text(sb.ToString());
    }
}

public class AcceptQuestioNnumberAndReturnMenu : ITelegramStage
{
    private readonly QuestionService _service;
    private readonly RoutingTable _routingTable;
    public AcceptQuestioNnumberAndReturnMenu(QuestionService service, RoutingTable routingTable)
    {
        _service = service;
        _routingTable = routingTable;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var questionsDict = ctx.Cache.Get<Dictionary<int, Guid>>(SelectQuestionCommand.QUESTIONDICT_CACHEKEY);
        if (!int.TryParse(ctx.Message.Text, out int number) || !(questionsDict.Count + 1 >= number && number > 0))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("Please enter a valid number");
        }
        var questionId = questionsDict[number];
        var question = _service.Get(questionId);
        ctx.Cache.Set(SelectQuestionCommand.QUESTIONID_CACHEKEY, questionId);

        ctx.Cache.Remove(SelectQuestionCommand.QUESTIONDICT_CACHEKEY);

        StringBuilder sb = new();

        sb.AppendLine($"🔶 {question.Text}");
        question.PredefinedAnswers.ForEach(pa =>
        {
            sb.AppendLine($"     ▪️ {pa.Text}");
        });
        sb.AppendLine();
        sb.AppendLine("Select an option for this quesdtion:");

        return ContentResponse.New(new()
        {
            Text = sb.ToString(),
            Menu = new(Menu.MenuType.MessageMenu, new[]
            {
                new[]{ new Button(_routingTable.AlternativeRoute<EditQuestionText>(), _routingTable.Route<EditQuestionText>()) },
                new[]{ new Button(_routingTable.AlternativeRoute<DeleteQuestion>(), _routingTable.Route<DeleteQuestion>()) },
            })
        });

    }
}
