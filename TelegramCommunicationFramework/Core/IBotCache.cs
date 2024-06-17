namespace TelegramBot.ChatEngine.Core;

/// <summary>
/// Temporary cache for telegram bot where the information about user data,stages commands and temporary stage data is written.Note: the default implementation of cache writes data into a text file. If you want other persistent storage - implement this interface
/// </summary>
public interface IBotCache
{
    TResult Get<TResult>(string key, bool getThanDelete = false);
    Dictionary<string, string> GetDictionary(string key);
    void Set(string key, object value);
    void SetDictionary(string key, Dictionary<string, string> value);
}