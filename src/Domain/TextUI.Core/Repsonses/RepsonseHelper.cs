namespace Application.TextCommunication.Core.Repsonses;

public class RepsonseHelper
{
    public bool CanIvokeNext { get; set; } = true;
    public bool PipelineEnded { get; set; }
    public bool DeleteLastUserMessage { get; set; }
    public bool DeleteLastBotMessage { get; set; }
    //TODO: Implement in telegram message handling
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
