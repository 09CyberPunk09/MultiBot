using System;

namespace Persistence.Caching.Redis.TelegramCaching
{
    [Serializable]
    public class CachePayload
    {
        public long ChatId { get; set; }
        public string Key { get; set; }
    }
}
