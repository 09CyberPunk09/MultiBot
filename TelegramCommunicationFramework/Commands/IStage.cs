using System.Threading.Tasks;

namespace TelegramBot.ChatEngine.Commands;
/// <summary>
/// Base interface for stages. Not usable for implementing it
/// </summary>
public interface IStage
{

}
public interface IStage<TContext> : IStage where TContext : TelegramMessageContext
{
    Task<StageResult> Execute(TContext ctx);
}