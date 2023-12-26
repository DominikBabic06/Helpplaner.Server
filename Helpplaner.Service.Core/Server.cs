namespace Helpplaner.Service.Core
{
    using System.Net;
    using System.Net.Sockets;
    using Helpplaner.Service.Shared;
    public class Server
    {
        Socket _socket;  
        IPEndPoint _endPoint;    
        IPAddress _ipAddress;   
        int _port;  
        IServiceLogger _logger;
        ClientHandler clientHandler;
    public    bool isRunning = false;  
        public Server(IServiceLogger logger , string ipAddress = "127.0.0.1", int port = 50000 )
        {
            _ipAddress = IPAddress.Parse(ipAddress);
            _port = port;
            _endPoint = new IPEndPoint(_ipAddress, _port);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _logger = logger;
        }  
        
        public void Start()
        {
            _socket.Bind(_endPoint);
            _socket.Listen(100);
            _logger.Log("Server started", "green"); 
            _logger.Log($"Listening on {_ipAddress}:{_port}", "green");
            isRunning = true;
           clientHandler = new ClientHandler(_socket, _logger);  
            Thread thread = new Thread(clientHandler.AcceptClients);    
            thread.IsBackground = true; 
            thread.Start();
            

        }
        public void Stop() {
            if (_socket.Connected)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
          
                _logger.Log("Server stopped", "red");   
            }
          


        }
        public void Restart()
        {
            Stop();
            Start();
        }   

       
    }
}