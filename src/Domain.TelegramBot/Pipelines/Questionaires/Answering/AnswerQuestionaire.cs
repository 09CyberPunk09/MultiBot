using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services.Questionaires;
using Application.Services.Questions;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Common.Entites.Questionaires;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.TelegramBot.Commands.Pipelines.Questionaires.Answering.QuestionaireAnsweringModel;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires.Answering;

[Route("/answer_questionaire")]
public class AnswerQuestionaireCommand : ITelegramCommand
{
    private readonly QuestionaireService _questionaireService;
    private readonly QuestionService _questionService;
    public AnswerQuestionaireCommand(QuestionaireService questionaireService, QuestionService questionService)
    {
        _questionaireService = questionaireService;
        _questionService = questionService;
    }
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<QuestionCarouselStage>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var sessionInfo = new QuestionaireAnsweringModel();

        //load all data in the cache
        //TODO: Move all consts to a separate type
        var questionaireId = ctx.Cache.Get<Guid>(QuestionaireService.QUESTIONAIREID_CACHEKEY);
        sessionInfo.QuestionaireId = questionaireId;

        var questionaireSession = _questionaireService.CreateQuestionaireSession(questionaireId);
        sessionInfo.QuestionaireSessionId = questionaireSession.Id;

        var questionaire = _questionaireService.Get(questionaireId);
        var questions = questionaire.Questions.OrderBy(x => x.Position).Select(x => new QuestionPosition()
        {
            Position = x.Position,
            QuestionId = x.Id
        });
        sessionInfo.Questions = questions.ToList();
        var firstQuestion = questionaire.Questions.FirstOrDefault();
        if (firstQuestion != null)
        {
            if (firstQuestion.AnswerType != AnswerType.WithoutPredefinedAnswers)
            {
                sessionInfo.HasCurrentQuestionValidation = true;
            }
            sessionInfo.CurrentPosition = firstQuestion.Position;
            sessionInfo.CurrentQuestionId = firstQuestion.Id;

            var sb = new StringBuilder();
            sb.AppendLine($"🔶{firstQuestion.Text}");
            sb.AppendLine();
            switch (firstQuestion.AnswerType)
            {
                case AnswerType.WithPredefinedAnswers:
                    sb.AppendLine("🔑 Select one of predefined answers");
                    break;
                case AnswerType.WithoutPredefinedAnswers:
                    sb.AppendLine("🔑 You are free to type anything");
                    break;
                case AnswerType.Numeric:
                    sb.AppendLine($"🔑Enter a value in {firstQuestion.RangeMin} - {firstQuestion.RangeMax}");
                    break;
                default:
                    throw new NotImplementedException();
            }

            Menu menu = null;
            if (firstQuestion.AnswerType == AnswerType.WithPredefinedAnswers)
            {
                menu = new(Menu.MenuType.MessageMenu, firstQuestion.PredefinedAnswers.Select(x => new[] { new Button(x.Text, x.Text) }).ToArray());
            }

            ctx.Cache.Set(sessionInfo);
            return ContentResponse.New(new()
            {
                Text = sb.ToString(),
                Menu = menu
            });
        }
        else
        {
            ctx.Response.SetPipelineEnded();
            return ContentResponse.Text("The questionaire has no questions");
        }
    }
}
public class QuestionaireAnsweringModel
{
    public Guid QuestionaireId { get; set; }
    public Guid QuestionaireSessionId { get; set; }
    public Guid CurrentQuestionId { get; set; }
    public bool HasCurrentQuestionValidation { get; set; }
    public int CurrentPosition { get; set; }
    public List<QuestionPosition> Questions { get; set; }
    public class QuestionPosition
    {
        public int Position { get; set; }
        public Guid QuestionId { get; set; }
    }
}

public class QuestionCarouselStage : ITelegramStage
{
    private readonly QuestionaireService _questionaireService;
    private readonly QuestionService _questionService;
    public QuestionCarouselStage(QuestionaireService questionaireService, QuestionService questionService)
    {
        _questionaireService = questionaireService;
        _questionService = questionService;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var sessionInfo = ctx.Cache.Get<QuestionaireAnsweringModel>();
        var currentQuestion = _questionService.Get(sessionInfo.CurrentQuestionId);
        string text = ctx.Message.Text;
        //accepting answer to current q
        if (sessionInfo.HasCurrentQuestionValidation)
        {
            bool validationPassed = false;
            //if a q has validation-  we validate it
            switch (currentQuestion.AnswerType)
            {
                case AnswerType.WithPredefinedAnswers:
                    validationPassed = currentQuestion.PredefinedAnswers.Any(answer => answer.Text == text);
                    break;
                case AnswerType.WithoutPredefinedAnswers:
                    validationPassed = true;
                    break;
                case AnswerType.Numeric:
                    validationPassed = int.TryParse(text, out var n) && currentQuestion.RangeMax >= n && currentQuestion.RangeMin <= n;
                    break;
                default:
                    break;
            }

            if (!validationPassed)
            {
                ctx.Response.ForbidNextStageInvokation();
                return ContentResponse.Text("Answer the question following the rule near the 🔑");
            }
        }
        _questionService.AnswerQuestion(sessionInfo.QuestionaireSessionId, currentQuestion.Id, text);

        var nextQuestionModel = sessionInfo.Questions.FirstOrDefault(x => x.Position == sessionInfo.CurrentPosition + 1);
        if (nextQuestionModel == null)
        {
            ctx.Cache.Remove(sessionInfo);
            return ContentResponse.Text("✅ Questionaire Completed");
        }
        var nextQuestion = _questionService.Get(nextQuestionModel.QuestionId);
        if (nextQuestion.AnswerType != AnswerType.WithoutPredefinedAnswers)
        {
            sessionInfo.HasCurrentQuestionValidation = true;
        }
        sessionInfo.CurrentPosition = nextQuestion.Position;
        sessionInfo.CurrentQuestionId = nextQuestion.Id;

        var sb = new StringBuilder();
        sb.AppendLine($"🔶{nextQuestion.Text}");
        sb.AppendLine();
        switch (nextQuestion.AnswerType)
        {
            case AnswerType.WithPredefinedAnswers:
                sb.AppendLine("🔑 Select one of predefined answers");
                break;
            case AnswerType.WithoutPredefinedAnswers:
                sb.AppendLine("🔑 You are free to type anything");
                break;
            case AnswerType.Numeric:
                sb.AppendLine($"🔑Enter a value in {nextQuestion.RangeMin} - {nextQuestion.RangeMax}");
                break;
            default:
                throw new NotImplementedException();
        }

        Menu menu = null;
        if (nextQuestion.AnswerType == AnswerType.WithPredefinedAnswers)
        {
            menu = new(Menu.MenuType.MessageMenu, nextQuestion.PredefinedAnswers.Select(x => new[] { new Button(x.Text, x.Text) }).ToArray());
        }

        ctx.Cache.Set(sessionInfo);
        ctx.Response.ForbidNextStageInvokation();
        return ContentResponse.New(new()
        {
            Text = sb.ToString(),
            Menu = menu
        });
    }
}
