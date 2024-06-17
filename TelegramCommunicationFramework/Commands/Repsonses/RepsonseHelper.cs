namespace TelegramBot.ChatEngine.Commands.Repsonses;

public class RepsonseHelper
{
    public bool InvokeNextImmediately { get; set; } = false;
    public bool CanIvokeNext { get; set; } = true;
    public bool PipelineEnded { get; set; }
    public bool DeleteLastUserMessage { get; set; }
    public bool DeleteLastBotMessage { get; set; }
    //TODO: Implement in telegram message handling
    //TODO: Make the previous message menu not deleted if forbidnextstageinvokation is used
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
