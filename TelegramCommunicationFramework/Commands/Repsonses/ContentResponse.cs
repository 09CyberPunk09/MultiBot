using System.Threading.Tasks;

namespace TelegramBot.ChatEngine.Commands.Repsonses;

public class ContentResponse
{
    public static Task<StageResult> New(ContentResultV2 result)
    {
        return Task.FromResult(new StageResult()
        {
            Content = result,
        });
    }
    public static Task<StageResult> Text(string text)
    {
        return Task.FromResult(new StageResult()
        {
            Content = new()
            {
                Text = text
            },
        });
    }
}
