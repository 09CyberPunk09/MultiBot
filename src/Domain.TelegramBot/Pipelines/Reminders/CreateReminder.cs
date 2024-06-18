using Application.Services.Reminders;
using Application.TelegramBot.Commands.Pipelines.DefaultBehavior;
using Application.TelegramBot.Commands.Pipelines.SChedulingV2.Helpers;
using Common.Entites.Scheduling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Repsonses;
using static Application.TelegramBot.Commands.Pipelines.Reminders.AcceptReminderTextAndAskFiringType;

namespace Application.TelegramBot.Commands.Pipelines.Reminders;

[Route("/create_reminder", "⏰ Create Reminder")]
public class CreateReminderCommand : ITelegramCommand
{
    public const string REMINDER_TEXT = "ReminderText";
    public void DefineStages(StageMapBuilder builder)
    {
        builder.Stage<AcceptReminderTextAndAskFiringType>();
        builder.Stage<AcceptReminderScheduleTypeAndAskSchedule>();
        builder.Stage<AcceptSchedulerTypeAndShowSchedulerConfiguration>();
        //between these two stages will be scheduling stages
        builder.Stage<AcceptScheduleExpressionAndSavReminder>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var reminderTextFromUnrecognizedCommand = ctx.Cache.Get<string>(SuggestActionsCommand.TEXTCONTENT_CACHEKEY);
        if (reminderTextFromUnrecognizedCommand != null)
        {
            ctx.Response.InvokeNextImmediately = true;
            ctx.Message.Text = reminderTextFromUnrecognizedCommand;
            return ContentResponse.New(new());
        }

        return ContentResponse.Text("Enter Reminder text:");
    }
}

public class AcceptReminderTextAndAskFiringType : ITelegramStage
{
    public enum ReminderScheduleType
    {
        FireAndForget = -56789032,
        Recurent
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        ctx.Cache.Set(CreateReminderCommand.REMINDER_TEXT, ctx.Message.Text);
        var menu = new Menu(Menu.MenuType.MessageMenu, new[]
        {
            new[]{ new Button("⚡️ Fire-and-Forget",((int)ReminderScheduleType.FireAndForget).ToString()) },
            new[]{ new Button("♻️ Recurent", ((int)ReminderScheduleType.Recurent).ToString()) }
        });
        return ContentResponse.New(new()
        {
            Text = "Select the type of schedule:",
            Menu = menu
        });
    }
}
public class AcceptReminderScheduleTypeAndAskSchedule : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        if(!Enum.TryParse<ReminderScheduleType>(ctx.Message.Text,out var type))
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("Select a value from menu");
        }
        IEnumerable<IEnumerable<Button>> menu = ScheduleTypeListingHelper
    .Modes
    .Where(x => type switch
        {
            ReminderScheduleType.FireAndForget => x.Item1.Recurent == false,
            ReminderScheduleType.Recurent => x.Item1.Recurent,
            _ => throw new NotImplementedException(),
        })
    .Select(x => new[] { new Button(x.Item1.Name, x.Item1.Name) })
    .ToArray();

        return ContentResponse.New(new()
        {
            Text = "Select the scheduler for your reminder:",
            Menu = new Menu(Menu.MenuType.MessageMenu, menu)
        });
    }
}

public class AcceptSchedulerTypeAndShowSchedulerConfiguration : ITelegramStage
{
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var isValidSelection = ScheduleTypeListingHelper.Modes.Any(x => x.Item1.Name == ctx.Message.Text);
        if (!isValidSelection)
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("Select an option from the menu");
        }
        var type = ScheduleTypeListingHelper.Modes.First(x => x.Item1.Name == ctx.Message.Text).Item2;
        ctx.Response.InvokeNextImmediately = true;
        return Task.FromResult(new StageResult()
        {
            NextStage = type.FullName
        });
    }
}


public class AcceptScheduleExpressionAndSavReminder : ITelegramStage
{
    private readonly ReminderService _service;
    public AcceptScheduleExpressionAndSavReminder(ReminderService service)
    {
        _service = service;
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        string reminderName = ctx.Cache.Get<string>(CreateReminderCommand.REMINDER_TEXT, true);

        var reminderTextFromUnrecognizedCommand = ctx.Cache.Get<string>(SuggestActionsCommand.TEXTCONTENT_CACHEKEY, true);
        if (reminderTextFromUnrecognizedCommand != null)
        {
            reminderName = reminderTextFromUnrecognizedCommand;
        }


        var schedulerExpression = ctx.Cache.Get<ScheduleExpressionDto>(ScheduleExpressionDto.CACHEKEY, true);

        _service.Create(new()
        {
            //UserId = ctx.User.Id,
            ScheduleExpression = schedulerExpression,
            Text = reminderName
        });

        return ContentResponse.Text("✅ Done");
    }
}

