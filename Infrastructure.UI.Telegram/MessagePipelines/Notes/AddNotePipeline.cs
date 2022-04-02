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
            RegisterStage(AskToEnterANote);
            RegisterStage(SaveNote);
            IsLooped = true;
        }

        private ContentResult AskToEnterANote(MessageContext ctx)
        {
            return Text("Enter Note text:");
        }

        private ContentResult SaveNote(MessageContext ctx)
        {
            _noteService.Create(ctx.Message.Text, GetCurrentUser().Id);
            return Text("✅ Note saved");
        }
    }
}