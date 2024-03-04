using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;   
using Helpplaner.Service.Shared;  
using Helpplaner.Service.Objects;
using Helpplaner.Service.SqlHandling;   
using System.Data.SqlClient;    

namespace Helpplaner.Service.Core
{
    internal class ClientHandler
    {
        Socket _socket;
      
        IServiceLogger Logger; 
        Dictionary<Guid, SessionHandler> _sessions = new Dictionary<Guid, SessionHandler>();
      

      public  ClientHandler(Socket socket, IServiceLogger logger)
        {
            _socket = socket;
            Logger = logger;
          
          
        } 
        
        public void AcceptClients()
        {
            try
            {
                while (true)
                {
                    Socket client = _socket.Accept();
                    Logger.Log($"Client connected: {client.RemoteEndPoint}", "green");
                    SessionHandler session = new SessionHandler(client, Logger, GetFirstAvailableSessionId());
                    session.SessionClosed += SessionHandler;
                    session.TriggererServerMessage += Session_TriggererServerMessage;
                    Thread sessionTask = new Thread(session.HandleClient);
                    sessionTask.IsBackground = true;
                    sessionTask.Start();
                    _sessions.Add(session._sessionId, session);

                }
            }
            catch (Exception)
            {

                Logger.Log("Server stopped", "red");    
            }
           
           
        }

        private void Session_TriggererServerMessage(object? sender, string e)
        {
            PostMessage(e);

        }

        public void PostMessage(string message)
        {
           
            foreach (Guid id in _sessions.Keys)
            {
                Logger.Log($"Sending message to session {id}", "green");
                _sessions[id].PostMessage(message);
            }
        }
        public void Close()
        {
        
          CloseAllSessions();   

        }   


        

        public void CloseAllSessions()
        {
            Logger.Log("Closing all sessions", "red");
            foreach (Guid id in _sessions.Keys)
            {
            
                _sessions.Remove(id);
        

            }
            Logger.Log("All sessions closed", "red");
        }
        public void SessionHandler(object sender, EventArgs args)
        {
          
            SessionHandler session = (SessionHandler)sender;
            
            _sessions.Remove(session._sessionId);
            Logger.Log("Session closed", "red");
        }
    
        public Guid GetFirstAvailableSessionId()
        {
            Guid sessionId = Guid.NewGuid();
            while (_sessions.ContainsKey(sessionId))
            {
                sessionId = Guid.NewGuid();
            }
            return sessionId;
        }   
    } 
}
