namespace Application.TextCommunication.Core.Repsonses;

public enum PhotResultMode
{
    Url, Content
}

public class PhotoResult
{
    private readonly PhotResultMode _mode;

    public PhotResultMode Mode
    {
        get { return _mode; }
    }

    public PhotoResult(string url)
    {
        Url = url;
        _mode = PhotResultMode.Url;
    }

    public PhotoResult(byte[] content)
    {
        PhotoContent = content;
        _mode = PhotResultMode.Content;
    }

    public string Url { get; }
    public byte[] PhotoContent { get; }
    public string FileName { get; set; }
}
