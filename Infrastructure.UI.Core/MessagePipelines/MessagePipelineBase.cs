using Infrastructure.UI.Core.Interfaces;
using Infrastructure.UI.Core.Types;
using Newtonsoft.Json;
using Persistence.Caching.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.UI.Core.MessagePipelines
{
    public class MessagePipelineBase : IMessagePipeline
	{
		private MessageContext _context;
		public Func<MessageContext, ContentResult> Current { get; set; }
		public List<Func<MessageContext, ContentResult>> Stages { get; set; }
		public bool IsLooped { get; set; }
		public Action<ContentResult, MessageContext> StagePostAction { get; set; }
		public int CurrentActionIndex { get; set; }
		public bool IsDone { get; set; }

		public MessagePipelineBase()
		{
			InitBaseComponents();
			RegisterPipelineStages();
			Current = Stages.First();
			CurrentActionIndex = 0;
		}
		public virtual void RegisterPipelineStages()
		{
			
		}

		protected void InitBaseComponents()
		{
			Stages = new();
			ConfigureBasicPostAction();
		}

		public ContentResult ExecuteCurrent(MessageContext ctx)
		{
			//try
			//{
			_context = ctx;
			var result = Current(ctx);
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

		public void ConfigureBasicPostAction()
		{
			StagePostAction = (ContentResult r, MessageContext ctx) =>
			{
				ctx.MoveNext = true;
				if (CurrentActionIndex + 1 == Stages.Count)
				{
					if (IsLooped)
					{
						Current = Stages.First();
						CurrentActionIndex = 0;
					}
					else
					{
						CurrentActionIndex = 0;
						IsDone = true;
					}
				}
				else
				{
					CurrentActionIndex++;
					Current = Stages[CurrentActionIndex];
				}
			};
		}

		protected ICache _cache;

		[Serializable]
		class CachePayload
        {
            public string TypeName { get; set; }
            public long ChatId { get; set; }
            public string Key { get; set; }
        }

		protected T GetCachedValue<T>(string key)
        {
			if(_cache == null)
				throw new Exception("You are trying to use the caching feature but you didnt initialize a cache in the constructor");
			CachePayload get = new()
			{
				ChatId = Convert.ToInt64(_context.Recipient),
				Key = key,
				TypeName = GetType().FullName
			};
			return _cache.Get<T>(JsonConvert.SerializeObject(get));
		}

		protected void SetValueToCache(string key,object value)
        {
			if (_cache == null)
				throw new Exception("You try to Use the caching feature but you didnt initialize a cache in the constructor");
			CachePayload cacheKey = new()
			{
				ChatId = Convert.ToInt64(_context.Recipient),
				Key = key,
				TypeName = GetType().FullName
			};
			_cache.Set(JsonConvert.SerializeObject(cacheKey),value);
		}

        protected static ContentResult Text(string text)
        {
            return new() { Text = text };
        }
    }
}