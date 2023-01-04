using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Application.Chatting.Core.Repsonses.Menu;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.SChedulingV2.Pipelines.Test;

[Route("/test_schedule")]
public class CreateTestScheduleCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var avaliableVariants = new List<SchedulingMode>()
            {
               SchedulingMode.EveryDayAT,
               SchedulingMode.EceryDAYSAT,
               SchedulingMode.EveryWEEKENDAT,
               SchedulingMode.OnDATEAT,
               SchedulingMode.EveryNTHDAYStartingFromTodayAT,
               SchedulingMode.EveryMONTHonDAYAT,
               SchedulingMode.CRON
            };
        var buttonMenu = avaliableVariants.Select(m => new List<Button>()
            {
                 new Button(text : m.Name, callbackData: m.Value.ToString())
            });

        return StageResult.TaskContentResult(new()
        {
            Text = "Select The type of a schedule:",
            Menu = new(MenuType.MessageMenu, buttonMenu)
        });
    }
}