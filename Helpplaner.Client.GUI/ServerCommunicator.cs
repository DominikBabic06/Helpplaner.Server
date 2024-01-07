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

        public ServerCommunicator(IServiceLogger logger)
        {
            sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sk.Connect(new IPEndPoint(IPAddress.Loopback, 50000));
            _writer = new SocketWriter(sk, logger);
            _reader = new SocketReader(sk, logger  );


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
                proj = _reader.ReadObject();
                _writer.Send("check");
                projects.Add((Project)proj);
            }
            catch (Exception ex)
            {

                input = (string)proj;
            }




            return projects.ToArray();
        }
        public void ReceiveAsyncMessage()
        {

        }
    }
}
