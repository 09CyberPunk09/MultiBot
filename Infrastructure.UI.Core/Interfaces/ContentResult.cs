namespace Infrastructure.UI.Core.Interfaces
{
    public class ContentResult
    {
        public string Text { get; set; }
        public virtual bool InvokeNextImmediately { get; set; } = false;
    }
}
