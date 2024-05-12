using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Helpplaner.Service.Shared;
using System.Data.SqlClient;
using Helpplaner.Service.SqlHandling;
using Helpplaner.Service.Objects;
using System.Reflection.Metadata.Ecma335;
using System.Diagnostics;

namespace Helpplaner.Service.Core
{
    internal class SessionHandler
    {
        Socket _clientSocket;
        IServiceLogger _logger;
        public Guid _sessionId;
        SocketWriter writer;
        SocketReader reader;
        SqlConnection _connection;
        InsertSqlCommandHandler _insertSqlCommandHandler;
        SelectSqlCommandHandler _selectSqlCommandHandler;
        AlterSqlCommandHandler _alterSqlCommandHandler;
        public  User user;
        string info;
        Config config;
        public  Project currentProj;
        public WorkSession currentWorkSession; 
        Stopwatch sw = new Stopwatch();
        ClientHandler father;
       public Delegate _Dissconect;
        

        public SessionHandler(Socket client, IServiceLogger logger, Guid id, ClientHandler father)
        {
            config = new Config();
            _logger = logger;
            _clientSocket = client;
            _sessionId = id;
            writer = new SocketWriter(_clientSocket, _logger);
            reader = new SocketReader(_clientSocket, _logger);
            info = "None";
            _Dissconect = Close;
           this.father = father;    

            _connection = new SqlConnection(config.ConnectionString);
            _insertSqlCommandHandler = new InsertSqlCommandHandler(_connection, _logger);
            _selectSqlCommandHandler = new SelectSqlCommandHandler(_connection, _logger);
            _alterSqlCommandHandler = new AlterSqlCommandHandler(_connection, _logger); 


        }

