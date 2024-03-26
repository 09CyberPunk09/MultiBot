using Application.Chatting.Core.Context;
using Application.Chatting.Core.StageMap;

namespace Application.Chatting.Core.Interfaces;
public interface ICommand
{
    /// <summary>
    /// Method is called on routes initialization for declaration a sequence of stages. NOTE: If a stage returns a sign to execute another stage,the new stage will be executed next, and then - the next defined one from the routes map
    /// </summary>
    /// <param name="builder"></param>
    void DefineStages(StageMapBuilder builder);
}
/// <summary>
/// Basic interface for commands.
/// </summary>
public interface ICommand<TContext> : ICommand, IStage<TContext> where TContext : MessageContext
{

}
