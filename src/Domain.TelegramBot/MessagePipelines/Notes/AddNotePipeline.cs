using Application.Services;
using Application.Services.Files;
using Application.Services.FileStorage;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.TelegramBot.MessagePipelines.Notes
{
    [Route("/new-note", "📋 New note")]
    [Description("Use this command for creating notes")]
    public class AddNotePipeline : MessagePipelineBase
    {
        private readonly NoteAppService _noteService;
        private readonly FileStorageService _storageService;
        private readonly StoragePathService _pathService;
        private readonly TagAppService _tagService;
        private const string TAGDICTIONARY_CACHEKEY = "TagsDictionary";
        private const string NOTEID_CACHEKEY = "NoteId";

        public AddNotePipeline(
            NoteAppService noteService,
            StoragePathService pathService,
            FileStorageService storageService, 
            TagAppService tagAppService,
            ILifetimeScope scope) : base(scope)
        {
            _noteService = noteService;
            _storageService = storageService;
            _pathService = pathService;
            _tagService = tagAppService;
        }

        public override void RegisterPipelineStages()
        {
            RegisterStageMethod(AskToEnterANote);
            RegisterStageMethod(SaveNote);
            RegisterStageMethod(YesNoResponse);
            RegisterStageMethod(TagNote);
            IsLooped = true;
        }

        private ContentResult AskToEnterANote()
        {
            return Text("Enter Note text:");
        }

        private ContentResult SaveNote()
        {
            var files = MessageContext.Message.Files;
            Guid? id = null;
            bool hasMultipleFiles = false;
            if (files != null || (files != null && files.Count != 0))
            {
                if(!files.All(f => f.UploadSucceeded))
                {
                    Response.ForbidNextStageInvokation();
                    return Text("Sorry, these files can not be uploaded. Try again without them.");
                }
                foreach (var file in files)
                {
                    hasMultipleFiles = true;
                    var fileDto = _storageService.GetFileByHashAsync(MessageContext.User.Id, file.FileHash).Result;
                    var newHash = _storageService.UploadFileAsync(MessageContext.User.Id, fileDto.Stream, $"{_pathService.GetNotesFolderName()}/{_pathService.GetFileName(fileDto.FilePath)}");
                    id = _noteService.Create(MessageContext.User.Id, MessageContext.Message.Text, file.FileHash).Id;
                }
            }
            else
            {
                id = _noteService.Create(MessageContext.User.Id, MessageContext.Message.Text).Id;
            }
            if(!hasMultipleFiles)
                SetCachedValue(NOTEID_CACHEKEY, id);

            return new() 
            { 
                Text = hasMultipleFiles ? "✅ Note saved." : "✅ Note saved. Do you want to add tags the note?",
                Buttons = hasMultipleFiles ? null : new(new[] {
                    Button("Yes", true.ToString()),
                    Button("No", false.ToString())
                })
            };
        }

        public ContentResult YesNoResponse()
        {
            var text = MessageContext.Message.Text;
            if(bool.TryParse(text, out var desicion))
            {
                if (desicion)
                {
                    var tags = _tagService.GetAll(MessageContext.User.Id);

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

                    SetCachedValue(TAGDICTIONARY_CACHEKEY, dictionary);

                    return new ContentResult()
                    {
                        Text = b.ToString()
                    };
                }
                else
                {
                    Response.SetPipelineEnded();
                    return Text("Ok");
                }
            }
            else
            {
                return Text("Ok,move next");
            }
        }

        public ContentResult TagNote()
        {
            var noteId = GetCachedValue<Guid>(NOTEID_CACHEKEY);
            var text = MessageContext.Message.Text;
            var dict = GetCachedValue<Dictionary<int, Guid>>(TAGDICTIONARY_CACHEKEY, true);
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
                Response.ForbidNextStageInvokation();
                return Text("⛔️ Enter a number or numbers form the suggested list");
            }
            return Text("🫡 Done");
        }

        /// <summary>
        /// Parses numbers from string
        /// </summary>
        /// <param name="input">inout string</param>
        /// <param name="minNumber"> min number of range</param>
        /// <param name="maxNumber"> max number of range</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"> is thrown if there is a number outside of the given range</exception>
        private int[] GetValidated(string input,int minNumber, int maxNumber)
        {
            int[] result = null;
            bool singleNumber = int.TryParse(input, out int rparsingResult);
            if (singleNumber)
            {
                result =  new[] { rparsingResult };
            }
            else //if the string contains more than ont value - parse them
            {
                var preparatedString = input.Replace(" ", "");
                var matches = preparatedString
                                            .Split(",")
                                            .All(x => int.TryParse(x,out _));
                if (matches)
                {
                    result = preparatedString
                        .Split(",")
                        .Select(x => int.Parse(x))
                        .ToArray();
                }
            }

            if(result != null)
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
} 