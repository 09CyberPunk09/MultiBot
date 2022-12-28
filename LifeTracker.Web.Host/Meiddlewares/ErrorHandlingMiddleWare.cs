using NLog;

namespace LifeTracker.Web.Host.Meiddlewares
{
    public class ErrorHandlingMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ErrorHandlingMiddleWare(RequestDelegate next) =>
        _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExtensionsAsync(exception);
            }
        }

        private Task HandleExtensionsAsync(Exception exception)
        {
            logger.Error(exception);
            return Task.CompletedTask;
        }
    }
}