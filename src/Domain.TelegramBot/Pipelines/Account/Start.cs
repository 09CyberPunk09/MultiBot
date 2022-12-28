using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services.Users;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.Account;

[Route("/start")]
public class TelegramStart : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder
            .Stage<AcceptEmail>()
            .Stage<AcceptPasswordAndSignIn>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Welcome! If you gave already an account in LifeTracker, please, enter you email address. If you don't have an account - use command /register or visit our site - ");
    }
}
class SignInPayload
{
    public string Email { get; set; }
}
public class AcceptEmail : ITelegramStage
{
    private readonly UserAppService _userService;
    public AcceptEmail(UserAppService userAppService)
    {
        _userService = userAppService;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var email = ctx.Message.Text;
        try
        {
            MailAddress m = new MailAddress(email);
            ctx.Cache.Set(new SignInPayload()
            {
                Email = email
            });
        }
        catch (FormatException)
        {
            //todo: add punishment for moving next of email int valid
        }
        var exists = _userService.GetUserByEmail(email) != null;
        if (!exists)
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("Seems like the entered email is not registered in the system.");
        }

        return ContentResponse.Text("Enter password:");
    }
}

public class AcceptPasswordAndSignIn : ITelegramStage
{
    private readonly UserAppService _userService;
    public AcceptPasswordAndSignIn(UserAppService userAppService)
    {
        _userService = userAppService;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        _userService.TelegramLogOut(ctx.RecipientUserId);
        var email = ctx.Cache.Get<SignInPayload>().Email;
        ctx.Response.DeleteLastUserMessage = true;
        var success = _userService.TelegramLogin(email, ctx.Message.Text, ctx.RecipientUserId);
        var user = _userService.GetUserByEmail(email);
        if (success)
        {
            //TODO: Add authentificationstuff
            return ContentResponse.Text("Welcome!");
        }
        else
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("You entered not vcalid values. Try again");
        }
    }
}
