using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Senstate.CSharp_Client.Tests
{
    [TestClass]
    public class SenstateContextTests
    {
        [TestMethod]
        public void Sends_AddApp_Event()
        {
            var webSocketMock = TestContext.RegisterApp();

            webSocketMock.Verify(w => w.SendToSocket(It.IsAny<string>()), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.AddApp))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateContext.AppId))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateContext.AppName))), Times.Once);
        }
    }
}
