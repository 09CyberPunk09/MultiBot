using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextUI.Core.PipelineBaseKit;

namespace Application.TelegramBot.Pipelines.Old.IOInstances
{
    internal class MiddlewareHandler
    {
        private readonly ILifetimeScope _scope;
        private List<IMessageHandlerMiddleware> _stages = new();

        public MiddlewareHandler(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public void AddMiddleware<TMiddleware>() where TMiddleware : IMessageHandlerMiddleware
        {
            _stages.Add((TMiddleware)_scope.Resolve(typeof(TMiddleware)));
        }

        public async Task<bool> ExecuteMiddlewares(MessageContext ctx)
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
