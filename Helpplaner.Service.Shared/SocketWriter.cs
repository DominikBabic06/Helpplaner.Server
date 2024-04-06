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
                Message Message = new Message();
                byte[] buffer = new byte[1024];
             
                Message.Type = message.GetType().ToString();
                Message.Content = message;

                buffer = JsonSerializer.SerializeToUtf8Bytes(Message);

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
                Message Message = new Message();

                byte[] buffer = new byte[10000];
                Message = new Message();
                Message.Type = obj.GetType().ToString();
                Message.Content = JsonSerializer.Serialize(obj);

                buffer = JsonSerializer.SerializeToUtf8Bytes(Message);    
             
                socket.Send(buffer);    

                
            }
            catch (Exception e)
            {

                logger.Log(e.Message, "red");
            }   
        }
    }
}
