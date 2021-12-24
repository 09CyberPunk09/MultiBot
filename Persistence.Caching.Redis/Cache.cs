using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace Persistence.Caching.Redis
{
    public class Cache : ICache
    {
        private readonly ConnectionMultiplexer redis;
        private readonly IDatabase db;

        //add an enuum with values System,PipelineMetadata, Pipeline where the value will be the database ids 
        //todo: Add expiration
        public Cache()
        {
            //todo: move to config.json
            redis = ConnectionMultiplexer.Connect("localhost");
            db = redis.GetDatabase();
        }
        public TResult Get<TResult>(string key)
        {
            var data = db.StringGet(new RedisKey(key));
            return data != default ? JsonConvert.DeserializeObject<TResult>(data.ToString()) : default;
        }

        public void Set(string key, object value)
        {
            string valueToSet = JsonConvert.SerializeObject(value);
            db.StringSet(new RedisKey(key), new RedisValue(valueToSet));
        }



        [Serializable]
        class CachePayload
        {
            public string TypeName { get; set; }
            public long ChatId { get; set; }
            public string Key { get; set; }
        }

        public T GetValueForChat<T>(string key,long chatId)
        {
            CachePayload get = new()
            {
                ChatId = chatId,
                Key = key,
                TypeName = GetType().FullName
            };
            return Get<T>(JsonConvert.SerializeObject(get));
        }

        public void SetValueForChat(string key, object value,long chatId)
        {
            CachePayload cacheKey = new()
            {
                ChatId = Convert.ToInt64(chatId),
                Key = key,
                TypeName = GetType().FullName
            };
            Set(JsonConvert.SerializeObject(cacheKey), value);
        }

    }
}
