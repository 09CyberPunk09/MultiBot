using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextUI.Core.PipelineBaseKit
{
    public class Response
    {
        public bool CanIvokeNext { get; set; } = true;
        public bool PipelineEnded { get; set; }
        public bool DeleteLastUserMessage { get; set; }
        public bool DeleteLastBotMessage { get; set; }
        public void ForbidNextStageInvokation()
        {
            CanIvokeNext = false;
            PipelineEnded = false;
        }
        public void SetPipelineEnded()
        {

        }
    }
}
