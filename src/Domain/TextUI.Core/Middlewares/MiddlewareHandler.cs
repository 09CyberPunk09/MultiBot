using Application.TelegramBot.Pipelines.V2.Core.Middlewares.Interfaces;
using Application.TextCommunication.Core.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.V2.Core.Middlewares
{
    public class BaseMiddlewareHandler<TContext> where TContext : MessageContext
    {
        private readonly IServiceProvider _serviceProvider;
        private List<IMiddleware<TContext>> _middlewares = new();
        public BaseMiddlewareHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        //TODO: Add default realization with generic parameter of an context
        public void Add<TMiddleware>() where TMiddleware : IMiddleware<TContext>
        {
            var resolved = _serviceProvider.GetService<TMiddleware>();
            _middlewares.Add(resolved);
        }

        public async Task<bool> ExecuteMiddlewares(TContext context)
        {
            foreach (var middleware in _middlewares)
            {
                bool succeeded = await middleware.ExecuteAsync(context);
                if(!succeeded) return false;
            }
            return true;
        }
    }
}
