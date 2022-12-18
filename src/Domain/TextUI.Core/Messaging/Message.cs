using Common.Entites;
using System.Collections.Generic;

namespace Application.TextCommunication.Core.Messaging;

public class Message
{
    public string Text { get; set; }
    public List<UploadedFileDto> Files { get; set; }
}
