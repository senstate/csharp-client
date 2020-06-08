using System;
using System.Collections.Generic;
using System.Text;

namespace Senstate.CSharp_Client
{

    public static class SenstateEventConstants
    {
        public static string AppApp {get;set;} = "addApp";
        public static string AppWatcher {get;set;} = "addWatcher";
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

        public static void RegisterApp()
        {
            WebSocketInstance.SendToSocket(CreateEventJson(SenstateEventConstants.AppApp, new {
                appId = AppId,
                name = AppName
            }));
        }

        public static string CreateEventJson(string eventType, object eventData)
        {
            var jsonedData = SerializerInstance.ConvertToString(eventData);

            var objectToSend = new Dictionary<string, string>
            {
                {"event", eventType },
        {"data", jsonedData}
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
        void SendToSocket(string jsonData);
    }
}
