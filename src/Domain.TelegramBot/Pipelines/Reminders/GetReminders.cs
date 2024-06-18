using Application.Services.Reminders;
using Common;
using Common.Entites;
using Common.Entites.Scheduling;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;

namespace Application.TelegramBot.Commands.Pipelines.Reminders;

[Route("/reminders", "🧾 Show Reminders")]
public class GetRemindersCommand : ITelegramCommand
{

    private readonly ReminderService _service;
    public GetRemindersCommand(ReminderService service)
    {
        _service = service;
    }

    public void DefineStages(StageMapBuilder builder)
    {
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var userId = ctx.User.Id;
        var reminders = _service.GetAll(userId);
        StringBuilder sb = new();
        sb.AppendLine("Your reminders:");
        sb.AppendLine();
        foreach (var reminder in reminders)
        {
            AppendReminderInfo(reminder, sb);
        }
        return ContentResponse.Text(sb.ToString());
    }

    private void AppendReminderInfo(Reminder reminder, StringBuilder sb)
    {
        sb.AppendLine();
        sb.AppendLine($"🔶 {reminder.Name}");
        var schedulerExpr = reminder.SchedulerExpression.FromJson<ScheduleExpressionDto>();
        sb.AppendLine($"Fires: {schedulerExpr.Description}");
        string activeOrNot = reminder.IsActive ? "Enabled" : "Disabled";
        sb.AppendLine($"🟢🔴 {activeOrNot}");
    }
}
