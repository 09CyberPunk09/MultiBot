using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Reminders
{
    [Route("/reminders_menu", "Reminders")]
    internal class RemindersMenuCommand : ITelegramCommand
    {
        private readonly RoutingTable _routingTable;
        public RemindersMenuCommand(RoutingTable routingTable)
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
                Text = "Reminders menu",
                Menu = new Menu(Menu.MenuType.MenuKeyboard, 
                new[]
                {
                    new[]
                    {
                        new Button(_routingTable.AlternativeRoute<CreateReminderCommand>()),
                        new Button(_routingTable.AlternativeRoute<GetRemindersCommand>()),
                    },
                    new[]
                    {
                        new Button(_routingTable.AlternativeRoute<SelectOneReminderCommand>()),
                    }
                }
                )
            }); 
        }
    }
}
