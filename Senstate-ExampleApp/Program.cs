using Newtonsoft.Json;
using Senstate.CSharp_Client;
using System;

namespace Senstate_ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SenstateContext.AppName = "C# Console Log";
            SenstateContext.SerializerInstance = new DummySerializer();
            SenstateContext.WebSocketInstance = new DummyWebSocket();
            SenstateContext.RegisterApp();

            var stringWatcher = new Watcher(
                new WatcherMeta
                {
                    Tag = "Some Label",
                    Type = WatcherType.String,
                }
                );

            stringWatcher.SendData("This an example Text");

            Console.ReadKey();
        }

        private class DummySerializer : ISenstateJson
        {
            public string ConvertToString(object data)
            {
                return JsonConvert.SerializeObject(data);
            }
        }

        private class DummyWebSocket : ISenstateWebSocket
        {
            public void SendToSocket(string jsonData)
            {
                Console.WriteLine($"{jsonData}");
            }
        }
    }
}
