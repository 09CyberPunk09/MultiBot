using Application.TextCommunication.Core.Context;
using Application.TextCommunication.Core.Repsonses;
using System.Threading.Tasks;

namespace Application.TextCommunication.Core.Interfaces;
/// <summary>
/// Bas einterfacew for stages. Not usable for implementing it
/// </summary>
public interface IStage
{

}
public interface IStage<TContext> : IStage where TContext : MessageContext
{
    Task<StageResult> Execute(TContext ctx);
}