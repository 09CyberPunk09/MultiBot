using Application.Services;
using Application.TelegramBot.Commands.Pipelines.DefaultBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.ChatEngine.Commands;
using TelegramBot.ChatEngine.Commands.Repsonses;
using static TelegramBot.ChatEngine.Commands.Repsonses.Menu;

namespace Application.TelegramBot.Pipelines.V2.Pipelines.NotesV2;

[Route("/new-note", "📋 New note")]
public class AddNoteCommand : ITelegramCommand
{
    public void DefineStages(StageMapBuilder builder)
    {
        builder
            .Stage<SaveNoteStage>()
            .Stage<AcceptTagginNewNoteDesicion>()
            .Stage<TagNewlySavedNote>();
    }

    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        //TODO: CREATE ANOTHER COMMAND FOR IT
        //DO THE SAME FOR THIS FUNCITONALITY IN REMINDERS AND TODO
        var noteTextFromUnrecognizedCommand = ctx.Cache.Get<string>(SuggestActionsCommand.TEXTCONTENT_CACHEKEY);
        if(noteTextFromUnrecognizedCommand != null)
        {
            ctx.Response.InvokeNextImmediately = true;
            ctx.Message.Text = noteTextFromUnrecognizedCommand;
            return ContentResponse.New(null);
        }
        return ContentResponse.Text("Enter Note text:");
    }
}
public class SaveNoteStage : ITelegramStage
{
    private readonly NoteAppService _noteService;
    public const string NOTEID_CACHEKEY = "NoteId";
    public SaveNoteStage(NoteAppService noteAppService)
    {
        _noteService = noteAppService;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        string noteText = ctx.Message.Text;
        var noteTextFromUnrecognizedCommand = ctx.Cache.Get<string>(SuggestActionsCommand.TEXTCONTENT_CACHEKEY, true);
        if (noteTextFromUnrecognizedCommand != null)
        {
            noteText = noteTextFromUnrecognizedCommand;
        }

        Guid? id = _noteService.Create(ctx.User.Id, noteText).Id;
        ctx.Cache.Set(NOTEID_CACHEKEY, id);

        return Task.FromResult(StageResult.ContentResult(new()
        {
            Text = "✅ Note saved. Do you want to add tags the note?",
            Menu = new(MenuType.MessageMenu, new[]
            {
                new[] {new Button("Yes", true.ToString()) ,
                    new Button("No", false.ToString()) }
            })
        }));
    }
}
public class AcceptTagginNewNoteDesicion : ITelegramStage
{
    private readonly TagAppService _tagService;
    public const string TAGDICTIONARY_CACHEKEY = "TagsDictionary";

    public AcceptTagginNewNoteDesicion(TagAppService tagAppService)
    {
        _tagService = tagAppService;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var text = ctx.Message.Text;
        if (bool.TryParse(text, out var desicion))
        {
            if (desicion)
            {
                var tags = _tagService.GetAll(ctx.User.Id);

                var b = new StringBuilder();
                b.AppendLine("Enter the number of a tag or tags(separate them using comma) to select which tags you want to attach the note to:");

                int counter = 0;
                var dictionary = new Dictionary<int, Guid>();

                foreach (var item in tags)
                {
                    ++counter;
                    b.AppendLine($"🔷 {counter}. {item.Name}");
                    dictionary[counter] = item.Id;
                }

                ctx.Cache.Set(TAGDICTIONARY_CACHEKEY, dictionary);

                return ContentResponse.Text(b.ToString());
            }
            else
            {
                ctx.Response.SetPipelineEnded();
                return ContentResponse.Text("Ok");
            }
        }
        else
        {
            return ContentResponse.Text("Ok,move next");
        }
    }
}
public class TagNewlySavedNote : ITelegramStage
{
    private readonly TagAppService _tagService;
    private readonly NoteAppService _noteService;

    public TagNewlySavedNote(TagAppService tagAppService, NoteAppService noteAppService)
    {
        _tagService = tagAppService;
        _noteService = noteAppService;
    }
    public Task<StageResult> Execute(TelegramMessageContext ctx)
    {
        var noteId = ctx.Cache.Get<Guid>(SaveNoteStage.NOTEID_CACHEKEY);
        var text = ctx.Message.Text;
        var dict = ctx.Cache.Get<Dictionary<int, Guid>>(AcceptTagginNewNoteDesicion.TAGDICTIONARY_CACHEKEY);
        try
        {
            int[] numbers = GetValidated(text, 1, dict.Count);
            numbers
                .ToList()
                .ForEach(x =>
                {
                    var tagId = dict[x];
                    _noteService.TagNote(noteId, tagId);
                });
        }
        catch (IndexOutOfRangeException _)
        {
            ctx.Response.ForbidNextStageInvokation();
            return ContentResponse.Text("⛔️ Enter a number or numbers form the suggested list");
        }
        return ContentResponse.Text("🫡 Done");
    }

    /// <summary>
    /// Parses numbers from string
    /// </summary>
    /// <param name="input">inout string</param>
    /// <param name="minNumber"> min number of range</param>
    /// <param name="maxNumber"> max number of range</param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"> is thrown if there is a number outside of the given range</exception>
    private int[] GetValidated(string input, int minNumber, int maxNumber)
    {
        int[] result = null;
        bool singleNumber = int.TryParse(input, out int rparsingResult);
        if (singleNumber)
        {
            result = new[] { rparsingResult };
        }
        else //if the string contains more than ont value - parse them
        {
            var preparatedString = input.Replace(" ", "");
            var matches = preparatedString
                                        .Split(",")
                                        .All(x => int.TryParse(x, out _));
            if (matches)
            {
                result = preparatedString
                    .Split(",")
                    .Select(x => int.Parse(x))
                    .ToArray();
            }
        }

        if (result != null)
        {
            //validate by range
            bool validatedByRange = result.All(n => n >= minNumber && n <= maxNumber);

            if (!validatedByRange)
                throw new IndexOutOfRangeException();
            //get distinct values
            result = result
                            .Distinct()
                            .ToArray();
        }
        return result;
    }
}