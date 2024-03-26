using Application.Chatting.Core.Context;
using Application.Chatting.Core.Repsonses;
using System.Threading.Tasks;

namespace Application.Chatting.Core.Interfaces;
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