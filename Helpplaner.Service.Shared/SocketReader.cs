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
            int Lenght = 0;
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
                    case "System.String":
                        return message.Content;
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
                        List<WorkPackage> workPackages = new List<WorkPackage>();
                        Lenght = int.Parse(message.Content);
                        socket.Send(Encoding.UTF8.GetBytes("ok"));
                        for (int i = 0; i < Lenght; i++)
                        {
                            socket.Receive(buffer);
                            string obje = Encoding.UTF8.GetString(buffer);
                            obje = obje.Trim('\0');
                            workPackages.Add(JsonSerializer.Deserialize<WorkPackage>(obje));
                            socket.Send(Encoding.UTF8.GetBytes("ok"));
                            buffer = new byte[65535];
                        }
                        return workPackages.ToArray();
                        break;

                    case "Helpplaner.Service.Objects.WorkSession[]":
                        List<WorkSession> workSessions = new List<WorkSession>();
                        Lenght = int.Parse(message.Content);
                        socket.Send(Encoding.UTF8.GetBytes("ok"));
                        for (int i = 0; i < Lenght; i++)
                        {
                            socket.Receive(buffer);
                            string obje = Encoding.UTF8.GetString(buffer);
                            obje = obje.Trim('\0');
                            workSessions.Add(JsonSerializer.Deserialize<WorkSession>(obje));
                            socket.Send(Encoding.UTF8.GetBytes("ok"));
                            buffer = new byte[65535];
                        }
                        return workSessions.ToArray();
                        break;
                    case "System.String[]":
                        List<string> strings = new List<string>();
                        Lenght = int.Parse(message.Content);
                        socket.Send(Encoding.UTF8.GetBytes("ok"));
                        for (int i = 0; i < Lenght; i++)
                        {
                            socket.Receive(buffer);
                            string obje = Encoding.UTF8.GetString(buffer);
                            obje = obje.Trim('\0');
                            strings.Add(obje);
                            socket.Send(Encoding.UTF8.GetBytes("ok"));
                            buffer = new byte[65535];
                        }
                        return strings.ToArray();
                        break;
                    case "Helpplaner.Service.Objects.User[]":
                        List<User> users = new List<User>();    
                         Lenght = int.Parse(message.Content);
                        socket.Send(Encoding.UTF8.GetBytes("ok"));
                        for (int i = 0; i < Lenght; i++)
                        {
                            socket.Receive(buffer);
                            string obje = Encoding.UTF8.GetString(buffer);
                            obje = obje.Trim('\0');
                            users.Add(JsonSerializer.Deserialize<User>(obje));
                            socket.Send(Encoding.UTF8.GetBytes("ok"));
                            buffer = new byte[65535];

                        }
                        return users.ToArray();
                   
                    case "Helpplaner.Service.Objects.Project[]":
                         Lenght = int.Parse(message.Content);
                        List<Project> projects = new List<Project>();   
                        socket.Send(Encoding.UTF8.GetBytes("ok"));
                        for (int i = 0; i < Lenght; i++)
                        {
                           socket.Receive(buffer);
                            string obje  = Encoding.UTF8.GetString(buffer);
                            obje =   obje.Trim('\0');
                            projects.Add(JsonSerializer.Deserialize<Project>(obje));
                            socket.Send(Encoding.UTF8.GetBytes("ok"));
                            buffer = new byte[65535];
                        }
                     
                


                        return projects.ToArray();
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
