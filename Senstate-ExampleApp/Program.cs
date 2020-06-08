using Newtonsoft.Json;
using Senstate.CSharp_Client;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Senstate_ExampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SenstateContext.AppName = "C# Console Log";
            SenstateContext.SerializerInstance = new DummySerializer();
            SenstateContext.WebSocketInstance = new DotNetWebSocket();
            SenstateContext.RegisterApp();

            var stringWatcher = new Watcher(
                new WatcherMeta
                {
                    Tag = "Some Label",
                    Type = WatcherType.String,
                    Group = "Example Group 1"
                }
                );

            var numberWatcher = new Watcher(
            new WatcherMeta
            {
                Tag = "Number",
                Type = WatcherType.Number,
                Group = "Example Group 1"
            }
      );

            var objectWatcher = new Watcher(
        new WatcherMeta
        {
            Tag = "Object",
            Type = WatcherType.Json,
            Group = "Special"
        }
  );

            try
            {
                throw new NullReferenceException();
            }
            catch(Exception ex)
            {
                ErrorSender.Send(ex);
            }

            for (var i = 0; i<10000; i++) {

                Thread.Sleep(500);
            stringWatcher.SendData($"This an example Text {i}");
                numberWatcher.SendData(i);
                Logger.SendLog(LoggerType.Debug, $"Debug {i}");
                Logger.SendLog(LoggerType.Info, $"Info {i}");


                objectWatcher.SendData(new
                {
                    example = true,
                    sub = new
                    {
                        data = i
                    }
                });
            }


            Console.ReadKey();
        }

        private class DummySerializer : ISenstateJson
        {
            public string ConvertToString(object data)
            {
                return JsonConvert.SerializeObject(data, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
        }

        private class DummyWebSocket : ISenstateWebSocket
        {
            public void CreateSocket(Uri targetEndpoint)
            {
                Console.WriteLine("Created a Dummy Socket");
            }

            public void SendToSocket(string jsonData)
            {
                Console.WriteLine($"{jsonData}");
            }
        }


        private class DotNetWebSocket : ISenstateWebSocket
        {
            ClientWebSocket m_socket = new ClientWebSocket();
            private Task connectionTask = null;

            public void CreateSocket(Uri targetEndpoint)
            {
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;

                connectionTask = m_socket.ConnectAsync(targetEndpoint, token);
            }

            public async void SendToSocket(string jsonData)
            {
                if (!connectionTask.IsCompleted)
                {
                    await connectionTask;
                }

                if (m_socket.State == WebSocketState.Open)
                {
                    CancellationTokenSource source = new CancellationTokenSource();
                    CancellationToken token = source.Token;

                    var utf8Array = Encoding.UTF8.GetBytes(jsonData).AsMemory();


                    Console.WriteLine($"Sending to Hub {jsonData}");
                    await m_socket.SendAsync(utf8Array, WebSocketMessageType.Text, true, token);
                }
            }
        }
    }
}
