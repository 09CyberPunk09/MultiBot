using Common.Entites;
using System.Collections.Generic;

namespace Persistence.Common.DataAccess
{
    public class NonRelationalSchema
    {
        public virtual List<Note> Notes { get; set; }
        public virtual List<PredefinedAnswer> PredefinedAnswers { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public virtual List<Question> Questions { get; set; }
        public virtual List<Tag> Tags { get; set; }
        public virtual List<User> Users { get; set; }
        public virtual List<TimeTrackingActivity> TimeTrackingActivities { get; set; }
        public virtual List<TimeTrackingEntry> TimeTrackingEntries { get; set; }
    }
}