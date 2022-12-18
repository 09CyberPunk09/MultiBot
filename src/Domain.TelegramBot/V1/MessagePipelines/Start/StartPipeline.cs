using Application.Services.Users;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Persistence.Caching.Redis;
using System;
using System.Net.Mail;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Start
{
    [Route("/start")]
    public class StartPipeline : MessagePipelineBase
    {
        private const string EMAIL_CACHEKEY = "EmailAddress";
        private readonly UserAppService _userService;

        public StartPipeline(UserAppService userService, ILifetimeScope scope) : base(scope)
        {
            _userService = userService;
        }

        public override void RegisterPipelineStages()
        {
            RegisterStageMethod(AskEmail);
            RegisterStageMethod(AcceptkEmail);
            RegisterStageMethod(AcceptPasswordAndSignIn);
            //todo: implement
            // IntegrateChunkPipeline<MenuPipeline>();
            IsLooped = true;
        }

        public ContentResult AskEmail()
        {
            return Text("Welcome! If you gave already an account in LifeTracker, please, enter you email address. If you don't have an account - use command /register or visit our site - ");
        }

        public ContentResult AcceptkEmail()
        {
            var email = MessageContext.Message.Text;
            try
            {
                MailAddress m = new MailAddress(email);
                SetCachedValue(EMAIL_CACHEKEY, email);
            }
            catch (FormatException)
            {
            }

            var exists = _userService.GetUserByEmail(email) != null;

            if (!exists)
            {
                Response.ForbidNextStageInvokation();
                return Text("Seems like the entered email is not registered in the system.");
            }

            return Text("Enter password:");
        }
        public ContentResult AcceptPasswordAndSignIn()
        {
            _userService.TelegramLogOut(MessageContext.RecipientUserId);
            var email = GetCachedValue<string>(EMAIL_CACHEKEY, true);
            Response.DeleteLastUserMessage = true;
            var success = _userService.TelegramLogin(email, MessageContext.Message.Text, MessageContext.RecipientUserId);
            var user = _userService.GetUserByEmail(email);
            if (success)
            {
                //TODO: Move these two lines to a separate service which will be named something like TelegramAuthentificationService
                var cache = new Cache(DatabaseType.TelegramLogins);
                cache.Set(MessageContext.Message.UserId.ToString(), user.Id.ToString());
                return Text("Welcome!");
            }
            else
            {
                Response.ForbidNextStageInvokation();
                return Text("You entered not vcalid values. Try again");
            }
        }
    }
}