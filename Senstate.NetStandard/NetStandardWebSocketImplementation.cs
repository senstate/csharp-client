using Senstate.CSharp_Client;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Senstate.NetStandard
{
   public class NetStandardWebSocketImplementation : ISenstateWebSocket
   {
      public event EventHandler<ExceptionEventArgs> ExceptionThrown;

      ClientWebSocket m_socket = new ClientWebSocket();
      private Task connectionTask = null;
      
      public NetStandardWebSocketImplementation()
      {
      }

      public void CreateSocket(Uri targetEndpoint)
      {
         CancellationTokenSource source = new CancellationTokenSource();
         CancellationToken token = source.Token;

         connectionTask = m_socket.ConnectAsync(targetEndpoint, token);
      }

      public async void SendToSocket(string jsonData)
      {
         try
         {
            if (!connectionTask.IsCompleted)
            {
               await connectionTask;
            }

            if (m_socket.State == WebSocketState.Open)
            {
               CancellationTokenSource source = new CancellationTokenSource();
               CancellationToken token = source.Token;

               var utf8Array = new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonData));

               await m_socket.SendAsync(utf8Array, WebSocketMessageType.Text, true, token);
            }
         }
         catch (Exception ex)
         {
            if (ExceptionThrown != null)
            {
               ExceptionThrown.Invoke(this, new ExceptionEventArgs
               {
                  Exception = ex
               });
            }
         }
      }
   }

   public class ExceptionEventArgs : EventArgs
   {
      public Exception Exception { get; set; }
   }
}
