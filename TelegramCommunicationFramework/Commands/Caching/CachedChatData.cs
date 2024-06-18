using Newtonsoft.Json;

namespace TelegramBot.ChatEngine.Commands.Caching;

/// <summary>
/// Serializable chat data object
/// </summary>
public class CachedChatData
{
    public Dictionary<string, string> Data { get; set; } = new();
}

/// <summary>
/// wrapper with functionality for cached chat data
/// </summary>
public class CachedChatDataWrapper
{
    public CachedChatDataWrapper()
    {
        Data = new();
    }
    public CachedChatDataWrapper(CachedChatData data)
    {
        if (data == null)
            Data = new();
        else
            Data = data;
    }
    public CachedChatData Data { get; init; }

    public void Remove(string key)
    {
        Data.Data.Remove(key);
    }

    public void Remove<T>(T _)
    {
        Data.Data.Remove(typeof(T).Name);
    }


    public T Get<T>(string key, bool getThanDelete = false)
    {
        var got = Data.Data.TryGetValue(key, out var value);
        if (got && getThanDelete)
        {
            Data.Data.Remove(key);
        }
        return !got ? default : JsonConvert.DeserializeObject<T>(value);
    }
    public T Get<T>(bool getThanDelete = false)
    {
        return Get<T>(typeof(T).Name, getThanDelete);
    }

    public void Set(string key, object value)
    {
        Data.Data[key] = JsonConvert.SerializeObject(value, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
    }
    public void Set<T>(T value)
    {
        Set(typeof(T).Name, value);
    }
}