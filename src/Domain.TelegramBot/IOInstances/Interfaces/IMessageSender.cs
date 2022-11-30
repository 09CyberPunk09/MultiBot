using Infrastructure.TextUI.Core.PipelineBaseKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TelegramBot.Pipelines.IOInstances.Interfaces
{
    public interface IMessageSender
    {
        void SendMessage(ContentResult result);
        Task SendMessageAsync(ContentResult result);
    }
}
