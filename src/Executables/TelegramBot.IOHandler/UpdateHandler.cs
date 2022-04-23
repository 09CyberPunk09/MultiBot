using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LifeTracker.TelegramBot.IOHandler
{
    internal class MessageUpdateHandler : IUpdateHandler
    {
        private MessageConsumer _messageConsumer = new();
        private Logger logger = LogManager.GetCurrentClassLogger();
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            //TODO: Add logging
            return Task.CompletedTask;
        }

        public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Infrastructure.TextUI.Core.PipelineBaseKit.Message message = null;
            switch (update.Type)
            {
                case UpdateType.Unknown:
                    break;

                case UpdateType.Message:
                    {
                        message = new()
                        {
                            ChatId = update.Message.Chat.Id,
                            Text = update.Message.Text
                        };
                    }
                    break;

                case UpdateType.InlineQuery:
                    {
                        message = new()
                        {
                            Text = update.CallbackQuery.Data,
                            ChatId = update.CallbackQuery.Message.Chat.Id
                        };
                        break;
                    }
                case UpdateType.ChosenInlineResult:
                    break;

                case UpdateType.CallbackQuery:
                    {
                        message = new()
                        {
                            Text = update.CallbackQuery.Data,
                            ChatId = update.CallbackQuery.Message.Chat.Id
                        };
                    }
                    break;

                default:
                    break;
            }
            logger.Info($"Message '{message.Text}' recieved form {message.ChatId}");
            _messageConsumer.ConsumeMessage(message);

            return Task.CompletedTask;
        }
    }
}