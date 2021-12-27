using Application.Services;
using Autofac;
using Infrastructure.UI.Core.Attributes;
using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.MessagePipelines;
using Infrastructure.UI.Core.Types;
using System.ComponentModel;

namespace Infrastructure.UI.TelegramBot.MessagePipelines
{
    [Route("/new-note")]
	[Description("Use this command for creating notes")]
	//TODO: REQWRITE TO: FIRST WE TELL THE USER TO ADD A MESSAGE AND THEN WE SAVE IT
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
