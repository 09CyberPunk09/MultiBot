using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.Types;
using Persistence.Caching.Redis;
using Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;
using SystemUser = Persistence.Sql.Entites.User;

namespace Infrastructure.UI.Core.MessagePipelines
{
    public class MessagePipelineBase : IMessagePipeline
	{
		protected Cache cache = new();

		private MessageContext _context;
		public Func<MessageContext, ContentResult> Current { get; set; }
		public List<Func<MessageContext, ContentResult>> Stages { get; set; }
		public bool IsLooped { get; set; }
		public Action<ContentResult, MessageContext> StagePostAction { get; set; }
		public int CurrentActionIndex { get; set; }
		public bool IsDone { get; set; }

		public MessagePipelineBase(int currentActionIndex = 0)
		{
			InitBaseComponents();
			RegisterPipelineStages();
			Current = Stages.First();
			CurrentActionIndex = currentActionIndex;
		}
		public virtual void RegisterPipelineStages()
		{

		}

		protected void InitBaseComponents()
		{
			Stages = new();
			ConfigureBasicPostAction();
		}

		private ContentResult Execute(MessageContext ctx,int index = -1)
		{
			//try
			//{
			_context = ctx;
			CurrentActionIndex = index != -1 ? index : 0;
			var result = index == -1 ? Current(ctx) : Stages[index](ctx);
			StagePostAction?.Invoke(result, ctx);
			_context = null;
			return result;
			//}
			//catch (Exception ex)
			//{
			//	// todo: here wil be some logics for logging
			//	throw;
			//}
		}

		public ContentResult ExecuteByIndex(MessageContext ctx, int index) 
			=> Execute(ctx, index);

		public ContentResult ExecuteCurrent(MessageContext ctx)
			=> Execute(ctx);

		public void ConfigureBasicPostAction()
		{
			StagePostAction = (ContentResult r, MessageContext ctx) =>
			{
				IsDone = false;
				if(!(CurrentActionIndex + 1 < Stages.Count))
                {
					ctx.MoveNext = false;
					IsDone = true;
					ctx.PipelineEnded = true;
				}
			};
		}


        protected static ContentResult Text(string text)
        {
			//todo: implement delayd messages and fire-and-forget messages
            return new() { Text = text };
        }

		protected BotMessage YesNo(string question)
        {
            var markups = new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData("Yes", true.ToString()),
                InlineKeyboardButton.WithCallbackData("No", false.ToString())
            };

            return new BotMessage()
			{
				Text = question,
				Buttons = new InlineKeyboardMarkup(markups.ToArray())
			};
		}


	protected SystemUser GetCurrentUser()
        {
            //todo: implement getting from cache
            //todo: add caching library
            using (var ctx = new SqlServerDbContext())
            {
				return ctx.Users.FirstOrDefault(u => u.TelegramUserId.HasValue && u.TelegramUserId == _context.Recipient);
            }
        }
    }
}