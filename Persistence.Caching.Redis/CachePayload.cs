using System;

namespace Persistence.Caching.Redis
{
    [Serializable]
    public class CachePayload
    {
        public long ChatId { get; set; }
        public string Key { get; set; }
    }
}