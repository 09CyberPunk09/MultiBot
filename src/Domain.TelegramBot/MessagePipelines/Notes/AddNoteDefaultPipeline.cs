using Application.Services;
using Autofac;
using Infrastructure.TextUI.Core.PipelineBaseKit;
using System.Linq;

namespace Application.TelegramBot.Pipelines.MessagePipelines.Notes
{
    //[Route("/default_add_as_note")]
    //public class DefaultAddNotePipeline : MessagePipelineBase
    //{
    //    private const string MESSAGEASNOTE_CACHEKEY = "MessageToSave";
    //    private readonly NoteAppService _noteService;

    //    public DefaultAddNotePipeline(
    //        NoteAppService noteService, 
    //        ILifetimeScope scope) : base(scope)
    //    {
    //        _noteService = noteService;
    //        RegisterStageMethod(AskIfAddAsNote);
    //        RegisterStageMethod(AcceptUserDesicion);
    //    }

    //    public ContentResult AskIfAddAsNote()
    //    {
    //        SetCachedValue(MESSAGEASNOTE_CACHEKEY, MessageContext.Message);
    //        return new()
    //        {
    //            //TODO: Add replyTo user message
    //            Text = "The bot could not recognize the message as any command. Do you want to save this message as note?",
    //            Buttons = new(new[]
    //            {
    //                Button("Yes",true.ToString()),
    //                Button("No",false.ToString())
    //            })
    //        };
    //    }

    //    public ContentResult AcceptUserDesicion()
    //    {
    //        var text = MessageContext.Message.Text;
    //        if(bool.TryParse(text, out bool result))
    //        {
    //            if (result)
    //            {
    //                var messageRoSave = GetCachedValue<Message>(MESSAGEASNOTE_CACHEKEY);

    //                var files = messageRoSave.Files;
    //                if (files != null || 
    //                    (files != null && files.Count != 0))
    //                {
    //                    if (!files.All(f => f.UploadSucceeded))
    //                    {
    //                        Response.ForbidNextStageInvokation();
    //                        return Text("Sorry, these files can not be uploaded. Try again without them.");
    //                    }
    //                    foreach (var file in messageRoSave.Files)
    //                    {
    //                        _noteService.Create(
    //                            MessageContext.User.Id, 
    //                            messageRoSave.Text, 
    //                            file.FileHash);
    //                    }
    //                }
    //                else
    //                {
    //                    _noteService.Create(
    //                        MessageContext.User.Id, 
    //                        messageRoSave.Text);
    //                }
    //                return Text("✅ Got it!");
    //            }
    //            else
    //            {
    //                return Text("Ok, I do nothing☺️");
    //            }
    //        }
    //        else
    //        {
    //            return Text("Ok, I do nothing☺️");
    //        }
    //    }
    //}
}
