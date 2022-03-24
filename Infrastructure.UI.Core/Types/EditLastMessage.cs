using Infrastructure.TextUI.Core.Interfaces;

namespace Infrastructure.TextUI.Core.Types
{
    public class EditLastMessage : ContentResult
    {
        public BotMessage NewMessage { get; set; }
    }
}