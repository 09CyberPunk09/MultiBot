using Common.Entites;

namespace LifeTracker.Web.Core.Models.IncomeModels.Reminders
{
    public class CreateReminderIncomeModel
    {
        public Reminder Reminder { get; set; }
        public Guid UserId { get; set; }
    }
}