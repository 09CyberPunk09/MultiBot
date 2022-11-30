using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using Persistence.Caching.Redis;
using System.Text.RegularExpressions;

namespace Domain.TelegramBot.MessagePipelines.Start
{
    [Route("/register", "🚀 Register")]
    public class RegisterPipeline : MessagePipelineBase
    {
        private const string EMAIL_CACHEKEY = "SignUp_Email";
        private const string NAME_CACHEKEY = "SignUp_Name";
        public RegisterPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStageMethod(AskName);
            RegisterStageMethod(AcceptNameAndAskEmail);
            RegisterStageMethod(AcceptEmailAndAskPassword);
            RegisterStageMethod(AcceptPasswordAndSignUp);
        }
        public ContentResult AskName() => Text("Enter your name:");
        public ContentResult AcceptNameAndAskEmail()
        {
            SetCachedValue(NAME_CACHEKEY, MessageContext.Message.Text);
            return Text("Enter you email:");
        }

        public ContentResult AcceptEmailAndAskPassword()
        {
            var email = MessageContext.Message.Text;
            if (!Regex.IsMatch(email, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$"))
            {
                Response.ForbidNextStageInvokation();
                return Text("You entered a not valid email. Try again");
            }

            var userService = _scope.Resolve<UserAppService>();
            var emailExists = userService.GetUserByEmail(email) != null;
            if (emailExists)
            {
                Response.ForbidNextStageInvokation();
                return Text("The given email already exists in the system");
            }

            SetCachedValue(EMAIL_CACHEKEY, email);

            return Text("Enter you passrod:");
        }

        public ContentResult AcceptPasswordAndSignUp()
        {
            var pass = MessageContext.Message.Text;
            var email = GetCachedValue<string>(EMAIL_CACHEKEY);
            var name = GetCachedValue<string>(NAME_CACHEKEY);
            var userService = _scope.Resolve<UserAppService>();
            var id = userService.SignUp(name, email, pass).Result;
            var cache = new Cache(DatabaseType.TelegramLogins);
            cache.Set(MessageContext.Message.UserId.ToString(), id.ToString());
            return Text("You are successfully signed up in the system. Enter /menu for getting the main menu");
        }
    }
}
