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
            byte[] buffer = new byte[65535];
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
        public event EventHandler<string> ServerMEssage;
        public object ReadObject()
        {
            Message message;

            string re;
            byte[] buffer = new byte[65535];
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

                    case "Helpplaner.Service.Objects.User":
                        User user = JsonSerializer.Deserialize<User>(message.Content);
                        return user;
                        break;
                    case "Helpplaner.Service.Objects.WorkPackage":
                        Helpplaner.Service.Objects.WorkPackage task = JsonSerializer.Deserialize<Helpplaner.Service.Objects.WorkPackage>(message.Content);
                        return task;
                        break; 
                    case "Helpplaner.Service.Objects.WorkPackage[]":
                        Helpplaner.Service.Objects.WorkPackage[] tasks = JsonSerializer.Deserialize<Helpplaner.Service.Objects.WorkPackage[]>(message.Content);
                        return tasks;
                        break;
                    case "Helpplaner.Service.Objects.User[]":
                        Helpplaner.Service.Objects.User[] users = JsonSerializer.Deserialize<Helpplaner.Service.Objects.User[]>(message.Content);
                        return users;
                        break;
                    case "Helpplaner.Service.Objects.Project[]":
                        Helpplaner.Service.Objects.Project[] projects = JsonSerializer.Deserialize<Helpplaner.Service.Objects.Project[]>(message.Content);
                        return projects;
                        break;




                    case "SERVERAsync":

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



        public virtual void OnUserfound(string Message)
        {
            if (ServerMEssage != null)
            {
                ServerMEssage(this, Message);
            }

        }
    }
}
