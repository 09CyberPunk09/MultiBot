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
            PipelineEnded = true;
        }
    }
}
