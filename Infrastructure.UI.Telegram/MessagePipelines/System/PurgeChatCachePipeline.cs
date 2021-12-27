using Application.Services;
using Autofac;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UI.TelegramBot.MessagePipelines.System
{
    [Route("/purge-chat-cache-data")]
    [Description("Use this command purging chat cache data")]
    internal class PurgeChatCachePipeline : MessagePipelineBase
    {
        private readonly CacheService _cacheService;
        public PurgeChatCachePipeline(CacheService cacheService, ILifetimeScope scope) : base(scope)
        {
            _cacheService = cacheService;
        }

        public override void RegisterPipelineStages()
        {
            RegisterStage(Confirm);
            RegisterStage(Purge);
        }

        public ContentResult Confirm(MessageContext ctx) => ResponseTemplates.YesNo("Are you shure you want to purge this chat data?");

        public ContentResult Purge(MessageContext ctx)
        {
            bool answer;
            if (!bool.TryParse(ctx.Message.Text, out answer))
            {
                ctx.MoveNext = false;
                ctx.PipelineStageSucceeded = false;
                return Text("Please use the dialog for confirming this operation.");
            }

            if (answer)
            {
                _cacheService.Purge(GetCurrentUser().Id);
                return Text("Done");
            }
            else
            {
                return Text("Operation is canceled.");
            }
            
        }
    }
}
