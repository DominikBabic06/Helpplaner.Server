using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace Helpplaner.Service.Shared
{
    public class SocketWriter
    {
        Socket socket;
        IServiceLogger logger;
        BinaryFormatter formatter = new BinaryFormatter();


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
               MemoryStream stream = new MemoryStream();    
                byte[] buffer = new byte[1024];
                formatter.Serialize(stream, obj);   
                buffer = Encoding.UTF8.GetBytes(obj.ToString());
                socket.Send(buffer);
            }
            catch (Exception e)
            {

                logger.Log(e.Message, "red");
            }   
        }
    }
}
