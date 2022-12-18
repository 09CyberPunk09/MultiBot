using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Chunks;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.ComponentModel;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling
{
    internal enum MenuActiotype
    {
        Selection = 700,
        Undo,
        Confirm,
        Next,
        Previous
    }

    record DayPayload(string Text, DayOfWeek? DayOfWeek, bool ContinueChoosing = true, MenuActiotype ActionType = MenuActiotype.Selection);

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