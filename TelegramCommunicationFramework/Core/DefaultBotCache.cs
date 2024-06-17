using Newtonsoft.Json;
namespace TelegramBot.ChatEngine.Core;

public class DefaultBotCache : IBotCache
{
    //NOW: TEST PuRPOSE
    private readonly string _filePath = "C:\\Users\\vklap\\OneDrive\\Desktop\\cache\\cache.json";
    private readonly Dictionary<string, string> _cache;

    public DefaultBotCache()
    {
        _cache = LoadCacheFromFile();
    }

    public TResult Get<TResult>(string key, bool getThanDelete = false)
    {
        if (_cache.TryGetValue(key, out var value))
        {
            if (getThanDelete)
            {
                _cache.Remove(key);
                SaveCacheToFile();
            }
            return JsonConvert.DeserializeObject<TResult>(value);
        }
        return default(TResult);
    }

    public Dictionary<string, string> GetDictionary(string key)
    {
        if (_cache.TryGetValue(key, out var value))
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(value);
        }
        return new Dictionary<string, string>();
    }

    public void Set(string key, object value)
    {
        var @string = JsonConvert.SerializeObject(value);
        _cache[key] = @string;
        SaveCacheToFile();
    }

    public void SetDictionary(string key, Dictionary<string, string> value)
    {
        _cache[key] = JsonConvert.SerializeObject(value);
        SaveCacheToFile();
    }

    private Dictionary<string, string> LoadCacheFromFile()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        }
        return new Dictionary<string, string>();
    }

    private void SaveCacheToFile()
    {
        var json = JsonConvert.SerializeObject(_cache, Formatting.Indented);
        File.WriteAllText(_filePath, json);
    }
}
