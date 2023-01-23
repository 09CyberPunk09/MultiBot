using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Commands.Pipelines.Questionaires.Create;
using Application.TelegramBot.Commands.Pipelines.Questionaires.Dto;
using Application.TelegramBot.Commands.Pipelines.Questionaires.Questions;
using Common.Entites.Questionaires;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires;

public class ShowQuestionaireStage : ITelegramStage
{
    private readonly RoutingTable _routingTable;
    public ShowQuestionaireStage(RoutingTable routingTable)
    {
        _routingTable = routingTable;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var quesitonaire = ctx.Cache.Get<NewQuestionaireDto>();
        var sb = new StringBuilder();
        sb.AppendLine($"🔶 {quesitonaire.Name}");
        quesitonaire.Questions.ForEach(x =>
        {
            sb.AppendLine($"🔷 {x.Text}");
            switch (x.AnswerType)
            {
                case AnswerType.WithPredefinedAnswers:
                    x.PredefinedAnswers.ForEach(y => sb.AppendLine($"    🔹 {y.Text}"));
                    break;
                case AnswerType.WithoutPredefinedAnswers:
                    break;
                case AnswerType.Numeric:
                    sb.AppendLine($"🔹 {x.NumericRange.Item1} - {x.NumericRange.Item2}");
                    break;
                default:
                    break;
            }
            sb.AppendLine();
        });

        var createQRoute = _routingTable.AlternativeRoute<CreateQuestionCommand>();
        var removeQRoute = _routingTable.AlternativeRoute<RemoveLastQuestionFromQuestionaireCommand>();
        var saveQRoute = _routingTable.AlternativeRoute<SaveQuestionaireCommand>();
        return ContentResponse.New(new()
        {
            Text = sb.ToString(),
            Menu = new(Menu.MenuType.MessageMenu, new[]
            {
                new[] { new Button(createQRoute, createQRoute) },
                new[] { new Button(removeQRoute, removeQRoute) },
                new[] { new Button(saveQRoute, saveQRoute) }
            })
        });
    }
}