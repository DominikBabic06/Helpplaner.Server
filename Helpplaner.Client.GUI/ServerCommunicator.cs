using Helpplaner.Service.Objects;
using Helpplaner.Service.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;



namespace Helpplaner.Client.GUI
{
   public class ServerCommunicator
    {
        private SocketWriter _writer;
        private SocketReader _reader;
        Socket sk;
        Thread thread;  
            

       public  event EventHandler<string> ServerMessage;
        public ServerCommunicator(IServiceLogger logger)
        {
            sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
          
            _writer = new SocketWriter(sk, logger);
            _reader = new SocketReader(sk, logger  );
            _reader.ServerMEssage += ReceiveAsyncMessage;
           


        }
        public bool tryConnect()
        {
            if (sk.Connected)
            {
                try
                {
                    _writer.Send("ping");
                    if (_reader.Read() == "pong")
                    {
                        return true;
                    }

                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {

                    return false;
                }
               
            }
            else
            {
                try
                {
                    sk.Connect(new IPEndPoint(IPAddress.Loopback, 50000));
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
           
        }


        public string Send(string message)
        {
            _writer.Send(message);
            return _reader.Read();
        }
        public string Receive(string message)
        {
            return _reader.Read();
        }

        public User TryLogin(string username, string password)
        {

            _writer.Send(username + ";" + password);
          
            string input = _reader.Read();
   
            if (input == "done")
            {
               
                User user = (User)_reader.ReadObject();
                return user;
            }
            else
            {
                return null;
            }
        }   
        public Project[] GetProjectsforUser()
        {
            List<Project> projects = new List<Project>();
            string input = "";
            _writer.Send("getallprojects");

            Object proj = null;
            try
            {
                do
                {
                    proj = _reader.ReadObject();
                    _writer.Send("done");
                    projects.Add((Project)proj);
                } while (true);
               

               
            }
            catch (Exception ex)
            {

                input = (string)proj;
            }




            return projects.ToArray();
        }
        public void Logout()
        {
            _writer.Send("logout");
            _reader.Read();
        }
         
        public Project[] GetAdminProjekts()
        {
            List<Project> projects = new List<Project>();
            string input = "";
            _writer.Send("getadminprojects");

            Object proj = null;
            try
            {
                do
                {
                    proj = _reader.ReadObject();
                    _writer.Send("done");
                    projects.Add((Project)proj);
                } while (true);
            }
            catch (Exception ex)
            {

                input = (string)proj;
            }


            return projects.ToArray();
        }

        public void ReceiveAsyncMessage(object sender, string e)
        {
            if (ServerMessage != null)
            {
                ServerMessage(sender, e);
            }
        }   
    }
}
