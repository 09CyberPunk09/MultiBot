namespace Application.TelegramBot.Commands.Middlewares
{
    //public class UserAllowedFeatureMiddleware : IMessageHandlerMiddleware
    //{
    //    private readonly IMessageSender _sender;
    //    public UserAllowedFeatureMiddleware(IMessageSender sender)
    //    {
    //        _sender = sender;
    //    }
    //    public Task<bool> Execute(ILifetimeScope scope, MessageContext ctx)
    //    {
    //        var features = ctx.CurrentPipeline.Type.GetCustomAttribute<FeaureFlagsAttribute>(true) as FeaureFlagsAttribute;
    //        if (features == null)
    //        {
    //            return Task.FromResult(true);
    //        }
    //        else
    //        {
    //            var userFeatureFlags = ctx.User.FeatureFlags.Select(x => x.FeatureFlag);
    //            var anyMatches = userFeatureFlags.Any(x => features.FeatureFlags.Any(y => x == y));
    //            if (!anyMatches)
    //            {
    //                _sender.SendMessage(new()
    //                {
    //                    Text = "You do not have access to this command.",
    //                    //RecipientChatId = ctx.Message.ChatId,
    //                });
    //            }
    //            return Task.FromResult(anyMatches);
    //        }

    //    }
    //}
}
