using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextUI.Core.PipelineBaseKit;

namespace Domain.TelegramBot.IOInstances
{
    internal class MiddlewareHandler
    {
        private readonly ILifetimeScope _scope;
        private List<IMessageHandlerMiddleware> _stages = new();

        public MiddlewareHandler(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public void AddMiddleware<TMiddleware>(TMiddleware middleware) where TMiddleware : IMessageHandlerMiddleware
        {
            _stages.Add(middleware);
        }

        public async Task<bool> ExecuteStages(MessageContext ctx)
        {
            foreach (var stage in _stages)
            {
                var result = await stage.Execute(_scope, ctx);
                if (!result)
                    return false;
            }
            return true;
        }
    }

}
