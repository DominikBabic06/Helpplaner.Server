using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

                return re;

            }
            catch (Exception)
            {

                return "exit";
            }

        }
    }
}
