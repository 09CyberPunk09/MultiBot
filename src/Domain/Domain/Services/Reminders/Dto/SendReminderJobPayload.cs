using Common;
using Common.Entites.Scheduling;

namespace Application.Services.Reminders.Dto;

public class SendReminderJobPayload : JobConfigurationPayload
{
    public string Text { get; set; }
    public ScheduleExpressionDto ScheduleExpression { get; set; }
    public long[] TelegramChatIds { get; set; }

}
