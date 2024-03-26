using Common.Entites.Scheduling;
using System;

namespace Application.Services.Reminders.Dto
{
    public class CreateReminderDto
    {
        public string Text { get; set; }
        public ScheduleExpressionDto ScheduleExpression { get; set; }
        public Guid UserId { get; set; }
    }
}
