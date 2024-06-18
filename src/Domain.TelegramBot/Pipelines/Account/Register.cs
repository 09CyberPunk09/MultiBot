using Application.Services.Users;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Commands.Pipelines.Account;

[Route("/register", "🚀 Register")]
public class RegisterCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptNameAndAskEmail>();
        builder.Stage<AcceptEmailAndAskPassword>();
        builder.Stage<AcceptPasswordAndSignUp>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Enter your name:");
    }
}

class RegisterDataCacheModel
{
    public string Name { get; set; }
    public string Email { get; set; }
}

public class AcceptNameAndAskEmail : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var cacheModel = new RegisterDataCacheModel()
        {
            Name = ctx.Message.Text
        };
        ctx.Cache.Set(cacheModel);
        return ContentResponse.Text("Enter you email:");
    }
}

public class AcceptEmailAndAskPassword : ITelegramStage
{
    public UserAppService UserService { get; init; }
    public AcceptEmailAndAskPassword(UserAppService userService)
    {
        UserService = userService;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var email = ctx.Message.Text;
        if (!Regex.IsMatch(email, "^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$"))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("You entered a not valid email. Try again");
        }

        var emailExists = UserService.GetUserByEmail(email) != null;
        if (emailExists)
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("The given email already exists in the system");
        }

        var cachedModel = ctx.Cache.Get<RegisterDataCacheModel>();
        cachedModel.Email = email;
        ctx.Cache.Set(cachedModel);

        return ContentResponse.Text("Enter you passrod:");
    }
}

public class AcceptPasswordAndSignUp : ITelegramStage
{
    public UserAppService UserService { get; init; }
    public AcceptPasswordAndSignUp(UserAppService userService)
    {
        UserService = userService;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var cachedModel = ctx.Cache.Get<RegisterDataCacheModel>();
        var pass = ctx.Message.Text;
        var email = cachedModel.Email;
        var name = cachedModel.Name;
        var id = UserService.SignUp(name, email, pass).Result;
        //TODO: COMPLETE THE REGISTRATION FLOW AND SIGN IN FLOW
        return ContentResponse.Text("You are successfully signed up in the system. Enter /menu for getting the main menu");

    }
}