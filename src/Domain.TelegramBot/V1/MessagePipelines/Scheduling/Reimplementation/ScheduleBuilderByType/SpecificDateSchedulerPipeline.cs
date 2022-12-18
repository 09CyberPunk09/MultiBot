using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Scheduling.Reimplementation.ScheduleBuilderByType
{
    [Route("/specific_date_scgeduler")]
    public class SpecificDateSchedulerPipeline : MessagePipelineBase
    {
        public SpecificDateSchedulerPipeline(ILifetimeScope scope) : base(scope)
        {

        }


    }
}
