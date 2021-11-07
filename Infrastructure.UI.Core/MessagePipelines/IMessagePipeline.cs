using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.Types;
using System;
using System.Collections.Generic;

namespace Infrastructure.UI.Core.MessagePipelines
{
    /// <summary>
    /// Message pipeline is a simple way to build complicated user-interaction scenarios for consuming messages step-by-step. The user communication is organized in a message-response way. 
    /// </summary>
    public interface IMessagePipeline
	{
		Func<MessageContext, ContentResult> Current { get; set; }

		List<Func<MessageContext, ContentResult>> Stages { get; set; }

		Action<ContentResult, MessageContext> StagePostAction { get; set; }

		ContentResult ExecuteCurrent(MessageContext ctx);
		void RegisterPipelineStages();

		public int CurrentActionIndex { get; set; }
		public bool IsDone { get; set; }

		bool IsLooped { get; set; }

		void ConfigureBasicPostAction();
	}
}
