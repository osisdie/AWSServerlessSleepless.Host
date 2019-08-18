using Newtonsoft.Json;
using ServerlessSleepless.Awaker.Abstraction;

namespace ServerlessSleepless.Awaker.Common
{
    public class CustomSerializer : ISerializer
    {
        public T Deserialize<T>(string s) where T : class
        {
            return JsonConvert.DeserializeObject<T>(s);
        }

        public string Serialize(object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
}
