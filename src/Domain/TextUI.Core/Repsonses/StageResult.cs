using System.Threading.Tasks;

namespace Application.TextCommunication.Core.Repsonses;

public class StageResult
{
    public ContentResultV2 Content { get; set; }
    public string NextStage { get; set; }

    public static StageResult ContentResult(ContentResultV2 result) => new()
    {
        Content = result
    };
    public static Task<StageResult> TaskContentResult(ContentResultV2 result) => Task.FromResult(new StageResult()
    {
        Content = result
    });
}
