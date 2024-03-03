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
       User user;
        string info; 

      public  SessionHandler(Socket client, IServiceLogger logger,Guid id)
        {
            _logger = logger;
            _clientSocket = client; 
            _sessionId = id;
            writer = new SocketWriter(_clientSocket, _logger);  
            reader = new SocketReader(_clientSocket, _logger);
           info = "None";
               
                
                _connection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=HELPPLANER;Integrated Security=True");
            _insertSqlCommandHandler = new InsertSqlCommandHandler(_connection, _logger);
            _selectSqlCommandHandler = new SelectSqlCommandHandler(_connection, _logger);

        }   
        
        public void HandleClient()
        {
            
            string text= "";
            
            while (text != "exit" )
            {
              
                    text = reader.Read();
                    try
                    {
                        Project project =  new Project();
                        Objects.Task[] tasks; 
                        User[] users;
                        Project[] projects;
                        int id;
                        string check; 
                      if(!text.Contains("info"))
                        if(user != null)    
                       _logger.Log(user.Nutzernamen = text , "green");
                        switch (text.Split(';')[0])
                        {



                            //format is command;parameter1;parameter2;parameter3;...
                            case "info":
                                writer.Send(info);
                                if(info != "None")
                                _logger.Log(info, "green"); 
                                info = "None";  
                                break;
                           
                        
                            case "getallusers":
                                OpenConnection();
                                writer.SendObject(_selectSqlCommandHandler.GiveAllUsers());
                                CloseConnection();
                                break;
                            case "getallprojects":
                                OpenConnection();
                                projects = _selectSqlCommandHandler.GetAllProjekte(user); 
                                
                                CloseConnection();
                                foreach (Project proj in projects)
                                {
                                    writer.SendObject(proj);
                                   check = reader.Read();
                                }   
                                writer.Send("done");
                                _logger.Log("All projects sent", "green");
                                break;
                            case "getalltasks":
                                //parameter1 is project id  
                                 id = int.Parse(text.Split(';')[1].Trim());
                               
                                OpenConnection();
                                project = _selectSqlCommandHandler.GiveProjekt(id);
                                tasks = _selectSqlCommandHandler.GetAllTasks(project);
                                CloseConnection();
                                foreach (Objects.Task ad in tasks)
                                {
                                    writer.SendObject(ad);
                                    check = reader.Read();
                                }
                                writer.Send("done");
                                _logger.Log("All tasks sent", "green");
                                 break;
                            case "getalluserprojects":

                                id = int.Parse(text.Split(';')[1].Trim());

                                //parameter1 is project id
                                OpenConnection();
                                users =  _selectSqlCommandHandler.GetAllUsers(project);
                                CloseConnection();
                                foreach (User user in users)
                                {
                                    writer.SendObject(user);
                                    check = reader.Read();

                                }   
                                writer.Send("done");
                                break;
                             case "getadminprojects":
                                OpenConnection();
                                projects = _selectSqlCommandHandler.GetAllAdminProjekte(user);
                                CloseConnection();
                                foreach (Project proj in projects)
                                {
                                    writer.SendObject(proj);
                                    check = reader.Read();

                                }
                                writer.Send("done");
                                _logger.Log("All projects sent", "green");
                                
                                break;
                            case "getusersforproject":
                                //parameter1 is project id  
                                id = int.Parse(text.Split(';')[1].Trim());

                                OpenConnection();
                                project = _selectSqlCommandHandler.GiveProjekt(id);
                                users = _selectSqlCommandHandler.GetAllUsers(project);
                                CloseConnection();
                                foreach (User user in users)
                                {
                                    writer.SendObject(user);
                                    check = reader.Read();

                                }
                                writer.Send("done");
                                break;

                        case "addTask":
                            //parameter1 is project id

                            writer.Send("ok");

                            Objects.Task task = (Objects.Task)reader.ReadObject();


                            OpenConnection();
                                 project = _selectSqlCommandHandler.GiveProjekt(Convert.ToInt32(task.Projekt_ID));   
                                 task.Arbeitspaket_ID = getFirstFreeTaskIDFromProject(project); 
                                _insertSqlCommandHandler.InsertArbeitspaket(task);
                                CloseConnection();
                                writer.Send("done");
                                 id =  Convert.ToInt32(project.Projekt_ID);
                                TriggererServerMessage(this, "tr;" + id);
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
                if (trueUser.Nutzer_Passwort == text.Split(';')[1].Trim())
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
            catch (Exception EX)
            {

                writer.Send("Login nicht erfolgreich");
            }
            

        }

        public event EventHandler SessionClosed;    
        public void Close()
        {
            if(_clientSocket.Connected)
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
           Objects.Task[] tasks = _selectSqlCommandHandler.GetAllTasks(pr);    
            int i = 0;  
            while (true)
            {
                bool found = false;
                foreach (Objects.Task task in tasks)
                {
                    if (task.Arbeitspaket_ID == i.ToString())
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
    }
}
