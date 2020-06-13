using Moq;
using Senstate.NetStandard;

namespace Senstate.CSharp_Client.Tests
{
    public class TestContext
    {
        public static Mock<ISenstateWebSocket> RegisterApp ()
        {
            var webSocketMock = new Mock<ISenstateWebSocket>();

            SenstateContext.AppId = "1234";

            SenstateContext.SerializerInstance = new NetStandardJsonNetImplementation();
            SenstateContext.WebSocketInstance = webSocketMock.Object;

            SenstateContext.RegisterApp("Some Name");

            return webSocketMock;
        }
    }
}
