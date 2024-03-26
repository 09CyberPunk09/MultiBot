
using Newtonsoft.Json;

namespace Common;

public static class ObjectExtensions
{
    public static T FromJson<T>(this string data)
    {
        return JsonConvert.DeserializeObject<T>(data);
    }

    public static string ToJson(this object data)
    {
        return JsonConvert.SerializeObject(data);
    }
}
