using Infrastructure.TextUI.Core.PipelineBaseKit;

namespace TextUI.Core.PipelineBaseKit
{
    public class PipelineExecutionResponse
    {
        public int NextStageIndex { get; set; }
        public string NextPipelineTypeFullName { get; set; }
        public ContentResult Result { get; set; }
        public bool CanIvokeNext { get; set; }
        public bool PipelineEnded { get; set; }
        public bool DeleteLastUserMessage { get; set; }
        public bool DeleteLastBotMessage { get; set; }
    }
}
