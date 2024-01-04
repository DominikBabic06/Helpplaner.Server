using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;   
using Helpplaner.Service.Shared;
using System.Data.SqlClient;
using Helpplaner.Service.SqlHandling;

namespace Helpplaner.Service.Core
{
    internal class SessionHandler
    {
        Socket _clientSocket;
        IServiceLogger _logger;
       public Guid _sessionId;
       SocketWriter writer;
        SocketReader reader;
        SqlConnection _connection;  
        InsertSqlCommandHandler _insertSqlCommandHandler;   
        SelectSqlCommandHandler _selectSqlCommandHandler;   

      public  SessionHandler(Socket client, IServiceLogger logger,Guid id)
        {
            _clientSocket = client; 
            _sessionId = id;
            writer = new SocketWriter(_clientSocket, _logger);  
            reader = new SocketReader(_clientSocket, _logger);
           _insertSqlCommandHandler = new InsertSqlCommandHandler(_connection, _logger);   
            _selectSqlCommandHandler = new SelectSqlCommandHandler(_connection, _logger);   
                _logger = logger;
                _connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=HELPPLANER;Integrated Security=True");   
           
        }   
        public void HandleClient()
        {
            
            string text= "";
            while (text != "exit" )
            {
                _connection.Open();
                text = reader.Read();    
                 
                _logger.Log($"{_sessionId}Text received: {text}", "yellow");
                if (text == null)
                {
                    break;  
                }
                writer.Send(text);  
                _logger.Log($"{_sessionId}Text sent: {text}", "blue");
                _connection.Close();
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
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
           
            if (SessionClosed != null) //required in C# to ensure a handler is attached
                SessionClosed(this, EventArgs.Empty);

        }
    }
}
