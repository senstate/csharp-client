using System;
using System.Collections.Generic;
using System.Text;

namespace Senstate.CSharp_Client
{

    public static class SenstateEventConstants
    {
        public static string AddApp {get;set;} = "addApp";
        public static string AddWatcher {get;set;} = "addWatcher";
        public static string InputEvent {get;set;} = "inputEvent";
        public static string LogEvent {get;set;} = "inputLogEvent";
        public static string ErrorEvent {get;set;} = "inputErrorEvent";
    }

    public static class SenstateContext
    {
        public static string AppName { get; set; }
        public static string AppId { get; set; } = Guid.NewGuid().ToString();


        public static ISenstateJson SerializerInstance { get; set; }
        public static ISenstateWebSocket WebSocketInstance { get; set; }

        public static void RegisterApp(Uri targetEndpoint = null)
        {
            if (targetEndpoint == null)
            {
                targetEndpoint = new Uri("ws://localhost:3333");
            }

            WebSocketInstance.CreateSocket(targetEndpoint);

            SendEventData(SenstateEventConstants.AddApp, new
            {
                appId = AppId,
                name = AppName
            });
        }

        public static void SendEventData(string eventType, object eventData)
        {
            var eventToSend = CreateEventJson(eventType, eventData);

            WebSocketInstance.SendToSocket(eventToSend);
        }

        public static string CreateEventJson(string eventType, object eventData)
        {
            var objectToSend = new Dictionary<string, object>
            {
                {"event", eventType },
        {"data", eventData}
            };

            return SerializerInstance.ConvertToString(objectToSend);
        }
    }

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
