# :tada: Senstate C#-Client 👋


## Install C# Client

Supported Platforms:
- NetStandard
- Blazor Server/WASM (Demo soon)

Visit: [Releases](https://github.com/senstate/csharp-client/releases) to download the Client & NetStandard Dlls.

Reference them as usual

## Connect to Senstate


```csharp
   var webSocket = new NetStandardWebSocketImplementation();
   webSocket.ExceptionThrown += (sender, e) =>  // Optional if you want to catch Connection issues
   {
      throw e.Exception;  
   };

   SenstateContext.SerializerInstance = new NetStandardJsonNetImplementation();
   SenstateContext.WebSocketInstance = webSocket;
   SenstateContext.RegisterApp("Your Name");
```


## Sending Data

You can send strings / numbers / (json) objects (see `WatcherType` Enum)


Create your Watcher:
```csharp
   var stringWatcher = new Watcher(
      new WatcherMeta
      {
         Tag = "Some Label", 
         Type = WatcherType.String, // or Number / Json
         Group = "Example Group 1"  // optional: if you want to group watchers
      }
   );
```

Send your data:
```csharp
  stringWatcher.SendData($"This an example Text {i}");
```

Once there is a connection, the Data will be shown in the Dashboard.


Addtional Example to send objects:


Create your Watcher:
```csharp
   var objectWatcher = new Watcher(
      new WatcherMeta
      {
         Tag = "Object",  // Just a label for this watcher
         Type = WatcherType.Json,
         Group = "Special"// optional:
      }
   );
```

Send your data:
```csharp
   var someObject = new
   {
      example = true,
      sub = new
      {
         data = i
      }
   };

   // Any objects, but they can't have cyclic references
   // So that JsonNet can serialize those
   objectWatcher.SendData(someObject);
```

## Sending Logs

Possible Log-Levels:
- Debug
- Info
- Warn
- Error

```csharp
   Logger.SendLog(LoggerType.Debug, $"Debug {i}", someObject);  // optional: someObject
   Logger.SendLog(LoggerType.Info, $"Info {i}");
```

## Sending Errors

```csharp
   ErrorSender.Send(ex);
```


# Show your support

Give a ⭐️ if this project helped you!

If you have features / ideas, open an issue :)
