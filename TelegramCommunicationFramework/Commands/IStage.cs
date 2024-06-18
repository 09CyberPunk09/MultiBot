namespace TelegramBot.ChatEngine.Commands;
/// <summary>
/// Base interface for stages. Not usable for implementing it
/// </summary>
public interface IStage
{

}
/// <summary>
/// Base interface for all stages and commands which allows them being executed
/// </summary>
/// <typeparam name="TContext"></typeparam>
public interface IStage<TContext> : IStage where TContext : TelegramMessageContext
{
    Task<StageResult> Execute(TContext ctx);
}