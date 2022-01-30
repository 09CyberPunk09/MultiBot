using Autofac;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.TelegramBot.MessagePipelines.Scheduling.Chunks;
using System;
using System.ComponentModel;

namespace Infrastructure.UI.TelegramBot.MessagePipelines.Scheduling
{
    enum MenuActiotype
    {
        Selection = 700,
        Undo,
        Confirm,
        Next,
        Previous
    }
    record DayPayload(string Text,DayOfWeek? DayOfWeek, bool ContinueChoosing = true, MenuActiotype ActionType = MenuActiotype.Selection);

    [Route("/schedule-message-test")]
    [Description("Use this command for testing schedule functionality")]
    public class ScheduleAMessageTestPipeline : MessagePipelineBase
    {
        public ScheduleAMessageTestPipeline(ILifetimeScope scope) : base(scope)
        { 
        }

        public override void RegisterPipelineStages()
        {
            IntegrateChunkPipeline<CreateScheduleChunk>();
        }

      
    }
}
