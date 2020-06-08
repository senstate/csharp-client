using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace Senstate.CSharp_Client.Tests
{
    [TestClass]
    public class SenstateContextTests
    {
        [TestMethod]
        public void Sends_AddApp_Event()
        {
            var webSocketMock = new Mock<ISenstateWebSocket>();

            SenstateContext.AppId = "1234";
            SenstateContext.AppName = "Some Name";

            SenstateContext.SerializerInstance = new DummySerializer();
            SenstateContext.WebSocketInstance = webSocketMock.Object;

            SenstateContext.RegisterApp();

            webSocketMock.Verify(w => w.SendToSocket(It.IsAny<string>()), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.AppApp))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateContext.AppId))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateContext.AppName))), Times.Once);
        }
    }

    public class DummySerializer : ISenstateJson
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
