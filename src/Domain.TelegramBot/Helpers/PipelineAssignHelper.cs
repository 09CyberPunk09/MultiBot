using Infrastructure.TelegramBot.IOInstances;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Persistence.Caching.Redis.TelegramCaching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.TelegramBot.Helpers
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