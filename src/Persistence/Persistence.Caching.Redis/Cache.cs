using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Caching.Redis
{
    public class Cache : ICache
    {
        private readonly ConnectionMultiplexer redis;
        private readonly IDatabase db;
        private readonly DatabaseType _dbType;

        private readonly string _serverAddress = "localhost:6379";

        private readonly ConfigurationOptions options = new()
        {
            //todo: move to config.json
            EndPoints = { { "localhost", 6379 } },
            AllowAdmin = true
        };

        //todo: Add expiration
        public Cache(DatabaseType dbType = DatabaseType.System)
        {
            _dbType = dbType;
            redis = ConnectionMultiplexer.Connect(options);
            db = redis.GetDatabase((int)dbType);
        }

        public TResult Get<TResult>(string key,bool getThanDelete = false)
        {
            var redisKey = new RedisKey(key);
            var data = getThanDelete ? db.StringGetDelete(redisKey) : db.StringGet(redisKey);
            return data != default ? JsonConvert.DeserializeObject<TResult>(data.ToString()) : default;
        }

        public void Set(string key, object value)
        {
            string valueToSet = JsonConvert.SerializeObject(value);
            db.StringSet(new RedisKey(key), new RedisValue(valueToSet), TimeSpan.FromDays(90));
        }

        public List<string> GetAllkeys()
        {
            List<string> listKeys = new List<string>();
            var keys = redis.GetServer(options.EndPoints.First()).Keys((int)_dbType);
            listKeys.AddRange(keys.Select(key => (string)key).ToList());
            return listKeys;
        }

        public void PurgeDatabase()
        {
            redis.GetServer(options.EndPoints.First()).FlushDatabase((int)_dbType);
        }

        protected IEnumerable<RedisKey> GetALLKeys()
        {
            return redis.GetServer(options.EndPoints.First()).Keys((int)_dbType);
        }

        protected void Remove(RedisKey key)
        {
            db.KeyDelete(key);
        }
    }
}