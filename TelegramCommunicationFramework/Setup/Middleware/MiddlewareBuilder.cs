namespace TelegramBot.ChatEngine.Setup.Middleware
{
    public class MiddlewareBuilder
    {
        /// <summary>
        /// Middlewares which are invoked once a command is called
        /// </summary>
        public OperationMiddlewareBuilder PerCommandMiddleware { get; } = new();
        /// <summary>
        /// Middlewares which are executer per every message
        /// </summary>
        public OperationMiddlewareBuilder PerMessageMiddleware { get; } = new();
    }
}