using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace Senstate.CSharp_Client.Tests
{
    [TestClass]
    public class WatcherTests
    {
        [TestMethod]
        public void Sends_WatcherMeta_And_DataEvents()
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
    }
}
