using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application.Chatting.Core.Caching;

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
    public T Get<T>(string key)
    {
        var got = Data.Data.TryGetValue(key, out var value);

        return !got ? default : JsonConvert.DeserializeObject<T>(value);
    }
    public T Get<T>()
    {
        return Get<T>(nameof(T));
    }

    public void Set(string key, object value)
    {
        Data.Data[key] = JsonConvert.SerializeObject(value);
    }
    public void Set<T>(T value)
    {
        Set(nameof(T), value);
    }
}