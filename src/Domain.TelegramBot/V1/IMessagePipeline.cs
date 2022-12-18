using System;
using TextUI.Core.PipelineBaseKit;

namespace Infrastructure.TextUI.Core.PipelineBaseKit
{
    /// <summary>
    /// Message pipeline is a simple way to build complicated user-interaction scenarios for consuming messages step-by-step. The user communication is organized in a message-response way.
    /// </summary>
	//todo: add missing methods to this interface
    public interface IMessagePipeline
    {
        Action<Stage, MessageContext, ContentResult> StagePostAction { get; set; }

        PipelineExecutionResponse Execute(MessageContext ctx,Type pipelineType, int stageIndex);

        void RegisterPipelineStages();

        bool IsLooped { get; set; }

        void ConfigureBasicPostAction();
    }
}