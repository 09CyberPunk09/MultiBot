using Autofac;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.Types;
using Persistence.Sql;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using StageDelegate = System.Func<Infrastructure.UI.Core.Types.MessageContext, Infrastructure.UI.Core.Interfaces.ContentResult>;
using SystemUser = Persistence.Sql.Entites.User;

namespace Infrastructure.UI.Core.MessagePipelines
{
    public class MessagePipelineBase : Pipeline,IMessagePipeline
	{
        public MessagePipelineBase(ILifetimeScope scope) : base(scope)
        {
			_scope = scope;
		}

		public ContentResult Execute(MessageContext ctx,string stageName = null)
		{
			//try
			//{
			MessageContext = ctx;
			Stage stage;
			if(stageName != null)
            {
				stage = Stages.Stages.FirstOrDefault(x => x.MethodName == stageName);
			}
            else
            {
				stage = Stages.Stages.FirstOrDefault();
            }

			ctx.CurrentStage = stage;

			var result = stage.Invoke(ctx);
			StagePostAction?.Invoke(stage, ctx);
			MessageContext = null;
			return result;
			//}
			//catch (Exception ex)
			//{
			//	// todo: here wil be some logics for logging
			//	throw;
			//}
		}


		protected void IntegrateChunkPipeline<TChunk>() where TChunk : PipelineChunk
		{
			var chunk = _scope.Resolve<TChunk>(new NamedParameter("ctx",MessageContext));
			chunk.Stages.Stages.ForEach(stage => RegisterStage(stage));
		}

		protected SystemUser GetCurrentUser()
		{
			//todo: implement getting from cache
			//todo: add caching library
			using (var ctx = new SqlServerDbContext())
			{
				return ctx.Users.FirstOrDefault(u => u.TelegramUserId.HasValue && u.TelegramUserId == MessageContext.Recipient);
			}
		}


		#region ResponseTemplates
		protected ContentResult Text(string text) => ResponseTemplates.Text(text);
        #endregion
    }


    public class StageMap 
	{
		public List<Stage> Stages { get; set; } = new();
        public Stage Root { get; set; }
        public Stage Current
        {
			//todo: remake for search in stacktrace unitill the algo finds the stage which calls the method
			get => Stages.FirstOrDefault(x => (new StackTrace()).GetFrame(1).GetMethod().Name == x.MethodName);
		}

		public Stage this [int index]
        {
			get => Stages[index];
			set => Stages[index] = value;
        }
			
		public void Add(Stage stage)
        {
			if (Root == null)
            {
				Root = stage;
				Stages.Add(stage);
				return;
            }

			var last = Stages.LastOrDefault();
			last.NextStage = stage;
			Stages.Add(stage);
        }
    }



	public class Stage
    {
		private readonly string _name;
        public Stage(StageDelegate f)
        {
			Method = f;
			_name = f.Method.Name;
		}
		public StageDelegate Method { get; }
        public Stage NextStage { get; set; }
        public string MethodName { get => _name; }

		public ContentResult Invoke(MessageContext ctx)
			=> Method.Invoke(ctx);

    }
}