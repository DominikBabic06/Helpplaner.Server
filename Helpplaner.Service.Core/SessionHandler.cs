using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;   
using Helpplaner.Service.Shared;    

namespace Helpplaner.Service.Core
{
    internal class SessionHandler
    {
        Socket _clientSocket;
        IServiceLogger _logger;
       public Guid _sessionId;
       SocketWriter writer;
        SocketReader reader;
      public  SessionHandler(Socket client, IServiceLogger logger,Guid id)
        {
            _clientSocket = client; 
            _logger = logger;   
            _sessionId = id;
            writer = new SocketWriter(_clientSocket, _logger);  
            reader = new SocketReader(_clientSocket, _logger);
           
        }   
        public void HandleClient()
        {
            string text= "";
            while (text != "exit" )
            {
                text = reader.Read();    
                 
                _logger.Log($"{_sessionId}Text received: {text}", "yellow");
                if (text == null)
                {
                    break;  
                }
                writer.Send(text);  
                _logger.Log($"{_sessionId}Text sent: {text}", "blue");

            }
            Close();    
         
        }

        public event EventHandler SessionClosed;    
        public void Close()
        {
            if(_clientSocket.Connected)
            {
                _clientSocket.Shutdown(SocketShutdown.Both);
                _clientSocket.Close();
         
            }
            if (SessionClosed != null) //required in C# to ensure a handler is attached
                SessionClosed(this, EventArgs.Empty);

        }
    }
}
