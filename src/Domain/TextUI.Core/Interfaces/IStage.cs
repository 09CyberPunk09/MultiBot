using Application.TextCommunication.Core.Context;
using Application.TextCommunication.Core.Repsonses;
using System.Threading.Tasks;

namespace Application.TextCommunication.Core.Interfaces;

public interface IStage
{
    Task<StageResult> Execute(MessageContext ctx);
}
