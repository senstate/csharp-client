using System;

namespace Senstate.CSharp_Client
{
   public interface ISenstateJson
   {
      string ConvertToString(object data);
   }

   public interface ISenstateWebSocket
   {
      void CreateSocket(Uri targetEndpoint);
      void SendToSocket(string jsonData);
   }
}
