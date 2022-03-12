using Infrastructure.Kernel;
using Infrastructure.UI.Core.Types;
using System.Threading.Tasks;

namespace Infrastructure.UI.Core.Interfaces
{
    public interface IResultSender : IStartStop
    {
        Task SendMessage(ContentResult message, long recipient);
    }
}
