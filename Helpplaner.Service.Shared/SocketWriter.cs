using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace Helpplaner.Service.Shared
{
    public class SocketWriter
    {
        Socket socket;
        IServiceLogger logger;
     


        public SocketWriter(Socket socket, IServiceLogger logger)
        {
            this.logger = logger;
            this.socket = socket;


        }

        public void Send(string message)
        {
            try
            {
                byte[] buffer = new byte[1024];
                buffer = Encoding.UTF8.GetBytes(message);
                socket.Send(buffer);
            }
            catch (Exception e)
            {

                logger.Log(e.Message, "red");
            }


        }
        public void SendObject(object obj)
        {
            try
            {
               
                byte[] buffer = new byte[1024];
                string json = JsonSerializer.Serialize(obj);    
               buffer = Encoding.UTF8.GetBytes(json);   
                socket.Send(buffer);    

                
            }
            catch (Exception e)
            {

                logger.Log(e.Message, "red");
            }   
        }
    }
}
