namespace TelegramBot.ChatEngine.Commands.Routing;

/// <summary>
/// The ways how a command can be called from text chat
/// </summary>
public class RouteDto
{
    public string Route { get; set; }
    public string AlternativeRoute { get; set; }
}
