using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Enums;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Reimplementation
{
    public class CreateSchedulePipelineChunk : PipelineChunk
    {
        public CreateSchedulePipelineChunk(ILifetimeScope scope) : base(scope)
        {
            RegisterStageMethod(AskMode);
        }

        public ContentResult AskMode()
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
            var buttonMenu = avaliableVariants.Select(m => new List<InlineKeyboardButton>()
            {
                Button(m.Name, m.Value.ToString())
            });

            return new()
            {
                Text = "Select the pattern how would you want to create a schedule:",
                Buttons = new(buttonMenu)
            };
        }
    }
}
