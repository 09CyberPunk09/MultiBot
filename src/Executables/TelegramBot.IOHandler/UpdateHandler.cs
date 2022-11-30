using Application.Services;
using Application.Services.Files;
using Application.Services.FileStorage;
using Autofac;
using Common.Entites;
using Common.Enums;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IContainer _container;
        public MessageUpdateHandler(IContainer container)
        {
            _container = container;
        }
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            //TODO: Add logging
            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
                            Text = update.Message.Text,
                            UserId = update.Message.From.Id
                        };

                        if (update.Message.Photo != null)
                        {
                            var userId = GetSystemUserId(update.Message.From.Id);
                            if(userId == null)
                            {
                                break;
                            }
                            message.Files = await DownloadAndSavePhoto(userId.Value, update.Message);
                            message.Text = update.Message.Caption;
                        }
                    }
                    break;

                case UpdateType.InlineQuery:
                    {
                        message = new()
                        {
                            Text = update.CallbackQuery.Data,
                            ChatId = update.CallbackQuery.Message.Chat.Id,
                            UserId = update.Message.From.Id
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
                            ChatId = update.CallbackQuery.Message.Chat.Id,
                            UserId = update.CallbackQuery.From.Id
                        };
                    }
                    break;

                default:
                    break;
            }
            logger.Info($"Message '{message.Text}' recieved form {message.ChatId}");
            _messageConsumer.ConsumeMessage(message);

           // return Task.CompletedTask;
        }

        private async Task<List<UploadedFileDto>> DownloadAndSavePhoto(Guid userId,Message message)
        {
            var fileHashes = new List<UploadedFileDto>();
            var client = _container.Resolve<ITelegramBotClient>();
            var storageService = _container.Resolve<FileStorageService>();
            var pathService = _container.Resolve<StoragePathService>();

            var photos = message.Photo;
            int counter = 0;
            foreach (var item in photos)
            {
                //download every third photo. Telegram gives three versions of the photo with different sizes, so we download the original
                if ((counter + 1) % 3 == 0)
                {
                    bool succeeded = false;
                    string hash = default;
                    try
                    {
                        var file = await client.GetFileAsync(item.FileId);
                        Stream fs = new MemoryStream();
                        await client.DownloadFileAsync(file.FilePath, fs);

                        hash = await storageService.UploadFileAsync(
                            userId: userId,
                            input: fs,
                            fileNameWithPath: $"{pathService.GetTemporaryPath()}/{Path.GetFileName(file.FilePath)}");
                        succeeded = true;
                    }
                    catch (Exception ex)
                    {
                        succeeded = false;
                        logger.Error(ex);
                    }
                    finally
                    {
                        fileHashes.Add(new()
                        {
                            FileHash = hash,
                            UploadSucceeded = succeeded
                        });
                    }

                }
                counter++;
            }
            return fileHashes;
        }

        private Guid? GetSystemUserId(long userId)
        {
            var userService = _container.Resolve<UserAppService>();
            return userService.GetByTgId(userId).Id;
        }
    }
}