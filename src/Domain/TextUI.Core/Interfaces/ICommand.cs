using Application.TextCommunication.Core.StageMap;

namespace Application.TextCommunication.Core.Interfaces;

/// <summary>
/// Basic interface for commands.
/// </summary>
public interface ICommand : IStage
{
    /// <summary>
    /// Method is called on routes initialization for declaration a sequence of stages. NOTE: If a stage returns a sign to execute another stage,the new stage will be executed next, and then - the next defined one from the routes map
    /// </summary>
    /// <param name="builder"></param>
    void DefineStages(StageMapBuilder builder);
}
