using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Commands.Pipelines.Questionaires.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Questionaires;


[Route("/questionaires", "Questionaires")]
public class QuestionairesMenu : ITelegramCommand
{
    private readonly RoutingTable _routingTable;
    public QuestionairesMenu(RoutingTable routingTable)
    {
        _routingTable = routingTable;
    }

    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.New(new()
        {
            Text = "Questionaires Menu",
            Menu = new Menu(Menu.MenuType.MenuKeyboard, new[]
            {
                new[]
                {
                    new Button(_routingTable.AlternativeRoute<CreateQuestionaireCommand>()),
                    new Button(_routingTable.AlternativeRoute<SelectQuestionaireCommand>()),
                }
            }
            )
        });
    }
}
