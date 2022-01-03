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
        //todo: move to config.json
        private readonly string _serverAddress = "localhost:6379";
        private readonly ConfigurationOptions options = new()
        {
            EndPoints = { { "localhost", 6379 } },
            AllowAdmin = true
        };

        //add an enuum with values System,PipelineMetadata, Pipeline where the value will be the database ids 
        //todo: Add expiration
        public Cache(DatabaseType dbType = DatabaseType.System)
        {
            //todo: move to config.json
            _dbType = dbType;
            redis = ConnectionMultiplexer.Connect(options);
            db = redis.GetDatabase((int)dbType);
        }

        public TResult Get<TResult>(string key)
        {
            var data = db.StringGet(new RedisKey(key));
            return data != default ? JsonConvert.DeserializeObject<TResult>(data.ToString()) : default;
        }

        public void Set(string key, object value)
        {
            string valueToSet = JsonConvert.SerializeObject(value);
            db.StringSet(new RedisKey(key), new RedisValue(valueToSet), TimeSpan.FromDays(90));
        }

        public void PurgeDatabase()
        {
            redis.GetServer(options.EndPoints.First()).FlushDatabase((int)_dbType, CommandFlags.HighPriority);
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
