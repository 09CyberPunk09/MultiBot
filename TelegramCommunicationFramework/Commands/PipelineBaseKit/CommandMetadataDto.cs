using TelegramBot.ChatEngine.Commands.Routing;

namespace TelegramBot.ChatEngine.Commands.PipelineBaseKit;

public class CommandMetadata
{
    public Type Type { get; set; }
    public RouteDto Route { get; set; }
    public StageSequence StagesSequence { get; set; }
}
