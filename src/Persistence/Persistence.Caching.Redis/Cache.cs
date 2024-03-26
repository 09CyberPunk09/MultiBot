using Common.Configuration;
using Newtonsoft.Json;
using NLog;
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
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static ConfigurationOptions options;

        private static int _port;
        private static string _host;

        static Cache()
        {
            var configuration = ConfigurationHelper.GetConfiguration();
            _host = configuration["Redis:Default:HostName"];
            _port = Convert.ToInt32(configuration["Redis:Default:Port"]);

        }

        public Cache(DatabaseType dbType = DatabaseType.System)
        {
            _dbType = dbType;
            try
            {
                options = new()
                {
                    EndPoints = { { _host, Convert.ToInt32(_port) } },
                    AllowAdmin = true
                };
                redis = ConnectionMultiplexer.Connect(options);
            }
            catch (RedisConnectionException ex)
            {
                logger.Fatal(ex);
                //logger.Info($"Rettrying to connect redis on container host");
                //options = new()
                //{
                //    EndPoints = { { "redis" } },
                //    AllowAdmin = true
                //};
                //redis = ConnectionMultiplexer.Connect(options);
            }

            db = redis.GetDatabase((int)dbType);
        }

        public TResult Get<TResult>(string key, bool getThanDelete = false)
        {
            var redisKey = new RedisKey(key);
            var data = getThanDelete ? db.StringGetDelete(redisKey) : db.StringGet(redisKey);
            return data != default ? JsonConvert.DeserializeObject<TResult>(data.ToString()) : default;
        }

        public void Set(string key, object value)
        {
            string valueToSet = JsonConvert.SerializeObject(value, settings: new()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });

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

        public void SetDictionary(string key, Dictionary<string, string> value)
        {
            db.KeyDelete(key);
            HashEntry[] preparedValues = value.Select(x => new HashEntry(x.Key, x.Value)).ToArray();
            db.HashSet(key, preparedValues);
        }

        public Dictionary<string, string> GetDictionary(string key)
        {
            var entries = db.HashGetAll(key);
            if (entries == null)
            {
                return null;
            }
            Dictionary<string, string> result = entries.ToDictionary(x => x.Name.ToString(), x => x.Value.ToString());

            return result;
        }
    }
}