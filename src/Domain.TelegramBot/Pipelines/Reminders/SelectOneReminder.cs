using Application.Services.Reminders;
using Application.TelegramBot.Commands.Pipelines.Reminders.Options;
using Common;
using Common.Entites;
using Common.Entites.Scheduling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Repsonses;
using TelegramBot.ChatEngine.Commands.Routing;

namespace Application.TelegramBot.Commands.Pipelines.Reminders;

[Route("/select_reminder", "Configure reminder")]
public class SelectOneReminderCommand : ITelegramCommand
{
    public const string SELECTEDREMINDERID_CACHEKEY = "SelectedReminderId";
    public const string REMINDERDict_CACHEKEY = "Reminders";
    private readonly ReminderService _service;
    public SelectOneReminderCommand(ReminderService service)
    {
        _service = service;
    }

    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<ValidateRmeinderNumberAndSendMenu>();
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var userId = ctx.User.Id;
        var reminders = _service.GetAll(userId);
        StringBuilder sb = new();
        sb.AppendLine("Your reminders:");
        sb.AppendLine();
        Dictionary<int,Guid> cachedReminderList =  new();
        int index = 0;
        foreach (var reminder in reminders)
        {
            index++;
            AppendReminderInfo(reminder, sb, index);
            cachedReminderList[index] = reminder.Id;

        }
        ctx.Cache.Set(REMINDERDict_CACHEKEY,cachedReminderList);
        return ContentResponse.Text(sb.ToString());
    }

    private void AppendReminderInfo(Reminder reminder, StringBuilder sb, int index)
    {
        sb.AppendLine($"🔶 {index}. {reminder.Name}");
    }
}

public class ValidateRmeinderNumberAndSendMenu : ITelegramStage
{
    private readonly ReminderService _reminderService;
    private readonly RoutingTable _routingTable;
    public ValidateRmeinderNumberAndSendMenu(ReminderService reminderService,RoutingTable routingTable)
    {
        _reminderService = reminderService;
        _routingTable = routingTable;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var reminders = ctx.Cache.Get<Dictionary<int, Guid>>(SelectOneReminderCommand.REMINDERDict_CACHEKEY);
        if(!int.TryParse(ctx.Message.Text,out int number) || !(reminders.Count + 1 >= number && number > 0))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("Please enter a valid number");
        }
        var reminderId = reminders[number];
        var reminder = _reminderService.Get(reminderId);
        ctx.Cache.Set(SelectOneReminderCommand.SELECTEDREMINDERID_CACHEKEY, reminderId);

        ctx.Cache.Remove(SelectOneReminderCommand.REMINDERDict_CACHEKEY);

        StringBuilder sb = new();

        sb.AppendLine($"🔶 {reminder.Name}");
        var schedulerExpr = reminder.SchedulerExpression.FromJson<ScheduleExpressionDto>();
        sb.AppendLine($"Fires: {schedulerExpr.Description}");
        string activeOrNot = reminder.IsActive ? "Enabled" : "Disabled";
        sb.AppendLine($"🟢🔴 {activeOrNot}");
        sb.AppendLine();
        sb.AppendLine("Select an option for this reminder:");

        return ContentResponse.New(new()
        {
            Text = sb.ToString(),
            Menu = new(Menu.MenuType.MessageMenu, new[]
            {
                new[]{ new Button(_routingTable.AlternativeRoute<EnableDisableReminderCommand>(), _routingTable.AlternativeRoute<EnableDisableReminderCommand>()) },
                new[]{ new Button(_routingTable.AlternativeRoute<EditReminderText>(), _routingTable.AlternativeRoute<EditReminderText>()) },
                new[]{ new Button(_routingTable.AlternativeRoute<DeleteReminder>(), _routingTable.AlternativeRoute<DeleteReminder>()) },
            })
        });
    }
}
