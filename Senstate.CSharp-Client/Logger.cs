namespace Senstate.CSharp_Client
{
    public enum LoggerType
    {
        Debug = 0, 
        Info, 
        Warn,
        Error
    }

   

    /// <summary>
    /// Probably the Handler to Register / Send Data
    /// 
    /// TODO? make it generic? 
    /// </summary>
    public class Logger
    {
        public static void SendLog(LoggerType logLevel, string log)
        {
            SenstateContext.SendEventData(SenstateEventConstants.LogEvent, new { 
               log,
               logLevel
            });
        }
    }

}
