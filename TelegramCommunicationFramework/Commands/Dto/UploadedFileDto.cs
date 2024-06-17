namespace TelegramBot.ChatEngine.Commands.Dto
{
    public class UploadedFileDto
    {
        public string FileHash { get; set; }
        public bool UploadSucceeded { get; set; } = true;
        public string FileName { get; set; }
    }
}
