using Autofac;
using Domain.TelegramBot.MessagePipelines.TimeTracker;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.ComponentModel;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.MessagePipelines.TimeTracker
{
    [Route("/time_tracker", "⏱ Time tracker")]
    [Description("Use this command for creating notes")]
    public class TrackerInfoPipeline : MessagePipelineBase
    {
        public TrackerInfoPipeline(ILifetimeScope scope) : base(scope)
        {
        }

        public override void RegisterPipelineStages()
        {
            RegisterStage(ReturnInfo);
            IsLooped = true;
        }

        public ContentResult ReturnInfo(MessageContext ctx)
        {
            return new ContentResult()
            {
                Text = @"Welcome to time tracker. You can user few commands to efectively manage your time. Here they are:
/track_in for start tracking time for specific activity
/track_out for stop tracking for last selected activity
/activities for managing activities
Or, use the menu below:",
                Menu = new(new KeyboardButton[][]
                {
                    MenuButtonRow(MenuButton(GetRoute<TrackInPipeline>().AlternativeRoute),MenuButton(GetRoute<TrackOutPipeline>().AlternativeRoute)),
                    MenuButtonRow(MenuButton(GetRoute<ActivityManagementPipeline>().AlternativeRoute)),
                    MenuButtonRow(MenuButton(GetRoute<GenerateReportPipeline>().AlternativeRoute))
                })
                {
                    ResizeKeyboard = true
                }
            };
        }
    }
}
