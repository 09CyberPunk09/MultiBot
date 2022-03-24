using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core;
using Infrastructure.TextUI.Core.Attributes;
using Infrastructure.TextUI.Core.Interfaces;
using Infrastructure.TextUI.Core.MessagePipelines;
using Infrastructure.TextUI.Core.Types;
using System.ComponentModel;

namespace Infrastructure.TelegramBot.MessagePipelines.System
{
    [Route("/purge-cache-data")]
    [Description("Use this command purging chat cache data")]
    internal class PurgeWholeCachePipeline : MessagePipelineBase
    {
        private readonly CacheService _cacheService;

        public PurgeWholeCachePipeline(CacheService cacheService, ILifetimeScope scope) : base(scope)
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
                _cacheService.Purge();
                return Text("Done");
            }
            else
            {
                return Text("Operation is canceled.");
            }
        }
    }
}