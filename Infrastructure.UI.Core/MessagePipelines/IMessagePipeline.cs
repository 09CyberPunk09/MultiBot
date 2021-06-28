using Infrastructure.UI.Core.Interfaces;
using System;
using System.Collections.Generic;

//todo: extend interface structure for error handling
namespace Infrastructure.UI.Core.MessagePipelines
{
	/// <summary>
	/// Message pipeline is a simple way to build complicated user-interaction scenarios for consuming messages step-by-step 
	/// </summary>
	public interface IMessagePipeline
	{
		Func<IPipelineContext, IContentResult> Current { get; set; }

		List<Func<IPipelineContext, IContentResult>> Stages {get;set;}

	}
}
