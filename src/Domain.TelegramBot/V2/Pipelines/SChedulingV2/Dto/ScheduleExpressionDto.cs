using System;
using System.Collections.Generic;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Dto
{
    public class ScheduleExpressionDto
    {
        public const string CACHEKEY = "SchedulingMode";
        public ScheduleExpressionDto(List<string> crons)
        {
            Crons = crons;
            FireAndForget = false;
        }

        public ScheduleExpressionDto(List<DateTime> fireAndForgetDates)
        {
            FireAndForgetDates = fireAndForgetDates;
            FireAndForget = true;
        }

        /// <summary>
        /// For deserialization only
        /// </summary>
        public ScheduleExpressionDto()
        {

        }

        public List<string> Crons { get; set; }
        public bool FireAndForget { get; set; } = false;
        public List<DateTime> FireAndForgetDates { get; set; }
    }
}
