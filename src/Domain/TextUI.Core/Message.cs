using Common.Entites;
using System.Collections.Generic;

namespace Application.Chatting.Core
{
    public class TelegramMessage
    {
        public string Text { get; set; }
        public long ChatId { get; set; }
        public long UserId { get; set; }
        public List<UploadedFileDto> Files { get; set; }
    }
}