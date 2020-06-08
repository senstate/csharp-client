using Newtonsoft.Json;

namespace Senstate.CSharp_Client.Tests
{
    public class DummySerializer : ISenstateJson
    {
        public string ConvertToString(object data)
        {
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
