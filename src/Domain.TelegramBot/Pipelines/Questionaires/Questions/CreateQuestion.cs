using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Commands.Pipelines.Questionaires.Dto;
using Common.Entites.Questionaires;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires.Questions;

[Route("/new_questionaire_question", "➕🚥 New question")]
public class CreateQuestionCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptQuesiotnNameAndAskForAnswers>();
        builder.Stage<AcceptAction>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Enter question:");
    }
}

public class AcceptQuesiotnNameAndAskForAnswers : ITelegramStage
{
    private readonly ShowQuestionStage _showQuestionStage;
    public AcceptQuesiotnNameAndAskForAnswers(ShowQuestionStage showQuestionStage)
    {
        _showQuestionStage = showQuestionStage;
    }

    public async Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var questionaireDto = ctx.Cache.Get<NewQuestionaireDto>();
        questionaireDto.Questions.Add(new()
        {
            Text = ctx.Message.Text
        });
        ctx.Cache.Set(questionaireDto);

        var result = await _showQuestionStage.Execute(ctx);
        QuestionMenuHelper.BuildMenu(result.Content);
        return result;
    }
}

public enum ActionType
{
    RemoveLastAnswer = -314898,
    Confirm,
    AddPredefinedAnswer
}

public class AcceptAction : ITelegramStage
{
    private readonly ShowQuestionaireStage _showQuestionaireStage;
    private readonly ShowQuestionStage _showQuestionStage;
    public AcceptAction(ShowQuestionaireStage showQuestionaireStage, ShowQuestionStage showQuestionStage)
    {
        _showQuestionaireStage = showQuestionaireStage;
        _showQuestionStage = showQuestionStage;
    }

    public async Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        if (!Enum.TryParse(ctx.Message.Text, out ActionType action))
        {
            action = ActionType.AddPredefinedAnswer;
        }

        var questionaire = ctx.Cache.Get<NewQuestionaireDto>();
        var currentQuestion = questionaire.Questions.LastOrDefault();

        switch (action)
        {
            case ActionType.AddPredefinedAnswer:
                ctx.Response.ForbidNextStageInvokation();
                var text = ctx.Message.Text;

                if (Regex.IsMatch(text, "[0-9]+ ?- ?[0-9]+"))
                {
                    currentQuestion.AnswerType = AnswerType.Numeric;
                    int min = int.Parse(Regex.Match(text, "[0-9]+ ?-").Value.Replace("-", "").Trim());
                    int max = int.Parse(Regex.Match(text, "- ?[0-9]+").Value.Replace("-", "").Trim());
                    currentQuestion.NumericRange = (min, max);
                    ctx.Cache.Set(questionaire);
                    return await _showQuestionaireStage.Execute(ctx);
                }
                else
                {
                    currentQuestion.AnswerType = AnswerType.WithPredefinedAnswers;

                    currentQuestion.PredefinedAnswers.Add(new()
                    {
                        Text = text,
                    });
                    ctx.Cache.Set(questionaire);
                    var result1 = await _showQuestionStage.Execute(ctx);
                    QuestionMenuHelper.BuildMenu(result1.Content);
                    return result1;

                }
            case ActionType.RemoveLastAnswer:
                ctx.Response.ForbidNextStageInvokation();

                currentQuestion?.PredefinedAnswers.Remove(currentQuestion.PredefinedAnswers.LastOrDefault());
                ctx.Cache.Set(questionaire);

                var result = await _showQuestionaireStage.Execute(ctx);
                QuestionMenuHelper.BuildMenu(result.Content);
                return result;
            case ActionType.Confirm:
                return await _showQuestionaireStage.Execute(ctx);
            default:
                throw new NotImplementedException();
        }

    }
}


public class QuestionMenuHelper
{
    public static void BuildMenu(ContentResultV2 result)
    {
        var suggestions = new StringBuilder();
        suggestions.AppendLine();
        suggestions.AppendLine();
        suggestions.AppendLine("Suggestions:");
        suggestions.AppendLine("🔅 Just manually enter message to add them as answers.");
        suggestions.AppendLine("🔅 You can enter a range for determine a ranged answer(for example, 1-10,0-8,1-5,etc)");
        suggestions.AppendLine("🔅 Or select the Confirm button to complete the question creation");

        result.Text += suggestions.ToString();
        result.Menu = new(Menu.MenuType.MessageMenu, new[]
                {
                new[]{ new Button("✅ Confirm", ((int)ActionType.Confirm).ToString()) },
                new[]{ new Button("🗑 Remove last answer", ((int)ActionType.RemoveLastAnswer).ToString()) }
            });
    }
}