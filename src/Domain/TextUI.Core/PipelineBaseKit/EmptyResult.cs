namespace Infrastructure.TextUI.Core.PipelineBaseKit
{
    public class EmptyResult : ContentResult
    {
        public override bool InvokeNextImmediately { get; set; } = true;
    }
}