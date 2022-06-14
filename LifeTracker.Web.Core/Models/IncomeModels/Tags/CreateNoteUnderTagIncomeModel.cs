namespace LifeTracker.Web.Core.Models.IncomeModels.Tags
{
    public class CreateNoteUnderTagIncomeModel
    {
        public Guid TagId { get; set; }
        public string Text { get; set; }
        public Guid UserId { get; set; }
    }
}