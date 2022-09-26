using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Threading.Tasks;

namespace TextUI.Core.PipelineBaseKit
{
    public interface IMessageHandlerMiddleware
    {
        /// <summary>
        /// A Method which needs to be executed before medssage nhandling
        /// </summary>
        /// <param name="ctx">message context</param>
        /// <returns>True, if a stage was executed successfully</returns>
        Task<bool> Execute(ILifetimeScope scope, MessageContext ctx);
    }

}
