using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.ComponentModel;

namespace Infrastructure.TelegramBot.MessagePipelines.Notes
{
    [Route("/new-note", "📋 New note")]
    [Description("Use this command for creating notes")]
    public class AddNotePipeline : MessagePipelineBase
    {
        private readonly NoteAppService _noteService;

        public AddNotePipeline(NoteAppService noteService, ILifetimeScope scope) : base(scope)
        {
            _noteService = noteService;
        }

        public override void RegisterPipelineStages()
        {
            RegisterStageMethod(AskToEnterANote);
            RegisterStageMethod(SaveNote);
            IsLooped = true;
        }

        private ContentResult AskToEnterANote()
        {
            return Text("Enter Note text:");
        }

        private ContentResult SaveNote()
        {
            _noteService.Create(MessageContext.Message.Text, MessageContext.User.Id);
            return Text("✅ Note saved");
        }
    }
}