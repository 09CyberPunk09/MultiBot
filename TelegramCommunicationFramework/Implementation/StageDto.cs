namespace TelegramBot.ChatEngine.Implementation;

internal class StageDto
{
    /// <summary>
    /// Command text
    /// </summary>
    public string Command { get; set; }
    /// <summary>
    /// Type name of a stage.Is used only if a stage is not in the command stage map, eg it is external
    /// </summary>
    public string Stage { get; set; }
    /// <summary>
    /// is used to iterate through the command stage map
    /// </summary>
    public int? StageIndex { get; set; }
}