using Common.Entites;
using System.Collections.Generic;

namespace Infrastructure.TextUI.Core.PipelineBaseKit
{
    public class TelegramMessage
    {
        public string Text { get; set; }
        public long ChatId { get; set; }
        public long UserId { get; set; }
        public List<UploadedFileDto> Files { get; set; }
    }
}