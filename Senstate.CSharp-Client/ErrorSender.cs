using System;
using System.Runtime.CompilerServices;

namespace Senstate.CSharp_Client
{
 
    public class ErrorSender
    {
        public static void Send(Exception exception, 
            [CallerMemberName] string methodName = "",
            [CallerLineNumber] int line = -1)
        {
            var message = exception.Message;
            var errorName = exception.GetType().Name;
            var stack = exception.StackTrace;

            SenstateContext.SendEventData(SenstateEventConstants.ErrorEvent, new { 
               message,
                errorName,
                methodName,
                line,
                stack
            });
        }
    }

}
