namespace Helpplaner.Service.SqlHandling
   
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Net.Security;
    using System.Threading.Tasks;
    using Helpplaner.Service.Objects;
    using Helpplaner.Service.Shared;


    public class SelectSqlCommandHandler
    {
        SqlConnection _connection;
        IServiceLogger _logger;
        public SelectSqlCommandHandler(SqlConnection connection, IServiceLogger logger)
        {
            _connection = connection;
            _logger = logger;

        }

        public int GetLastAddedWorkPackageID()
        {
            int id = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("SELECT TOP 1 *  FROM WorkPackage ORDER    BY ID DESC;", _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            id = Convert.ToInt32(reader["ID"].ToString());  




                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
            return id;
        }
        #region User
        public User[] GiveAllUsers()
        {
          
            
            List<User> users = new List<User>();
            try
            {
                using (SqlCommand command = new SqlCommand(@"SELECT * FROM ""User""", _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User();

                            user.ID = reader["ID"].ToString();
                            user.Password = reader["Password"].ToString();
                            user.Username = reader["Username"].ToString();
                            user.Email = reader["Email"].ToString();


                            users.Add(user);
                            _logger.Log(user.ToString(), "green");



                        }
                    }


                }

            }
            catch (Exception ex)
            {

                _logger.Log(ex.Message, "red");
            }
             
          
            return users.ToArray();

        }

        public User GiveUser(int id)
        {
            
            User user = new User();
            try
            {
                using (SqlCommand command = new SqlCommand(@"Select * from ""User"" where ""ID"" = @ID", _connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    _logger.Log(command.CommandText, "green");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            user.ID = reader["ID"].ToString();
                            user.Password = reader["Password"].ToString();
                            user.Username = reader["Username"].ToString();
                            user.Email = reader["Email"].ToString();
                            _logger.Log(user.ToString(), "green");




                        }
                    }


                }

            }
            catch (Exception ex)
            {

                _logger.Log(ex.Message, "red");
            }
         
            return user;


        }
        public User GiveUser(string username)
        {
           
            User user = new User();
            try
            {
                SqlString sqlString = new SqlString(username);  
                using (SqlCommand command = new SqlCommand("Select * from \"User\" where Username = @1", _connection))
                {
                    command.Parameters.AddWithValue("@1", sqlString);
                    _logger.Log(command.CommandText, "green");  
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.ID = reader["ID"].ToString();
                            user.Password = reader["Password"].ToString();
                            user.Username = reader["Username"].ToString();
                            user.Email = reader["Email"].ToString();

                            _logger.Log(user.ToString(), "green");

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.Log(ex.Message, "red");
            }
           
            return user;
        }
        #endregion

        #region Project


        public int GiveLastProjectID()
        {
            int id = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("SELECT TOP 1 *  FROM Project ORDER    BY ID DESC;", _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            id = Convert.ToInt32(reader["ID"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }   

            return id;
        }
        public Project GiveProjekt(int id)
        {


            Project proj = new Project();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Project where ID = @ID ", _connection))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            proj.ID = reader["ID"].ToString();
                            proj.Name = reader["Name"].ToString();
                            proj.Description = reader["Description"].ToString();
                            proj.Active  = reader["IsRunning"].ToString() == "True" ? true : false;

                            _logger.Log(proj.ToString(), "green");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");


            }
            foreach  ( User item in GetAllAdmins(proj)
                )
            {
                _logger.Log(item.ToString(), "green");
            }   

            return proj;
        }
        public Project[] GiveAllProjekte()
        {
          
            List<Project> projekte = new List<Project>();
               
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Project", _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project proj = new Project();
                            proj.ID = reader["ID"].ToString();
                            proj.Name = reader["Name"].ToString();
                            proj.Description = reader["Description"].ToString();
                            projekte.Add(proj); 

                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
         
            return projekte.ToArray();

        }


        #endregion

        #region Task 

        public WorkPackage GetTask(int id)
        {
          
            WorkPackage task = new WorkPackage();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from WorkPackage where ID = @ID", _connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            task.ID = reader["ID"].ToString();
                            task.Name = reader["Name"].ToString();    
                            task.ProjectID = reader["ProjectID"].ToString();  
                            task.Description = reader["Description"].ToString();    
                            task.ExpectedTime = reader["ExpectedTime"].ToString();
                            task.RealTime = reader["RealTime"].ToString();  
                            task.Responsible = reader["Responsible"].ToString();
                            task.Status = reader["Status"].ToString();
                            task.IdInProject = reader["IdInProject"].ToString();
                        }
                    }
               }
              
                    string Dependecys = "";
                    string Successors = "";
                    foreach (string item2 in GetDependecys(task))
                    {
                       
                        Dependecys += item2 + " ";
                    }
                    foreach (string item2 in GetSuccessors(task))
                    {
                        Successors += item2  + " ";
                    }
                    task.Dependecy = Dependecys;
                    task.Successor = Successors;
                   task.ArbeitsSitzungs.AddRange(GetAllArbeitsSitzungen(task)); 



               


            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
          
            return task;
        }


        public int GetFirstAvaliableIdInProject(Project proj)
        {
            int id = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("Select  top 1 IDinProject +1 as FirstId from WorkPackage where ProjectID = @ProjectID order by IDinProject  DESC;  ", _connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", proj.ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = Convert.ToInt32(reader["FirstId"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
            return id;
        }

        public WorkPackage GetTaksWithIDinProject(Project proj, string idInProject)
        {
            WorkPackage task = new WorkPackage();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from WorkPackage where ProjectID = @ProjectID and IdInProject = @IdInProject", _connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", proj.ID);
                    command.Parameters.AddWithValue("@IdInProject", idInProject);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            task.ID = reader["ID"].ToString();
                            task.Name = reader["Name"].ToString();
                            task.ProjectID = reader["ProjectID"].ToString();
                            task.Description = reader["Description"].ToString();
                            task.ExpectedTime = reader["ExpectedTime"].ToString();
                            task.RealTime = reader["RealTime"].ToString();
                            task.Responsible = reader["Responsible"].ToString();
                            task.Status = reader["Status"].ToString();
                            task.IdInProject = reader["IdInProject"].ToString();
                        }
                    }
                }

                string Dependecys = "";
                string Successors = "";
                foreach (string item2 in GetDependecys(task))
                {

                    Dependecys += item2 + " ";
                }
                foreach (string item2 in GetSuccessors(task))
                {
                    Successors += item2 + " ";
                }
                task.Dependecy = Dependecys;
                task.Successor = Successors;

            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }

            return task;
        }
        public WorkPackage[] GetAllTasks(Project proj)
        {
           
            List<WorkPackage> tasks = new List<WorkPackage>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from WorkPackage where ProjectID = @ProjectID", _connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", proj.ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            WorkPackage task = new WorkPackage();
                            task.ID = reader["ID"].ToString();
                            task.Name = reader["Name"].ToString();
                            task.ProjectID = reader["ProjectID"].ToString();
                            task.Description = reader["Description"].ToString();
                            task.ExpectedTime = reader["ExpectedTime"].ToString();
                            task.RealTime = reader["RealTime"].ToString();
                            task.Responsible = reader["Responsible"].ToString();
                            task.Status = reader["Status"].ToString();
                            task.IdInProject = reader["IdInProject"].ToString();
                            tasks.Add(task);

                        }
                    }
                    
                }

                foreach (WorkPackage item in tasks)
                {
                       string Dependecys =  ""; 
                       string Successors = "";
                       foreach (string item2 in GetDependecys(item))
                    {

                        Dependecys += item2 + " ";
                    }
                    foreach (string item2 in GetSuccessors(item))
                    {
                        Successors += item2 + " ";
                    }
                    item.Dependecy = Dependecys;
                         item.Successor = Successors;    

                      
                    
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
         
            return tasks.ToArray();
        }
        #endregion

        #region Kategorie 
        public Category[] GetAllKategorien()
        {
           
            List<Category> kategorien = new List<Category>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Category", _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kategorien.Add(new Category() { ID = reader["ID"].ToString(), Name = reader["Name"].ToString() }); 


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
          
            return kategorien.ToArray();
        }
        public Category GetKategorie(int id)
        {
           
            Category kat = new Category();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Category where ID = @ID ", _connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kat.ID = reader["ID"].ToString();   
                            kat.Name = reader["Name"].ToString();   

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
          
            return kat;
        }
        #endregion

        #region Kommentar
        public Comment[] GetAllKommentare(WorkPackage task)
        {
           
            List<Comment> kommentare = new List<Comment>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Comment where WorkPackageID = @WorkPackageID", _connection))
                {
                    command.Parameters.AddWithValue("@WorkPackageID", task.ID);  
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kommentare.Add(new Comment() { ID = reader["ID"].ToString(), CreatorID = reader["CreatorID"].ToString(), WorkPackageID = reader["WorkPackageID"].ToString(), Text = reader["Text"].ToString() }); 

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
          
            return kommentare.ToArray();
        }
        public Comment[] GetAllKommentare(User user)
        {
            List<Comment> kommentare = new List<Comment>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Comment where CreatorID = @CreatorID", _connection))
                {
                    command.Parameters.AddWithValue("@CreatorID", user.ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kommentare.Add(new Comment() { ID = reader["ID"].ToString(), CreatorID = reader["CreatorID"].ToString(), WorkPackageID = reader["WorkPackageID"].ToString(), Text = reader["Text"].ToString() });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
          
            return kommentare.ToArray();
        }
        #endregion

        #region Admins
        public User[] GetAllAdmins(Project proj)
        {
            List<User> admins = new List<User>();
            List<string> adminIDs = new List<string>();

            try
            {
             string query = "Select * from ProjectUser where ProjectID = @ProjectID and Administrator = 1   ";

                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", proj.ID);    
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            adminIDs.Add(reader["UserID"].ToString());   
                        }
                    }
                }
                foreach (string item in adminIDs)
                {
                    admins.Add(GiveUser(Convert.ToInt32(item))); 
                }   
            }
            catch (Exception ex)
            {
                // Log the entire exception, including stack trace
                _logger.Log($"Error in GetAllAdmins: {ex}", "red");
            }

            return admins.ToArray();
        }

        public Project[] GetAllAdminProjekte(User user)
        {
            List<Project> projekte = new List<Project>();
            List<string> projektIDs = new List<string>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from ProjectUSer where UserID = @UserID and Administrator = 1 ", _connection))
                {
                    command.Parameters.AddWithValue("@UserID", user.ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            projektIDs.Add(reader["ProjectID"].ToString());    

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
            foreach (string item in projektIDs)
            {

                projekte.Add(GiveProjekt(Convert.ToInt32(item))); 
            }
          
            return projekte.ToArray();
        }
        #endregion

        #region ArbeitsSitzung
      

        public string GetSumOFWorkingHours(WorkPackage package)
        {
            string time = "";
            try
            {
                using (SqlCommand command = new SqlCommand("select Convert( time(7),DATEADD(ms, SUM(DATEDIFF(ms, '00:00:00.000', WorkTime)), '00:00:00.000')) as time from WorkSession where WorkPackageID = @WorkPackageID", _connection))
                {
                    command.Parameters.AddWithValue("@WorkPackageID", package.ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            time = reader["time"].ToString();    

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
          
            return time;
        }
        public WorkSession[] GetAllArbeitsSitzungen(WorkPackage task)
        {
            List<WorkSession> arbeitsSitzungen = new List<WorkSession>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from WorkSession where WorkPackageID = @WorkPackageID", _connection))
                {
                    command.Parameters.AddWithValue("@WorkPackageID", task.ID);  
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            WorkSession session   = new WorkSession();
                            session.ID = reader["ID"].ToString();
                                session.WorkPackageID = reader["WorkPackageID"].ToString();
                            session.CreatorID = reader["CreatorID"].ToString();
                            session.WorkTime = reader["WorkTime"].ToString(); 
                            arbeitsSitzungen.Add(session);


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
          
            return arbeitsSitzungen.ToArray();
        }
        #endregion

        #region KategorieArbeitspaket
        public Category[] GetAllKategorien(WorkPackage task)
        {
            List<Category> kategorien = new List<Category>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from WorkpackageCategory where WorkPackageID = @WorkPackageID", _connection))
                {
                    command.Parameters.AddWithValue("@WorkPackageID", task.ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kategorien.Add(new Category() { ID = reader["ID"].ToString(), Name = reader["Name"].ToString() }); 

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
          
            return kategorien.ToArray();
        }
        public Category[] GetKategories()
        {
            List<Category> kategorien = new List<Category>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Category", _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kategorien.Add(new Category() { ID = reader["ID"].ToString(), Name = reader["Name"].ToString() });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
          
            return kategorien.ToArray();
        }
        #endregion

        #region ArbeitspaketRelations

        public string[] GetDependecys(WorkPackage task)
        {
            List<WorkPackage> tasks = new List<WorkPackage>();
            List<string> taskIDs = new List<string>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from WorkpackageRelation where SuccessorID = @SuccessorID ", _connection))
                {
                    command.Parameters.AddWithValue("@SuccessorID", task.ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            taskIDs.Add(reader["PredecessorID"].ToString());    

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }

          
            return taskIDs.ToArray();
        }  
        
        public string[] GetSuccessors(WorkPackage task)
        {
            List<WorkPackage> tasks = new List<WorkPackage>();
            List<string> taskIDs = new List<string>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from WorkpackageRelation where PredecessorID = @PredecessorID", _connection))
                {
                    command.Parameters.AddWithValue("@PredecessorID", task.ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            taskIDs.Add(reader["SuccessorID"].ToString());    

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
         

            return taskIDs.ToArray();
        }
        #endregion

        #region NutzerProjekt   

        public Project[] GetAllProjekte(User user)
        {
            List<Project> projekte = new List<Project>();
            List<string> projektIDs = new List<string>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from ProjectUser where UserID = @UserID", _connection))
                {
                    command.Parameters.AddWithValue("@UserID", user.ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            projektIDs.Add(reader["ProjectID"].ToString());    

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
            foreach (string item in projektIDs)
            {
                projekte.Add(GiveProjekt(Convert.ToInt32(item))); 
            }
          
            return projekte.ToArray();
        }
        public User[] GetAllUsers(Project proj)
        {
            List<User> users = new List<User>();
            List<string> userIDs = new List<string>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from ProjectUser where ProjectID = @ProjectID", _connection))
                {
                    command.Parameters.AddWithValue("@ProjectID", proj.ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userIDs.Add(reader["UserID"].ToString());    

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
            foreach (string item in userIDs)
            {
                users.Add(GiveUser(Convert.ToInt32(item))); 
            }
          
            return users.ToArray();
        }   
        #endregion


    }
}