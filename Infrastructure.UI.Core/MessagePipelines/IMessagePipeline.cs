using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.Types;
using System;

namespace Infrastructure.UI.Core.MessagePipelines
{
    /// <summary>
    /// Message pipeline is a simple way to build complicated user-interaction scenarios for consuming messages step-by-step. The user communication is organized in a message-response way. 
    /// </summary>
	//todo: add missing methods to this interface
    public interface IMessagePipeline
    {
        Action<Stage, MessageContext> StagePostAction { get; set; }

        ContentResult Execute(MessageContext ctx, string stageName);
        void RegisterPipelineStages();

        public int CurrentActionIndex { get; set; }
        public bool IsDone { get; set; }

        bool IsLooped { get; set; }

        void ConfigureBasicPostAction();
    }
}
