using Newtonsoft.Json;
using System.Linq;

namespace Persistence.Caching.Redis.TelegramCaching
{
    public sealed class TelegramCache : Cache
    {
        public TelegramCache() : base(DatabaseType.Telegram)
        {

        }
        public T GetValueForChat<T>(string key, long chatId)
        {
            CachePayload get = new()
            {
                ChatId = chatId,
                Key = key
            };
            return Get<T>(JsonConvert.SerializeObject(get));
        }

        public void SetValueForChat(string key, object value, long chatId)
        {
            CachePayload cacheKey = new()
            {
                ChatId = chatId,
                Key = key
            };
            Set(JsonConvert.SerializeObject(cacheKey), value);
        }

        public void PurgeChatData(long chatId)
        {
            GetALLKeys().ToList().ForEach(key =>
            {
                if (key.ToString().Contains(key.ToString()))
                {
                    Remove(key);
                }
            });
        }

    }
}
