using Infrastructure.UI.Core.Interfaces;
using System;
using System.Collections.Generic;

//todo: extend interface structure for error handling
namespace Infrastructure.UI.Core.MessagePipelines
{
	/// <summary>
	/// Message pipeline is a simple way to build complicated user-interaction scenarios for consuming messages step-by-step. The user communication is organized in a message-response way. 
	/// </summary>
	public interface IMessagePipeline
	{
		Func<IMessageContext, IContentResult> Current { get; set; }

		List<Func<IMessageContext, IContentResult>> Stages { get; set; }

		Action<IContentResult, IMessageContext> StagePostAction { get; set; }

		IContentResult ExecuteCurrent(IMessageContext ctx);
		//todo: move call to ctor
		void RegisterPipelineStages();

		public int CurrentActionIndex { get; set; }
		public bool IsDone { get; set; }

		bool IsLooped { get; set; }

		void ConfigureBasicPostAction();
	}
}
