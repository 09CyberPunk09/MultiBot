using Newtonsoft.Json;
using StackExchange.Redis;

namespace Persistence.Caching.Redis
{
    public class Cache : ICache
    {
        private readonly ConnectionMultiplexer redis;
        private readonly IDatabase db;

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
            return JsonConvert.DeserializeObject<TResult>(data.ToString());
        }

        public void Set(string key, object value)
        {
            string valueToSet = JsonConvert.SerializeObject(value);
            db.StringSet(new RedisKey(key), new RedisValue(valueToSet));
        }
    }
}
