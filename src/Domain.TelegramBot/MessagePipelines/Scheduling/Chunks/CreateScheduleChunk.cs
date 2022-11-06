using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using CallbackButton = Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton;

namespace Infrastructure.TelegramBot.MessagePipelines.Scheduling.Chunks
{
    public class CreateScheduleChunk : PipelineChunk
    {
        private const string DAYS_CACHEKEY = "SelectedDays";
        private const string HOUR_CACHEKEY = "SelectedHour";
        public const string CRONEXPR_CACHEKEY = "CronExpr";

        public CreateScheduleChunk(ILifetimeScope scope) : base(scope)
        { }

        public override void RegisterPipelineStages()
        {
            RegisterStage(nameof(ScheduleDay));
            RegisterStage(nameof(ConfirmSelectedDays));
            RegisterStage(nameof(AskMinutes));
            RegisterStage(nameof(AcceptTime));
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

                        StringBuilder selectedDaysText = new("Let's schedule your message.First, choose a day or days when the bot must     sendyoumessages:");

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

        private ContentResult AskTime()
        {
            var times = Enumerable.Range(0, 23);
            var chunked = times.Chunk(4);
            var buttons = chunked.Select(
             x => x.Select(y => Button($"🕧{y}:00", y.ToString()))
                     .ToList())
             .ToList();

            buttons.Add(new List<CallbackButton>() { Button("Confirm", MenuActiotype.Confirm.ToString()) });
            buttons.Add(new List<CallbackButton>() { Button("Undo", MenuActiotype.Undo.ToString()) });

            return new()
            {
                Edited = true,
                Text = "Select time of day when the bot should send you: ",
                Buttons = new InlineKeyboardMarkup(buttons)

            };
        }

        public ContentResult AskMinutes()
        {
            if (!int.TryParse(MessageContext.Message.Text, out int selectedHours))
            {
                Response.ForbidNextStageInvokation();
                return Text("Please,choose a correct time from the menu");
            }

            cache.SetValueForChat(HOUR_CACHEKEY, MessageContext.Message.Text, MessageContext.RecipientChatId);

            List<int> mins = new();
            for (int i = 0; i < 60; i += 5)
                mins.Add(i);

            var chunked = mins.Chunk(4);

            var buttons = chunked.Select(
                 x => x.Select(y => Button($"🕧{y}", y.ToString()))
                  .ToList())
            .ToList();

            return new()
            {
                Edited = true,
                Text = "Choose time in minutes or type custom:",
                Buttons = new InlineKeyboardMarkup(buttons)
            };
        }

        public ContentResult AcceptTime()
        {
            int mins;
            if (!int.TryParse(MessageContext.Message.Text, out mins))
            {
                Response.ForbidNextStageInvokation();
                return Text("Incorrect minutes selected. Try again.");
            }

            int hours = GetCachedValue<int>(HOUR_CACHEKEY, MessageContext.RecipientChatId);

            var days = GetCachedValue<List<DayPayload>>(DAYS_CACHEKEY, MessageContext.RecipientChatId);

            var stringDays = days.Select(x => Enum.GetName(x.DayOfWeek.Value)).ToList();

            var daysShortNames = stringDays.Select(d => d.ToUpper().Substring(0, 3));
            //Seconds 	Minutes 	Hours 	Day Of Month 	Month 	Day Of Week 	Year
            //0 	    24      	12 	        ? 	            * 	SUN,MON,TUE 	*
            string cronExpression = $"0 {mins} {hours} ? * {string.Join(",", daysShortNames)} *";

            cache.SetValueForChat(CRONEXPR_CACHEKEY, cronExpression, MessageContext.RecipientChatId);

            return Text($"You scheduled the message on every {string.Join(", ", stringDays)} at {hours}:{mins}. Cron expression: { cronExpression }", true);
        }

        private List<List<CallbackButton>> BuildDaysMenu(IEnumerable<DayPayload> days)
        {
            var chunked = days.Chunk(2);

            return chunked.Select(
                x => x.Select(y => Button(y.Text, y.Text))
                        .ToList())
                .ToList();
        }

        private static DayPayload[] daysMenu = new[]
        {
                new DayPayload("🗓Monday",DayOfWeek.Monday),
                new DayPayload("🗓Tuesday",DayOfWeek.Tuesday),
                new DayPayload("🗓Wednesday",DayOfWeek.Wednesday),
                new DayPayload("🗓Thursday",DayOfWeek.Thursday),
                new DayPayload("🗓Friday",DayOfWeek.Friday),
                new DayPayload("🗓Saturday",DayOfWeek.Saturday),
                new DayPayload("🗓Sunday",DayOfWeek.Sunday),
                new DayPayload("Continue",null,false,MenuActiotype.Confirm),
                new DayPayload("Undo",null,true,MenuActiotype.Undo),
        };
    }
}