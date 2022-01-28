using Autofac;
using Infrastructure.TextUI.Core.Types;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using Button = Telegram.Bot.Types.ReplyMarkups.InlineKeyboardButton;


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
        private const string DAYS_CACHEKEY = "SelectedDays";
        private const string HOUR_CACHEKEY = "SelectedHour";
        public ScheduleAMessageTestPipeline(ILifetimeScope scope) : base(scope)
        {
        }

        public override void RegisterPipelineStages()
        {
            RegisterStage(ScheduleDay);
            RegisterStage(ConfirmSelectedDays);
            RegisterStage(AskMinutes);
            RegisterStage(AcceptTime);
        }

        public ContentResult ScheduleDay(MessageContext ctx)
        {
            cache.SetValueForChat(DAYS_CACHEKEY, new List<DayOfWeek>(), ctx.Recipient);
            return new BotMessage()
            {
                Text = "Let's schedule your message. First, choose a day or days when the bot must send you messages:",
                Buttons = new InlineKeyboardMarkup(BuildDaysMenu(daysMenu.ToList()))
            };
        }

        public ContentResult ConfirmSelectedDays(MessageContext ctx)
        {
            DayPayload payload;
            try
            {
              //  var day = JsonConvert.DeserializeObject<DayOfWeek>(ctx.Message.Text);
                payload = daysMenu.FirstOrDefault(x => x.Text == ctx.Message.Text);
            }
            catch (Exception)
            {
                ForbidMovingNext();
                return Text("Please, select a value from the menu");
            }

            switch (payload.ActionType)
            {
                case MenuActiotype.Selection:
                {
                        var cachedDays = cache.GetValueForChat<List<DayPayload>>(DAYS_CACHEKEY, ctx.Recipient);
                        cachedDays.Add(payload);
                        cache.SetValueForChat(DAYS_CACHEKEY, cachedDays, ctx.Recipient);

                        var avaliableMenuItems = daysMenu.Where(x => !cachedDays.Any(y => y.DayOfWeek == x.DayOfWeek));

                        var buttons = BuildDaysMenu(avaliableMenuItems);

                        StringBuilder selectedDaysText = new("Let's schedule your message.First, choose a day or days when the bot must send you messages:");

                        selectedDaysText.AppendLine(" Selected days:");
                        daysMenu.Where(x => !avaliableMenuItems.Any(y => y.DayOfWeek == x.DayOfWeek))
                            .ToList()
                            .ForEach(x => selectedDaysText.AppendLine(x.Text));

                        ForbidMovingNext();

                        return new EditLastMessage()
                        {
                            NewMessage = new BotMessage()
                            {
                                Text = selectedDaysText.ToString(),
                                Buttons = new InlineKeyboardMarkup(buttons)
                            }
                        };
                }
                case MenuActiotype.Undo:
                {
                        var cachedDays = cache.GetValueForChat<List<DayPayload>>(DAYS_CACHEKEY, ctx.Recipient);
                        if(cachedDays.Count != 0)
                        {
                            cachedDays.Remove(cachedDays[^1]);
                        }
                        cache.SetValueForChat(DAYS_CACHEKEY, cachedDays, ctx.Recipient);
                        var avaliableMenuItems = daysMenu.Where(x => !cachedDays.Any(y => y.DayOfWeek == x.DayOfWeek));

                        var buttons = BuildDaysMenu(avaliableMenuItems);

                        StringBuilder selectedDaysText = new("Let's schedule your message.First, choose a day or days when the bot must send you messages:");

                        selectedDaysText.AppendLine(" Selected days:");
                        daysMenu.Where(x => !avaliableMenuItems.Any(y => y.DayOfWeek == x.DayOfWeek))
                            .ToList()
                            .ForEach(x => selectedDaysText.AppendLine(x.Text));

                        ForbidMovingNext();

                        return new EditLastMessage()
                        {
                            NewMessage = new BotMessage()
                            {
                                Text = selectedDaysText.ToString(),
                                Buttons = new InlineKeyboardMarkup(buttons)
                            }
                        };
                    }
                case MenuActiotype.Confirm:
                    return AskTime(ctx);
                default:
                    return Text("Unhandled");
            }
        }

        private ContentResult AskTime(MessageContext ctx)
        {
            var times = Enumerable.Range(0, 23);
            var chunked = times
             .Select((x, i) => new { Index = i, Value = x })
             .GroupBy(x => x.Index / 4)
             .Select(x => x.Select(v => v.Value).ToList())
             .ToList();

            var buttons =  chunked.Select(
             x => x.Select(y => Clickable($"🕧{y}:00", y.ToString()))
                     .ToList())
             .ToList();

            buttons.Add(new List<Button>() { Button.WithCallbackData("Confirm", (MenuActiotype.Confirm).ToString()) });
            buttons.Add(new List<Button>() { Button.WithCallbackData("Undo", (MenuActiotype.Undo).ToString()) });

            return new BotMessage()
            {
                Text = "Select time of day when the bot should send you: ",
                Buttons = new InlineKeyboardMarkup(buttons)
            };
        }

        public ContentResult AskMinutes(MessageContext ctx)
        {
            if (!int.TryParse(ctx.Message.Text, out int selectedHours))
            {
                ForbidMovingNext();
                return Text("Please,choose a correct time from the menu");
            }

            cache.SetValueForChat(HOUR_CACHEKEY, ctx.Message.Text, ctx.Recipient);

            List<int> mins = new();
            for (int i = 0; i <= 60; i+=5)
                mins.Add(i);

            var chunked = mins
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / 4)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();

            var buttons = chunked.Select(
                 x => x.Select(y => Clickable($"🕧{y}", y.ToString()))
                  .ToList())
            .ToList();

            return new BotMessage()
            {
                Text = "Choose time in minutes or type custom:",
                Buttons = new InlineKeyboardMarkup(buttons)
            };
        }

        public ContentResult AcceptTime(MessageContext ctx)
        {
            int mins;
            if(!int.TryParse(ctx.Message.Text,out mins))
            {
                ForbidMovingNext();
                return Text("Incorrect minutes selected. Try again.");
            }

            int hours = cache.GetValueForChat<int>(HOUR_CACHEKEY, ctx.Recipient);

            var days = cache.GetValueForChat<List<DayPayload>>(DAYS_CACHEKEY, ctx.Recipient);

            var stringDays = days.Select(x => Enum.GetName<DayOfWeek>(x.DayOfWeek.Value)).ToList();

            return Text($"You scheduled the message on every {string.Join(", ", stringDays)} at {hours}:{mins}");
        }


        private static Button Clickable(string text, string callbackData)
            => Button.WithCallbackData(text, callbackData);

        private static List<List<Button>> BuildDaysMenu(IEnumerable<DayPayload> days)
        {
            var chunked = days
            .Select((x, i) => new { Index = i, Value = x })
            .GroupBy(x => x.Index / 2)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();

            return chunked.Select(
                x => x.Select(y => Clickable(y.Text, y.Text))
                        .ToList())
                .ToList();
        }

        private static DayPayload[] daysMenu = new []
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
