namespace Persistence.Caching.Redis
{
    public interface ICache
    {
        void Set(string key, object value);

        TResult Get<TResult>(string key);
    }
}