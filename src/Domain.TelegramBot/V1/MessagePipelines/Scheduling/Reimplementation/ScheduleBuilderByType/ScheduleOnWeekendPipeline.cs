using Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Dto;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Collections.Generic;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Reimplementation.ScheduleBuilderByType
{
    [Route("/schedule_on_weekends")]
    public class ScheduleOnWeekendPipeline : MessagePipelineBase
    {
        public ScheduleOnWeekendPipeline(ILifetimeScope scope) : base(scope)
        {
            RegisterStageMethod(AskTime);
            RegisterStageMethod(TryAcceptTime);
        }

        public ContentResult AskTime()
        {
            return Text("Enter time in format HH:MM, HH:MM,...");
        }

        public ContentResult TryAcceptTime()
        {
            var text = MessageContext.Message.Text;
            try
            {
                var result = TimeParser.Parse(text);
                //0 0,35 7 ? * SUN,TUE,THU *
                var crons = new List<string>();
                foreach (var tuple in result)
                {
                    var cron = $"0 0,{tuple.Item2} {tuple.Item1} ? * SUN,SAT *";
                    crons.Add(cron);
                }
                var schedulerConfig = new ScheduleExpressionDto(crons);

                SetCachedValue(ScheduleExpressionDto.CACHEKEY, schedulerConfig);

                return Text("Schedule configured");
            }
            catch (System.ArgumentException)
            {
                Response.ForbidNextStageInvokation();
                return Text("You entered a not valid value. Try entering a string in format HH:MM, HH:MM,...");
            }

        }


    }
}
