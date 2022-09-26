using Application.Services;
using Autofac;
using Domain.TelegramBot.MessagePipelines.Scheduling.Chunks;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.TelegramBot.MessagePipelines.Reminder
{
    [Route("/fire_and_forget_reminder")]
    public class MakeReminderOneTimeFiringPipeline : MessagePipelineBase
    {
        private const string DATE = "SelectedFireDate";
        private const string HOUR = "SelectedFireHour";

        private readonly ReminderAppService _service;
        public MakeReminderOneTimeFiringPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<ReminderAppService>();
            RegisterStage(AskDay);
            RegisterStage(AceptDateAndAskTime);
            RegisterStage(AcceptTimeAndAskForMinutes);
            RegisterStage(SaveTime);
        }

        public ContentResult AskDay(MessageContext ctx)
        {
            var buttons = new List<List<InlineKeyboardButton>>()
            {
                ButtonRow("Today",  DateTime.Today.ToString()),
                ButtonRow("Tomorrow", DateTime.Today.AddDays(1).ToString())
            };

            var days = Enum.GetValues(typeof(DayOfWeek)).Cast<int>().Skip(2);
            var orderedDays = days.Where(d => d > ((int)DateTime.Today.DayOfWeek)).OrderBy(d => d).ToList();

            int counter = 0;
            foreach (var ot in orderedDays)
            {
                var temp = ButtonRow($"📅{Enum.GetName(typeof(DayOfWeek), DateTime.Today.AddDays(2 + counter).DayOfWeek)} ({DateTime.Today.AddDays(2 + counter): dd-MM-yyyy})",
                                                DateTime.Today.AddDays(2 + counter).ToString());
                counter++;
                buttons.Add(temp);
            };

            var other = days.Where(d => !orderedDays.Any(a => a == d)).OrderBy(d1 => d1);

            int counter2 = 0;
            foreach (var ot in other)
            {
                var temp = ButtonRow($"📅{Enum.GetName(typeof(DayOfWeek), DateTime.Today.AddDays(2 + counter2).DayOfWeek)} ({DateTime.Today.AddDays(2 + counter2): dd-MM-yyyy})",
                                                DateTime.Today.AddDays(2 + counter).ToString());
                counter2++;
                buttons.Add(temp);
            };

            return new()
            {
                Text = "Choose a day when the reminder should fire,or type a custom date in format dd-MM-yyyy",
                Buttons = new(buttons)
            };
        }

        public ContentResult AceptDateAndAskTime(MessageContext ctx)
        {
            if (DateTime.TryParse(ctx.Message.Text, out var date))
            {
                SetCachedValue(DATE, date, ctx.RecipientChatId);

                List<ushort> hours = new();
                for (ushort i = 0; i < 24; i++)
                {
                    hours.Add(i);
                }

                return new()
                {
                    Text = "Now, select the hour, or type custom:",
                    Edited = true,
                    Buttons = new(hours.Select(h => Button($"🕒{h}", h.ToString())).Chunk(4))
                };
            }
            else
            {
                ForbidMovingNext();
                return Text("Something went wrong.I Could not parse the date.");
            }
        }

        public ContentResult AcceptTimeAndAskForMinutes(MessageContext ctx)
        {
            if (byte.TryParse(ctx.Message.Text, out byte bt) && bt < 24 && bt >= 0)
            {
                SetCachedValue(HOUR, bt);
                List<ushort> hours = new();
                for (ushort i = 0; i < 60; i += 5)
                {
                    hours.Add(i);
                }

                return new()
                {
                    Text = "Now, select the minutes, or type custom:",
                    Edited = true,
                    Buttons = new(hours.Select(h => Button($"🕒{h}", h.ToString())).Chunk(4))
                };

            }
            else
            {
                ForbidMovingNext();
                return Text("Could not parse hours.Try Again");
            }
        }

        public ContentResult SaveTime(MessageContext ctx)
        {
            if (byte.TryParse(ctx.Message.Text, out var bt))
            {
                var mins = bt;
                var hours = GetCachedValue<byte>(HOUR,true);
                var date = GetCachedValue<DateTime>(DATE,true);
                var finalDate = date.AddMinutes(mins).AddHours(hours);
                var reminderId = GetCachedValue<Guid>(ScheduleReminderChunkPipeline.REMINDERID_CACHEKEY,true);
                var reminder = _service.Get(reminderId);
                reminder.ReminderTime = finalDate;
                _service.Update(reminder);

                return Text($"☑️Done. You scheduled a reminder on {finalDate: dd-MM-yyyy}, at {finalDate: HH:mm}");
            }
            else
            {
                ForbidMovingNext();
                return Text("Could not parse minutes.Please,try again");
            }
        }
    }
}
