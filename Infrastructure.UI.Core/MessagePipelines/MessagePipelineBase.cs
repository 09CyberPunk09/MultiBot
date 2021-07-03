using Infrastructure.UI.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UI.Core.MessagePipelines
{
	//todo: create an attrinute with a number which will indicate the position in the list of stages instead of manual adding them into the list
	public class MessagePipelineBase : IMessagePipeline
	{
		public Func<IPipelineContext, IContentResult> Current { get; set; }
		public List<Func<IPipelineContext, IContentResult>> Stages { get; set; }
		public bool IsLooped { get; set; }
		public Action<IContentResult, IPipelineContext> StagePostAction { get; set; }
		public int CurrentActionIndex { get; set; }
		public bool IsDone { get; set; }

		public MessagePipelineBase()
		{

		}
		public virtual void RegisterPipelineStages()
		{
			InitBaseComponents();
		}

		protected void InitBaseComponents()
		{
			Stages = new();
			ConfigureBasicPostAction();
		}

		public IContentResult ExecuteCurrent(IPipelineContext ctx)
		{
			//try
			//{
			var result = Current(ctx);
			StagePostAction?.Invoke(result, ctx);
			return result;
			//}
			//catch (Exception ex)
			//{
			//	//here wil be some logics for logging
			//	throw;
			//}
		}

		public void ConfigureBasicPostAction()
		{
			StagePostAction = (IContentResult r, IPipelineContext ctx) =>
			{
				ctx.MoveNext = true;
				if (CurrentActionIndex+1 == Stages.Count)
				{
					if (IsLooped)
					{
						Current = Stages.First();
						CurrentActionIndex = 0;
					}
					else
					{
						CurrentActionIndex = 0;
						IsDone = true;
					}
				}
				else
				{
					CurrentActionIndex++;
					Current = Stages[CurrentActionIndex];
				}
			};
		}
	}
}