using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace Senstate.CSharp_Client.Tests
{
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public void Sends_DebugLogEvent()
        {
            var webSocketMock = TestContext.RegisterApp();

            Logger.SendLog(LoggerType.Debug, "Some Debug Message, dont mind me");

            webSocketMock.Verify(w => w.SendToSocket(It.IsAny<string>()), Times.Exactly(2));
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.AddApp))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.LogEvent))), Times.Once);

            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains($"\"logLevel\":{(int)LoggerType.Debug}"))), Times.Once);


            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains("Some Debug Message, dont mind me"))), Times.Once);
        }

        [TestMethod]
        public void Sends_WarnLogEvent()
        {
            var webSocketMock = TestContext.RegisterApp();

            Logger.SendLog(LoggerType.Warn, "Uh-Oh Warning, mind me");

            webSocketMock.Verify(w => w.SendToSocket(It.IsAny<string>()), Times.Exactly(2));
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.AddApp))), Times.Once);
            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains(SenstateEventConstants.LogEvent))), Times.Once);

            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains($"\"logLevel\":{(int)LoggerType.Warn}"))), Times.Once);


            webSocketMock.Verify(w => w.SendToSocket(It.Is<string>(s => s.Contains("Uh-Oh Warning, mind me"))), Times.Once);
        }
    }
}
