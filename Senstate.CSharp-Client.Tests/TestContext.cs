using Moq;

namespace Senstate.CSharp_Client.Tests
{
    public class TestContext
    {
        public static Mock<ISenstateWebSocket> RegisterApp ()
        {
            var webSocketMock = new Mock<ISenstateWebSocket>();

            SenstateContext.AppId = "1234";
            SenstateContext.AppName = "Some Name";

            SenstateContext.SerializerInstance = new DummySerializer();
            SenstateContext.WebSocketInstance = webSocketMock.Object;

            SenstateContext.RegisterApp();

            return webSocketMock;
        }
    }
}
