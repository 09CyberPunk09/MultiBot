using Autofac;
using Infrastructure.UI.Core.Types;
using Persistence.Sql;
using System.Linq;
using SystemUser = Persistence.Sql.Entites.User;

namespace Infrastructure.UI.Core.MessagePipelines
{
    public class PipelineChunk : Pipeline
    {
        public PipelineChunk(ILifetimeScope scope) :base(scope)
        {
        }

		protected SystemUser GetCurrentUser(MessageContext ctx)
		{
			//todo: implement getting from cache
			//todo: add caching library
			using (var _dbContext = new SqlServerDbContext())
			{
				return _dbContext.Users.FirstOrDefault(u => u.TelegramUserId.HasValue && u.TelegramUserId == ctx.Recipient);
			}
		}

	}
}
