using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Kernel
{
    public static class Extensions
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
}