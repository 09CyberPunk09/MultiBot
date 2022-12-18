using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.ComponentModel;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.System
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

        public ContentResult Confirm() => ResponseTemplates.YesNo("Are you shure you want to purge this chat data?");

        public ContentResult Purge()
        {
            bool answer;
            if (!bool.TryParse(MessageContext.Message.Text, out answer))
            {
                Response.ForbidNextStageInvokation();
                return Text("Please use the dialog for confirming this operation.");
            }

            if (answer)
            {
                _cacheService.Purge(MessageContext.User.Id);
                return Text("Done");
            }
            else
            {
                return Text("Operation is canceled.");
            }
        }
    }
}