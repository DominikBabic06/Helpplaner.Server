using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json; 
using Helpplaner.Service.Objects;



namespace Helpplaner.Service.Shared
{
    public class SocketReader
    {
        Socket socket;

        IServiceLogger logger;
        public SocketReader(Socket sock, IServiceLogger log)
        {
            socket = sock;
            logger = log;

        }


        public string Read()
        {
            Message message;

            string re;
            byte[] buffer = new byte[1024];
            try
            {
                socket.Receive(buffer);
                if (buffer.Length == 0)
                {
                    return "exit";
                }
                re = Encoding.UTF8.GetString(buffer);
                re = re.Trim('\0');
                message = JsonSerializer.Deserialize<Message>(re);
                string re2 = message.Content;

                return re2;

            }
            catch (Exception)
            {

                return "exit";
            }

        }
        public object ReadObject()
        {
            Message message;

            string re;
            byte[] buffer = new byte[1024];
            try
            {
                socket.Receive(buffer);
                if (buffer.Length == 0)
                {
                    return "exit";
                }
                re = Encoding.UTF8.GetString(buffer);
                re = re.Trim('\0');
                message = JsonSerializer.Deserialize<Message>(re);
                switch (message.Type)
                {
                    case "Helpplaner.Service.Objects.Project":
                        Project project = JsonSerializer.Deserialize<Project>(message.Content);
                        return project;
                        break;
                    default:
                        break;
                }
                string re2 = message.Content;

                return re2;

            }
            catch (Exception)
            {

                return "exit";
            }

        }
    }
}
