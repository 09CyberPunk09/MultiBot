using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling
{
    public static class TimeParser
    {
        public static List<(int, int)> Parse(string input)
        {
            bool isSingleTime = Regex.IsMatch(input, "^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$");
            if (isSingleTime)
            {
                var preparedStr = input.Trim();
                var separatorPos = preparedStr.IndexOf(":");
                var hourStr = preparedStr.Substring(0, separatorPos);
                var minuteStr = preparedStr.Substring(separatorPos + 1);

                return new()
                {
                    (int.Parse(hourStr), int.Parse(minuteStr))
                };
            }

            var result = new List<(int, int)>();
            var timesStrArray = input.Split(',').Select(x => x.Trim().Replace(" ", ":"));
            foreach (var item in timesStrArray)
            {
                bool isTime = Regex.IsMatch(item, "^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$");
                if (!isTime)
                    throw new ArgumentException();

                var separatorPos = item.IndexOf(":");
                var hourStr = item.Substring(0, separatorPos);
                var minuteStr = item.Substring(separatorPos + 1);
                result.Add((int.Parse(hourStr), int.Parse(minuteStr)));

            }

            return result;
        }
    }
}