        public void HandleClient()
        {

            string text = "";

            while (text != "exit")
            {

                text = reader.Read();
                try
                {
                    Project project = new Project();
                    Objects.WorkPackage[] tasks;
                    User[] users;
                    Project[] projects;
                    int id;
                    int ProjectId;
                    string check;
                    int CurrentWorkedOnTask;    
                     
                    if (!text.Contains("info"))
                        if (user != null)
                            _logger.Log(user.Username = text, "green");
                    switch (text.Split(';')[0])
                    {



                        //format is command;parameter1;parameter2;parameter3;...
                        case "info":
                            writer.Send(info);
                            if (info != "None")
                                _logger.Log(info, "green");
                            info = "None";
                            break;

                        case "logoutother":
                         //parameter1 is user id
                            OpenConnection();
                            User user2 = _selectSqlCommandHandler.GiveUser(Convert.ToInt32(text.Split(';')[1].Trim()));
                           
                            CloseConnection();

                            father.GetSessionIDOfUser(user2,_sessionId);
                            father.PostMessageToSpecificSession("Logout", father.GetSessionIDOfUser(user2, _sessionId));
                         
                            writer.Send("done");
                            break;
                        case "startWorkSession":
                            //parameter1 is task id
                            currentWorkSession = new WorkSession(); 
                            currentWorkSession.WorkPackageID = text.Split(';')[1].Trim();
                            currentWorkSession.CreatorID = user.ID;    
                           CurrentWorkedOnTask = Convert.ToInt32(text.Split(';')[1].Trim());    
                            writer.Send("done");
                            break;
                        case "stopWorkSession":
                            //parameter1 is task id
                            sw.Stop();
                            currentWorkSession.WorkTime = $"{sw.Elapsed.Hours}:{sw.Elapsed.Minutes}:{sw.Elapsed.Seconds}";
                           OpenConnection();
                             _insertSqlCommandHandler.InsertArbeitsSitzung(currentWorkSession);
                            WorkPackage sessionPackage = _selectSqlCommandHandler.GetTask(Convert.ToInt32(currentWorkSession.WorkPackageID));
                            sessionPackage.RealTime = _selectSqlCommandHandler.GetSumOFWorkingHours(sessionPackage);
                            _alterSqlCommandHandler.EditWorkPackage(sessionPackage);
                            CloseConnection();
                            writer.Send("done");

                           
                            TriggererServerMessage(this, "tr;" + currentProj.ID);
                            currentWorkSession  = null;
                            CurrentWorkedOnTask = 0;


                            break;
                        case "getWorkSessionsForTask":
                            //parameter1 is task id
                            OpenConnection();
                            WorkPackage workPackage = _selectSqlCommandHandler.GetTask(Convert.ToInt32(text.Split(';')[1].Trim()));
                            WorkSession[] workSessions = _selectSqlCommandHandler.GetAllArbeitsSitzungen(workPackage);
                            CloseConnection();
                            writer.SendObjectArray(workSessions);
                            break;
                        case"pauseWorkSession":
                            sw.Stop();
                            writer.Send("done");
                            break;  
                        case"continueWorkSession":
                            sw.Start();
                            writer.Send("done");
                            break;  
                        case "getallusers":
                            OpenConnection();
                            writer.SendObjectArray(_selectSqlCommandHandler.GiveAllUsers());
                            CloseConnection();
                            break;
                        case "getallprojects":
                            OpenConnection();
                            projects = _selectSqlCommandHandler.GetAllProjekte(user);
                            CloseConnection();
                            writer.SendObjectArray(projects);
                            _logger.Log("All projects sent", "green");
                            break;
                        case "getalltasks":
                            //parameter1 is project id  
                            id = int.Parse(text.Split(';')[1].Trim());

                            OpenConnection();
                            project = _selectSqlCommandHandler.GiveProjekt(id);
                            currentProj = project;  
                            tasks = _selectSqlCommandHandler.GetAllTasks(project);
                            CloseConnection();
                            writer.SendObjectArray(tasks);
                            _logger.Log("All tasks sent", "green");
                            break;
                        case "getalluserprojects":

                            id = int.Parse(text.Split(';')[1].Trim());

                            //parameter1 is project id
                            OpenConnection();
                            users = _selectSqlCommandHandler.GetAllUsers(project);
                            CloseConnection();
                            writer.SendObjectArray(users);
                            _logger.Log("All users sent", "green");
                            break;
                        case "getadminprojects":
                            OpenConnection();
                            projects = _selectSqlCommandHandler.GetAllAdminProjekte(user);
                            CloseConnection();
                            writer.SendObjectArray(projects);
                            _logger.Log("All projects sent", "green");
                            break;
                        case "getusersforproject":
                            //parameter1 is project id  
                            id = int.Parse(text.Split(';')[1].Trim());

                            OpenConnection();
                            project = _selectSqlCommandHandler.GiveProjekt(id);
                            currentProj = project;  
                            users = _selectSqlCommandHandler.GetAllUsers(project);
                            CloseConnection();
                            writer.SendObjectArray(users);
                            _logger.Log("All users sent", "green");
                            break;

                        case "addTask":
                            //parameter1 is project id

                            writer.Send("ok");

                            Objects.WorkPackage task = (Objects.WorkPackage)reader.ReadObject();


                            OpenConnection();
                            project = _selectSqlCommandHandler.GiveProjekt(Convert.ToInt32(task.ProjectID));
                            task.ID = getFirstFreeTaskIDFromProject(project);
                            _insertSqlCommandHandler.InsertArbeitspaket(task);
                            CloseConnection();
                            writer.Send(task.ID);
                            id = Convert.ToInt32(currentProj.ID);
                            TriggererServerMessage(this, "tr;" + id);
                            break;
                        case "addDependency":
                                                       
                            writer.Send("ok");
                            Objects.WorkPackage pre = (Objects.WorkPackage)reader.ReadObject(); 

                            Objects.WorkPackage[] tasks2 = (Objects.WorkPackage[])reader.ReadObject();

                            OpenConnection();
                            foreach (Objects.WorkPackage task2 in tasks2)
                            {
                                _insertSqlCommandHandler.InsertRelation(task2, pre);
                            }
                           
                            CloseConnection();
                            writer.Send("done");
                            id = Convert.ToInt32(currentProj.ID);
                            TriggererServerMessage(this, "tr;" + id);
                            break;
                        case "lastAddWorkSessionID":
                            OpenConnection();
                            writer.Send(""+_selectSqlCommandHandler.GetLastAddedWorkPackageID());
                            CloseConnection();
                            break;

                        case "getUnalowedDependecys":
                        //parameter1 is task id
                            OpenConnection();
                            WorkPackage workpackage = _selectSqlCommandHandler.GetTask(Convert.ToInt32(text.Split(';')[1].Trim()));
                          
                            CloseConnection();
                            
                            
                            writer.SendObjectArray(rekursiveSuccsersorgetting(workpackage));
                            break;  
                        case "logout":
                            user = null;
                            writer.Send("done");
                            break;
                        case "login":
                            text = text.Replace("login;", "");
                            OpenConnection();
                            CheckPassword(text);
                            CloseConnection();
                            break;
                        
                        case "DeleteTask":
                            //parameter1 is task id
                            OpenConnection();
                             WorkPackage wp =    _selectSqlCommandHandler.GetTask(Convert.ToInt32(text.Split(';')[1].Trim()));
                            foreach (string item in _selectSqlCommandHandler.GetDependecys(wp))
                            {
                                _alterSqlCommandHandler.DeleteDependency(_selectSqlCommandHandler.GetTask(Convert.ToInt32(item)),wp);
                            }
                            
                            _alterSqlCommandHandler.DeleteWorkTask((_selectSqlCommandHandler.GetTask(Convert.ToInt32(text.Split(';')[1].Trim()))));
                            CloseConnection();
                            writer.Send("done");
                            id = Convert.ToInt32(currentProj.ID);
                            TriggererServerMessage(this, "tr;" + id);
                            break;

                        case "GetTaksIdWithIDinProject":
                        //parameter1 is ProjectId 
                        //parameter2 is InProject
                            OpenConnection();
                            Project project1 = _selectSqlCommandHandler.GiveProjekt(Convert.ToInt32(text.Split(';')[1].Trim()));
                            WorkPackage workpackage1= _selectSqlCommandHandler.GetTaksWithIDinProject(project1, text.Split(';')[2].Trim());

                            CloseConnection();
                            writer.Send(workpackage1.ID);
                            break;
                        case "GetFristAvalibleIdinProject":
                            //parameter1 is ProjectId 
                            OpenConnection();
                            Project project2 = _selectSqlCommandHandler.GiveProjekt(Convert.ToInt32(text.Split(';')[1].Trim()));
                            int ProjId = _selectSqlCommandHandler.GetFirstAvaliableIdInProject(project2);

                            CloseConnection();
                            writer.Send(""+ProjId);
                            break;

                        case "CreateProjekt":
                            //parameter1 is ProjectId 
                            writer.Send("ok");  
                            OpenConnection();
                            Project project3 = (Project)reader.ReadObject();
                            _insertSqlCommandHandler.InsertProject(project3);
                            project3.ID = ""+ _selectSqlCommandHandler.GiveLastProjectID(); 
                            _insertSqlCommandHandler.InsertProjektNutzer(project3, user, true);
                            _insertSqlCommandHandler.InsertArbeitspaket(new WorkPackage() { IdInProject = "1", Name = "Start", ProjectID = project3.ID, Description = "Start", RealTime = "0", Dependecy = "", Successor = "", ExpectedTime = "", Responsible = "1", Status = "Beendet", });

                            CloseConnection();
                          
                            writer.Send("done");
                            TriggererServerMessage(this, "ReloadProjects"); 
                            break;

                        case "GiveAllUsers":
                            OpenConnection();
                            writer.SendObjectArray(_selectSqlCommandHandler.GiveAllUsers());
                            CloseConnection();
                            break;
                        case "CreateUser":
                                                        //parameter1 is ProjectId 
                            writer.Send("ok");
                            OpenConnection();
                            User user4 = (User)reader.ReadObject();
                            _insertSqlCommandHandler.InsertNutzer(user4);
                            CloseConnection();
                            writer.Send("done");
                            break;
                        case "changeProject":
                            //parameter1 is ProjectId 
                            writer.Send("ok");
                            OpenConnection();
                            Project project4 = (Project)reader.ReadObject();
                            _alterSqlCommandHandler.EditProject(project4);
                            CloseConnection();
                            writer.Send("done");
                            TriggererServerMessage(this, "ReloadProjects");
                            break;
                        case "changeTask":
                            //parameter1 is ProjectId 
                            writer.Send("ok");
                            OpenConnection();
                            WorkPackage task3 = (WorkPackage)reader.ReadObject();
                            _alterSqlCommandHandler.EditWorkPackage(task3);
                            CloseConnection();
                            writer.Send("done");
                            id = Convert.ToInt32(currentProj.ID);
                            TriggererServerMessage(this, "tr;" + id);
                            break;
                        case "changeUser":
                            //parameter1 is ProjectId 
                            writer.Send("ok");
                            OpenConnection();
                            User user5 = (User)reader.ReadObject();
                            _alterSqlCommandHandler.EditUser(user5);
                            CloseConnection();
                            writer.Send("done");
                            break;
                        case "exit":
                            Close();

                            break;
                    }
                }
                catch (Exception ex)
                {

                    _logger.Log(ex.Message, "red");
                    writer.Send("0!;error");
                }






            }
            Close();

        }
        public void PostMessage(string message)
        {
            info = message;
        }
        public void CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }
        public void OpenConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }
        }
        private void CheckPassword(string text)
        {


            try
            {
                User trueUser = _selectSqlCommandHandler.GiveUser(text.Split(';')[0].Trim());
                if(!father.IsUserInUse(trueUser, _sessionId))
                { 
                if (trueUser.Password.Trim() == text.Split(';')[1].Trim()  )
                {
                    user = trueUser;
                    writer.Send("done");
                    writer.SendObject(user);
                }
                else
                {
                    user = null;
                    writer.Send("NDone");

                }
                }
                else
                {
                    user = trueUser;
                    writer.Send("UserLogged");
                    writer.SendObject(user);

                }   
            }
            catch (Exception EX)
            {

                writer.Send("NDone");
            }


        }

        public event EventHandler SessionClosed;
        public void Close()
        {
            if (_clientSocket.Connected)
            {
                _clientSocket.Shutdown(SocketShutdown.Both);
                _clientSocket.Close();

            }
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }

            if (SessionClosed != null) //required in C# to ensure a handler is attached
                SessionClosed(this, EventArgs.Empty);

        }

        public event EventHandler<string> TriggererServerMessage;


        public void TriggerServerMessage(string message)
        {
            if (TriggererServerMessage != null)
                TriggererServerMessage(this, message);
        }

        public string getFirstFreeTaskIDFromProject(Project pr)
        {
            Objects.WorkPackage[] tasks = _selectSqlCommandHandler.GetAllTasks(pr);
            int i = 1;
            while (true)
            {
                bool found = false;
                foreach (Objects.WorkPackage task in tasks)
                {
                    if (task.ID == i.ToString() )
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return i.ToString();
                }
                i++;
            }
        }


       public string[] rekursiveSuccsersorgetting(WorkPackage wp)
        {
            List<string> succsersors = new List<string>();
            foreach (string item in wp.Successor.Split(' '))
            {
                if (item != "")
                {
                    WorkPackage wp2 = _selectSqlCommandHandler.GetTask(Convert.ToInt32(item));
                    succsersors.Add(wp2.ID);
                    succsersors.AddRange(rekursiveSuccsersorgetting(wp2));
                }
            }
            return succsersors.ToArray();
        }

        


   
    }
}
