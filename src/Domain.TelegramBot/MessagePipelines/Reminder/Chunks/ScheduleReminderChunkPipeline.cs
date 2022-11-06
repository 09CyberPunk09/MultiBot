﻿using Application.Services;
using Autofac;
using Infrastructure.TelegramBot.MessagePipelines.Reminder;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Telegram.Bot.Types.ReplyMarkups;

namespace Domain.TelegramBot.MessagePipelines.Scheduling.Chunks
{
    public class ScheduleReminderChunkPipeline : PipelineChunk
    {
        public const string REMINDERID_CACHEKEY = "NewlyCreatedReminderId";
        public const string REMINDERTEXT_CACHEKEY = "NewlyCreatedReminderText";
        private readonly ReminderAppService _service;

        public ScheduleReminderChunkPipeline(ILifetimeScope scope) : base(scope)
        {
            _service = scope.Resolve<ReminderAppService>();
            RegisterStage(() =>
            {
                string text = GetCachedValue<string>(REMINDERTEXT_CACHEKEY, MessageContext.RecipientChatId);
                var res = _service.Create(new()
                {
                    Name = text
                }, MessageContext.User.Id);
                var t = MessageContext;
                SetCachedValue(REMINDERID_CACHEKEY, res.Id, MessageContext.RecipientChatId);

                return new()
                {
                    Text = "✅Gotcha. Do ypu want to make your reminder recurent or fire-and-forget?",
                    Buttons = new(new[]
                    {
                    InlineKeyboardButton.WithCallbackData("⏱Fire-and-forget",GetRoute<MakeReminderOneTimeFiringPipeline>().Route),
                    InlineKeyboardButton.WithCallbackData("♻️Recurent",GetRoute<MakeReminderRecurentPipeline>().Route)
                })
                };
            });
        }
    }
}
