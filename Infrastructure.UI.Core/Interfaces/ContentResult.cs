namespace Infrastructure.UI.Core.Interfaces
{
    public class ContentResult
	{
        public bool Succeeded { get; set; }
        public ContentResult()
        {
            Succeeded = true;
        }
        public string Text { get; set; }
	}
}
