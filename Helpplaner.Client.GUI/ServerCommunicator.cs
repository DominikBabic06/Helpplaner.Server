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
        private bool skipconnection = false;
        bool isconnected = false;   
       bool needsToBeReloaded = false;  
        int projetidthatneedsreloading = 0; 

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
            if (!skipconnection)
            {
                if (isconnected)
                {
                    try
                    {
                        _writer.Send("info");
                        string input = _reader.Read();
                   
                        if (input == "None")
                        {
                            return true;
                        }
                        if(input == "Shutdown")
                        {
                            _writer.Send("exit");
                            sk.Shutdown(SocketShutdown.Both);
                            sk.Disconnect(true);
                            Thread.Sleep(5000);
                            isconnected = false;
                            return false;
                        }
                        if (input.StartsWith("tr;"))
                        {

                            needsToBeReloaded = true;   
                            projetidthatneedsreloading = int.Parse(input.Remove(0, 3));  
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
                       sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);    
                        _writer = new SocketWriter(sk, new ConsoleLogger());    
                        _reader = new SocketReader(sk, new ConsoleLogger());    

                        sk.Connect(new IPEndPoint(IPAddress.Loopback, 50000));
                        isconnected = true;
                        return true;
                    }
                    catch (Exception)
                    {
                    //  sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        return false;
                    }
                }
            }
            return true;
         
           
        }


        public bool NeedsToBeReloaded(int CurrentProjectId)
        {
            if(needsToBeReloaded)
            { 
               if (CurrentProjectId == projetidthatneedsreloading)
                 {
                projetidthatneedsreloading = 0;
                    needsToBeReloaded = false;  
                return true;
                  }
            }
            return false;   
        }   
        public User[] GetUsersforProject(int id)
        {
            skipconnection = true;  
            List<User> users = new List<User>();
            string input = "";
            _writer.Send("getusersforproject;" + id);

            Object user = null;
            try
            {
                do
                {
                    user = _reader.ReadObject();
                    _writer.Send("done");
                    users.Add((User)user);
                } while (true);
            }
            catch (Exception ex)
            {

                input = (string)user;
            }

            skipconnection = false;
            return users.ToArray();
        }


        public Helpplaner.Service.Objects.WorkPackage[] GetTasksforProject(int id)
        {
            skipconnection = true;
            List<Helpplaner.Service.Objects.WorkPackage> tasks = new List<Helpplaner.Service.Objects.WorkPackage>();
            string input = "";
            _writer.Send("getalltasks;" + id);   
            Object task = null;
            try
            {
                do
                {
                    task = _reader.ReadObject();
                    _writer.Send("done");
                    tasks.Add((Helpplaner.Service.Objects.WorkPackage)task);
                } while (true);
            }
            catch (Exception ex)
            {

                input = (string)task;
            }
            

            skipconnection = false;
            return tasks.ToArray();

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
            skipconnection = true;
            Thread.Sleep(1000);
            _writer.Send("login;" + username + ";" + password);
          
            string input = _reader.Read();
   
            if (input == "done")
            {
               
                User user = (User)_reader.ReadObject();
                skipconnection = false;
                return user;
            }
            else
              
            {
                skipconnection = false;
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


        public void AddTaskToProject(Helpplaner.Service.Objects.WorkPackage task, int projectid)
        {
        
            _writer.Send("addTask;" + projectid);
            if (_reader.Read() == "ok")
            {
                _writer.SendObject(task);
            }   

            _reader.Read(); 


        }
    }
}
