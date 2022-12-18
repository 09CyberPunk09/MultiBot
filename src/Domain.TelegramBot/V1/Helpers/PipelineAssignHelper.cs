using Application.TelegramBot.Pipelines.Old.IOInstances;
using Application.TextCommunication.Core.Routing;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Persistence.Caching.Redis.TelegramCaching;
using System.Reflection;

namespace Application.TelegramBot.Pipelines.Old.Helpers
{
    public static class PipelineAssignHelper
    {
        private static readonly TelegramCache _cache = new();
        public static void SetPipeline<TPipeline>(long chatId) where TPipeline : MessagePipelineBase
        {
            var command = typeof(TPipeline).GetCustomAttribute<RouteAttribute>().Route;
            _cache.SetValueForChat(MessageHandler.CURRENT_MESSAGEPIPELINE_TYPE_NAME, command, chatId);
        }
    }
}