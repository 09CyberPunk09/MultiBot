using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services.Reminders;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Reminders.Options;

[Route("/delete_reminder", "Delete Reminder")]
public class DeleteReminder : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptDeleteReminderDesicion>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.New(new()
        {
            Text = "Do you want to delete the reminder?",
            Menu = new Menu(Menu.MenuType.MessageMenu, new[]
            {
                new[] { new Button("Yes", true.ToString() )},
                new[] { new Button("No", false.ToString() )}
            })
        });   
    }
}

public class AcceptDeleteReminderDesicion : ITelegramStage
{
    private readonly ReminderService _reminderService; 
    public AcceptDeleteReminderDesicion(ReminderService reminderService) 
    {
        _reminderService = reminderService;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        if(!bool.TryParse(ctx.Message.Text, out var result))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("Select a menu item");
        }

        if (!result)
        {
            return ContentResponse.Text("okay");
        }

        var reminderId = ctx.Cache.Get<Guid>(SelectOneReminderCommand.SELECTEDREMINDERID_CACHEKEY, true);
        _reminderService.Delete(reminderId);

        return ContentResponse.Text("Done");
    }
}
