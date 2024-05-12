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
       public  bool ProjectsneedtobeReloaded = false;    
 
       public bool needLogout = false;   

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
            lock(this) {
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
                                _writer.Send("logout;");
                            _writer.Send("exit");
                            sk.Shutdown(SocketShutdown.Both);
                            sk.Disconnect(true);
                            Thread.Sleep(5000);
                            isconnected = false;
                            return false;
                        }
                        if(input == "Logout")
                            {
                            
                            needLogout = true;   
                           
                            return true;
                        }
                        if (input.StartsWith("tr;"))
                        {

                            needsToBeReloaded = true;   
                            projetidthatneedsreloading = int.Parse(input.Remove(0, 3));  
                            return true;
                        }
                        if(input == "ReloadProjects")
                            {
                                ProjectsneedtobeReloaded = true;    
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
            }
            return true;
         
           
        }

        public void AddUserToProject(int projectid, int userid)
        {
            lock(this)
            {
            _writer.Send("addusertoproject;" + projectid + ";" + userid);
            _reader.Read();
            }
        }   
        public void RemoveUserFromProject(int projectid, int userid)
        {
            lock(this)
            {
            _writer.Send("removeuserfromproject;" + projectid + ";" + userid);
            _reader.Read();
            }
        }


        public User[] GiveAllUser()
        {
            lock(this)
            {
            _writer.Send("GiveAllUsers");
            Object user = null;
            user = _reader.ReadObject();
            return (User[])user;
            }
        }
        public void AddProject(Project proj)
        {
            lock(this)
            {
                _writer.Send("CreateProjekt;");
                _reader.Read(); 
                _writer.SendObject(proj);
                _reader.Read();

            }
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
            lock(this)
            { 
            skipconnection = true;  
            List<User> users = new List<User>();
            string input = "";
            _writer.Send("getusersforproject;" + id);
            Object user = null;
            user =  _reader.ReadObject();
            users.AddRange((User[])user);   
            skipconnection = false;
            return users.ToArray();
            }
        }


        public Helpplaner.Service.Objects.WorkPackage[] GetTasksforProject(int id)
        {
            lock (this)
            {
                skipconnection = true;
                List<Helpplaner.Service.Objects.WorkPackage> tasks = new List<Helpplaner.Service.Objects.WorkPackage>();
                string input = "";
                _writer.Send("getalltasks;" + id);
                Object task = null;
                task = _reader.ReadObject();
                tasks.AddRange((Helpplaner.Service.Objects.WorkPackage[])task);
                skipconnection = false;
                return tasks.ToArray();
            }
        }  
        
     

        public string Send(string message)
        {
            lock (this)
            {
                _writer.Send(message);
                return _reader.Read();
            }
        }
        public string Receive(string message)
        {
            lock(this) { 
            return _reader.Read();
            }
        }

        public User TryLogin(string username, string password)
        {

            skipconnection = true;
            lock (this)
            {
                _writer.Send("login;" + username + ";" + password);

                string input = _reader.Read();

                if (input == "done")
                {

                    User user = (User)_reader.ReadObject();
                    skipconnection = false;
                    return user;
                }
                if(input == "NDone")

                {
                    skipconnection = false;
                    return null;
                }
                if(input == "UserLogged")
                {
                    User user = (User)_reader.ReadObject();
                    LogOtherSessionOut(user);
                    skipconnection = false;
                    
                    return user;
                }
                else
                {
                    skipconnection = false;
                    return null;
                }
            }
         
        }   


        public WorkPackage GetTaksWithIDinProject(Project Proj, int IDinProject)
        {
            lock(this)
            {
            _writer.Send("GetTaksWithIDinProject;" + Proj.ID + ";" + IDinProject);
            return (WorkPackage)_reader.ReadObject();
            }
        }

        public int GetFristAvalibleIdinProject(Project Proj)
        {
            lock(this)
            {
            _writer.Send("GetFristAvalibleIdinProject;" + Proj.ID);
            return int.Parse(_reader.Read());
            }
        }   


        public void LogOtherSessionOut(User user)
        {
            lock(this)
            {
              
            _writer.Send("logoutother;" + user.ID);
            _reader.Read();
            }
        }   
        public Project[] GetProjectsforUser()
        {
            lock(this)
            { 
            List<Project> projects = new List<Project>();
            string input = "";
            _writer.Send("getallprojects");
            Object proj = null;
            proj = _reader.ReadObject();
            projects.AddRange((Project[])proj);
            return projects.ToArray();
            }
        }
        public void Logout()
        {
            lock(this) {
            _writer.Send("logout");
            _reader.Read();
            }
        }
         
        public Project[] GetAdminProjekts()
        {
            lock (this) { 
            List<Project> projects = new List<Project>();
            string input = "";
            _writer.Send("getadminprojects");
            Object proj = null;
            proj = _reader.ReadObject();
            projects.AddRange((Project[])proj);
            return projects.ToArray();
            }
        }

        public void ReceiveAsyncMessage(object sender, string e)
        {
            lock(this) {
            if (ServerMessage != null)
            {
                ServerMessage(sender, e);
            }
            }
        }   


        public string AddTaskToProject(Helpplaner.Service.Objects.WorkPackage task, int projectid)
        {
            lock (this) { 
        
            _writer.Send("addTask;" + projectid);
            if (_reader.Read() == "ok")
            {
                _writer.SendObject(task);
            }   

           _reader.Read();
            _writer.Send($"GetTaksIdWithIDinProject;{projectid};{task.IdInProject}" );
            return _reader.Read();
            }

        }
        public void addDependency(WorkPackage[] tasks, WorkPackage task)
        {
            lock (this)
            {
                
            
            _writer.Send("addDependency;");
            if (_reader.Read() == "ok")
            {
                _writer.SendObject(task);
                _writer.SendObjectArray(tasks);  
            }

            _reader.Read();
            }
        }


        public void DeleteTask(Helpplaner.Service.Objects.WorkPackage task)
        {
            lock (this) { 
            _writer.Send("DeleteTask;" + task.ID);

            _reader.Read();
            }
        }

        public void StartWorkSession(WorkPackage session)
        {
            lock (this)
            {
            _writer.Send("startWorkSession;" + session.ID);
            _reader.Read();
            }
        }  
        public void StopWorkSession()
        {
            lock (this)
            {
            _writer.Send("stopWorkSession;" );
            _reader.Read();
            }
        }
        public void PauseWorkSession()
        {
            lock (this)
            {
            _writer.Send("pauseWorkSession;");
            _reader.Read();
            }
        }  
       public void ContinueWorkSession()
        {
            lock (this)
            {
                _writer.Send("continueWorkSession;");
                _reader.Read();
            }
        }
    }
}
