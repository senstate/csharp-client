using Senstate.CSharp_Client;
using Senstate.NetStandard;
using System;
using System.Threading;

namespace Senstate_ExampleApp
{
   class Program
   {
      static void Main(string[] args)
      {
         var webSocket = new NetStandardWebSocketImplementation();
         webSocket.ExceptionThrown += (sender, e) =>
         {
            throw e.Exception;
         };

         SenstateContext.SerializerInstance = new NetStandardJsonNetImplementation();
         SenstateContext.WebSocketInstance = webSocket;
         SenstateContext.RegisterApp("C# Console Log");

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
         catch (Exception ex)
         {
            ErrorSender.Send(ex);
         }

         for (var i = 0; i < 10000; i++)
         {

            Thread.Sleep(500);
            stringWatcher.SendData($"This an example Text {i}");
            numberWatcher.SendData(i);

            var someObject = new
            {
               example = true,
               sub = new
               {
                  data = i
               }
            };

            Logger.SendLog(LoggerType.Debug, $"Debug {i}", someObject);
            Logger.SendLog(LoggerType.Info, $"Info {i}");

            objectWatcher.SendData(someObject);
         }

         Console.ReadKey();
      }
   }
}
