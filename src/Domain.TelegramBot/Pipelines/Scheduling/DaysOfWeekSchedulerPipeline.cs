using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Collections.Generic;
using System;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq;
using System.Text;
using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Dto;
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
namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Reimplementation.ScheduleBuilderByType
{
    [Route("/days_of_week_scheduler")]
    public class DaysOfWeekSchedulerPipeline : MessagePipelineBase
    {
        private const string DAYS_CACHEKEY = "SelectedDays";

        public DaysOfWeekSchedulerPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStageMethod(ScheduleDay);
            RegisterStageMethod(ConfirmSelectedDays);
            RegisterStageMethod(TryAcceptTime);
        }

        public ContentResult ScheduleDay()
        {
            cache.SetValueForChat(DAYS_CACHEKEY, new List<DayOfWeek>(), MessageContext.RecipientChatId);

            return new()
            {

                Text = "First, choose a day or days when the bot must send you message:",
                Buttons = new InlineKeyboardMarkup(BuildDaysMenu(daysMenu.ToList()))
            };
        }

        public ContentResult ConfirmSelectedDays()
        {
            DayPayload payload;
            try
            {
                //  var day = JsonConvert.DeserializeObject<DayOfWeek>(MessageContext.Message.Text);
                payload = daysMenu.FirstOrDefault(x => x.Text == MessageContext.Message.Text);
            }
            catch (Exception)
            {
                Response.ForbidNextStageInvokation();
                return Text("Please, select a value from the menu");
            }
            switch (payload.ActionType)
            {
                case MenuActiotype.Selection:
                    {
                        var cachedDays = cache.GetValueForChat<List<DayPayload>>(DAYS_CACHEKEY, MessageContext.RecipientChatId);
                        cachedDays.Add(payload);
                        cache.SetValueForChat(DAYS_CACHEKEY, cachedDays, MessageContext.RecipientChatId);

                        var avaliableMenuItems = daysMenu.Where(x => !cachedDays.Any(y => y.DayOfWeek == x.DayOfWeek));

                        var buttons = BuildDaysMenu(avaliableMenuItems);

                        StringBuilder selectedDaysText = new("Let's schedule your message.First, choose a day or days when the bot must send you messages:");

                        selectedDaysText.AppendLine(" Selected days:");
                        daysMenu.Where(x => !avaliableMenuItems.Any(y => y.DayOfWeek == x.DayOfWeek))
                            .ToList()
                            .ForEach(x => selectedDaysText.AppendLine(x.Text));

                        Response.ForbidNextStageInvokation();

                        return new()
                        {
                            Edited = true,
                            Text = selectedDaysText.ToString(),
                            Buttons = new InlineKeyboardMarkup(buttons)
                        };
                    }
                case MenuActiotype.Undo:
                    {
                        var cachedDays = cache.GetValueForChat<List<DayPayload>>(DAYS_CACHEKEY, MessageContext.RecipientChatId);
                        if (cachedDays.Count != 0)
                        {
                            cachedDays.Remove(cachedDays[^1]);
                        }
                        cache.SetValueForChat(DAYS_CACHEKEY, cachedDays, MessageContext.RecipientChatId);
                        var avaliableMenuItems = daysMenu.Where(x => !cachedDays.Any(y => y.DayOfWeek == x.DayOfWeek));

                        var buttons = BuildDaysMenu(avaliableMenuItems);

                        StringBuilder selectedDaysText = new("Let's schedule your message.First, choose a day or days when the bot must send youmessages:");

                        selectedDaysText.AppendLine(" Selected days:");
                        selectedDaysText.AppendLine();
                        daysMenu.Where(x => !avaliableMenuItems.Any(y => y.DayOfWeek == x.DayOfWeek))
                            .ToList()
                            .ForEach(x => selectedDaysText.AppendLine(x.Text));

                        Response.ForbidNextStageInvokation();

                        return new()
                        {
                            Edited = true,
                            Text = selectedDaysText.ToString(),
                            Buttons = new InlineKeyboardMarkup(buttons)
                        };
                    }
                case MenuActiotype.Confirm:
                    return AskTime();

                default:
                    return Text("Unhandled");
            }
        }

        public ContentResult AskTime()
        {
            return Text("Enter time in format HH:MM, HH:MM,...");
        }

        public ContentResult TryAcceptTime()
        {
            var text = MessageContext.Message.Text;
            try
            {
                var result = TimeParser.Parse(text);
                //0 0,35 7 ? * SUN,TUE,THU *
                var crons = new List<string>();
                var cachedDays = cache.GetValueForChat<List<DayPayload>>(DAYS_CACHEKEY, MessageContext.RecipientChatId);

                foreach (var day in cachedDays)
                {
                    foreach (var tuple in result)
                    {
                        var dayCronExpr = Enum.GetName(day.DayOfWeek.Value).Substring(0, 3).ToUpper();
                        var cron = $"0 0,{tuple.Item2} {tuple.Item1} ? * {dayCronExpr} *";
                        crons.Add(cron);
                    }
                }
                var schedulerConfig = new ScheduleExpressionDto(crons);

                SetCachedValue(ScheduleExpressionDto.CACHEKEY, schedulerConfig);

                return Text("Schedule configured");
            }
            catch (ArgumentException)
            {
                Response.ForbidNextStageInvokation();
                return Text("You entered a not valid value. Try entering a string in format HH:MM, HH:MM,...");
            }

        }


        private List<List<InlineKeyboardButton>> BuildDaysMenu(IEnumerable<DayPayload> days)
        {
            var chunked = days.Chunk(2);

            return chunked.Select(
                x => x.Select(y => Button(y.Text, y.Text))
                        .ToList())
                .ToList();
        }

        private static readonly DayPayload[] daysMenu = new[]
        {
                new DayPayload("🗓 Monday",DayOfWeek.Monday),
                new DayPayload("🗓 Tuesday",DayOfWeek.Tuesday),
                new DayPayload("🗓 Wednesday",DayOfWeek.Wednesday),
                new DayPayload("🗓 Thursday",DayOfWeek.Thursday),
                new DayPayload("🗓 Friday",DayOfWeek.Friday),
                new DayPayload("🗓 Saturday",DayOfWeek.Saturday),
                new DayPayload("🗓 Sunday",DayOfWeek.Sunday),
                new DayPayload("Continue",null,false,MenuActiotype.Confirm),
                new DayPayload("Undo",null,true,MenuActiotype.Undo),
        };
    }
}
