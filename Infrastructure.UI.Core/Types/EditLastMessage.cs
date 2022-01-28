using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.Types;

namespace Infrastructure.TextUI.Core.Types
{
    public class EditLastMessage : ContentResult
    {
        public BotMessage NewMessage { get; set; }
    }
}
