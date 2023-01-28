using Application.Chatting.Core.Repsonses;
using Application.Chatting.Core.Routing;
using Application.Chatting.Core.StageMap;
using Application.Services.Reminders;
using Application.TelegramBot.Commands.Core.Context;
using Application.TelegramBot.Commands.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.TelegramBot.Commands.Pipelines.Reminders.Options;

[Route("/edit_reminder_text", "Edit Reminder Text")]
public class EditReminderText : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<SaveRenamedReminder>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        return ContentResponse.Text("Enter new reminder text:");
    }
}

public class SaveRenamedReminder : ITelegramStage
{
    private readonly ReminderService _reminderService;
    public SaveRenamedReminder(ReminderService reminderService)
    {
        _reminderService = reminderService;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var reminderId = ctx.Cache.Get<Guid>(SelectOneReminderCommand.SELECTEDREMINDERID_CACHEKEY);
        var reminder = _reminderService.Get(reminderId);
        reminder.Name = ctx.Message.Text;
        _reminderService.Update(reminder);

        return ContentResponse.Text("Done");
    }
}
