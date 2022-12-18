using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.Old.MessagePipelines.Hello
{
    public class HelloMessagePipelineChunk : PipelineChunk
    {
        public HelloMessagePipelineChunk(ILifetimeScope scope) : base(scope)
        {
            RegisterStage(() =>
            {
                return Text("Pipeline chunk fire!");
            });
        }
    }
}
