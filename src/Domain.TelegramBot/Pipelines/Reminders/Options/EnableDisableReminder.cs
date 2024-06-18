using Application.Services.Reminders;
using System;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Commands.Pipelines.Reminders.Options;

[Route("/enable_disable_reminder", "Enable\\Disable Reminder")]
public class EnableDisableReminderCommand : ITelegramCommand
{
    private readonly ReminderService _reminderService;
    public EnableDisableReminderCommand(ReminderService service)
    {
        _reminderService = service;
    }
    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var reminderId = ctx.Cache.Get<Guid>(SelectOneReminderCommand.SELECTEDREMINDERID_CACHEKEY);
        _reminderService.ToggleEnabledDisabledState(reminderId);
        return ContentResponse.Text("Done");
    }
}
