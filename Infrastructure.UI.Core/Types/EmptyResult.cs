using Infrastructure.TextUI.Core.Interfaces;

namespace Infrastructure.TextUI.Core.Types
{
    public class EmptyResult : ContentResult
    {
        public override bool InvokeNextImmediately { get; set; } = true;
    }
}
