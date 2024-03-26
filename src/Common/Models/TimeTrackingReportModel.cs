using System;

namespace Common.Models
{
    public class TimeTrackingReportModel
    {
        public TimeTrackingReportModel(DateTime d, TimeSpan t)
        {
            Date = d;
            TrackedTime = t;
        }
        public DateTime Date { get; set; }
        public TimeSpan TrackedTime { get; set; }
    }
}
