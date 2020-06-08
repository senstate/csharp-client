using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace Senstate.CSharp_Client.Tests
{
    [TestClass]
    public class WatcherTests
    {
        [TestMethod]
        public void Sends_WatcherMeta_And_StringDataEvents()
        {
            // TODO Extract ?
            var webSocketMock = new Mock<ISenstateWebSocket>();

            SenstateContext.AppId = "1234";
            SenstateContext.AppName = "Some Name";

            SenstateContext.SerializerInstance = new DummySerializer();
            SenstateContext.WebSocketInstance = webSocketMock.Object;

            SenstateContext.RegisterApp();

            var watchMeta = new WatcherMeta
            {
                Tag = "Some Label",
                Type = WatcherType.String
            };


            var watcher = new Watcher(watchMeta);
            watcher.SendData("Some Data 1");
            watcher.SendData("Some Data 2");


            webSocketMock.Verify(w => w.SendToSocket(It.IsAny<string>()), Times.Exactly(4));
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.AddApp))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.AddWatcher))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.InputEvent))), Times.Exactly(2));

            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(watchMeta.Tag))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains($"\"type\":{(int)watchMeta.Type}"))), Times.Once);


            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains("Some Data"))), Times.Exactly(2));

        }

        [TestMethod]
        public void Sends_WatcherMeta_And_NumberDataEvents()
        {
            // TODO Extract ? #2
            var webSocketMock = new Mock<ISenstateWebSocket>();

            SenstateContext.AppId = "1234";
            SenstateContext.AppName = "Some Name";

            SenstateContext.SerializerInstance = new DummySerializer();
            SenstateContext.WebSocketInstance = webSocketMock.Object;

            SenstateContext.RegisterApp();

            var watchMeta = new WatcherMeta
            {
                Tag = "Some Other Label",
                Type = WatcherType.Number
            };

            var watcher = new Watcher(watchMeta);
            watcher.SendData(42);
            watcher.SendData(1337);


            webSocketMock.Verify(w => w.SendToSocket(It.IsAny<string>()), Times.Exactly(4));
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.AddApp))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.AddWatcher))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.InputEvent))), Times.Exactly(2));

            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(watchMeta.Tag))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains($"\"type\":{(int)watchMeta.Type}"))), Times.Once);


            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains("\"data\":42"))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains("\"data\":1337"))), Times.Once);

        }


        [TestMethod]
        public void Sends_WatcherMeta_And_JsonDataEvents()
        {
            // TODO Extract ? #3
            var webSocketMock = new Mock<ISenstateWebSocket>();

            SenstateContext.AppId = "1234";
            SenstateContext.AppName = "Some Name";

            SenstateContext.SerializerInstance = new DummySerializer();
            SenstateContext.WebSocketInstance = webSocketMock.Object;

            SenstateContext.RegisterApp();

            var watchMeta = new WatcherMeta
            {
                Tag = "Some Other Label",
                Type = WatcherType.Number
            };

            var myCustomObject = new
            {
                stuff = true,
                life = 42,
                sub = new
                {
                    just = "a string"
                }
            };

            var myCustomObjectJson = SenstateContext.SerializerInstance.ConvertToString(myCustomObject);

            var watcher = new Watcher(watchMeta);
            watcher.SendData(myCustomObject);


            webSocketMock.Verify(w => w.SendToSocket(It.IsAny<string>()), Times.Exactly(3));
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.AddApp))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.AddWatcher))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.InputEvent))), Times.Once);

            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(watchMeta.Tag))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains($"\"type\":{(int)watchMeta.Type}"))), Times.Once);


            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(myCustomObjectJson))), Times.Once);

        }
    }
}
