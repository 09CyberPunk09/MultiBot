using Autofac;
using Persistence.Sql;
using System.Linq;
using SystemUser = Common.Entites.User;

namespace Infrastructure.TextUI.Core.PipelineBaseKit
{
    public class PipelineChunk : Pipeline
    {
        public PipelineChunk(ILifetimeScope scope) : base(scope)
        {
        }



        protected SystemUser GetCurrentUser(MessageContext ctx)
        {
            //todo: implement getting from cache
            //todo: add caching library
            using (var _dbContext = new LifeTrackerDbContext())
            {
                return _dbContext.Users.FirstOrDefault(u => u.TelegramChatId.HasValue && u.TelegramChatId == ctx.Recipient);
            }
        }
    }
}