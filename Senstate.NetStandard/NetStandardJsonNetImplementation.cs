using Newtonsoft.Json;
using Senstate.CSharp_Client;

namespace Senstate.NetStandard
{
   public class NetStandardJsonNetImplementation : ISenstateJson
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
