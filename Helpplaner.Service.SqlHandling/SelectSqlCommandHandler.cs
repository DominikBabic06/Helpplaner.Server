namespace Helpplaner.Service.SqlHandling
   
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.SqlTypes;
    using System.Net.Security;
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


        #region User
        public User[] GiveAllUsers()
        {
          
            
            List<User> users = new List<User>();
            try
            {
                using (SqlCommand command = new SqlCommand("SELECT * FROM Nutzer", _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User();

                            user.Nutzer_ID = reader["Nutzer_ID"].ToString();
                            user.Nutzer_Passwort = reader["Nutzer_Passwort"].ToString();
                            user.Nutzernamen = reader["Nutzernamen"].ToString();
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
                using (SqlCommand command = new SqlCommand("Select * from Nutzer where Nutzer_ID = @Nutzer_ID", _connection))
                {
                    command.Parameters.AddWithValue("@Nutzer_ID", id);
                    _logger.Log(command.CommandText, "green");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            user.Nutzer_ID = reader["Nutzer_ID"].ToString();
                            user.Nutzer_Passwort = reader["Nutzer_Passwort"].ToString();
                            user.Nutzernamen = reader["Nutzernamen"].ToString();
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
                using (SqlCommand command = new SqlCommand("Select * from Nutzer where Nutzernamen = @1", _connection))
                {
                    command.Parameters.AddWithValue("@1", sqlString);
                    _logger.Log(command.CommandText, "green");  
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Nutzer_ID = reader["Nutzer_ID"].ToString();
                            user.Nutzer_Passwort = reader["Nutzer_Passwort"].ToString();
                            user.Nutzernamen = reader["Nutzernamen"].ToString();
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
        public Project GiveProjekt(int id)
        {


            Project proj = new Project();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Projekt where Projekt_ID = @Projekt_ID ", _connection))
                {
                    command.Parameters.AddWithValue("@Projekt_ID", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            proj.Projekt_ID = reader["Projekt_ID"].ToString();
                            proj.Projekt_Name = reader["Projekt_Name"].ToString();
                            proj.Projekt_Beschreibung = reader["Projekt_Beschreibung"].ToString();

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
                using (SqlCommand command = new SqlCommand("Select * from Projekt", _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Project proj = new Project();
                            proj.Projekt_ID = reader["Projekt_ID"].ToString();
                            proj.Projekt_Name = reader["Projekt_Name"].ToString();
                            proj.Projekt_Beschreibung = reader["Projekt_Beschreibung"].ToString();
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

        public Task GetTask(int id)
        {
          
            Task task = new Task();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Arbeitspaket where Arbeitspaket_ID = @Arbeitspaket_ID" , _connection))
                {
                    command.Parameters.AddWithValue("@Arbeitspaket_ID", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            task.Arbeitspaket_ID = reader["Arbeitspaket_ID"].ToString();
                            task.Arbeitspaket_Name = reader["Arbeitspaket_Name"].ToString();    
                            task.Projekt_ID = reader["Projekt_ID"].ToString();  
                            task.Arbeitspaket_Beschreibung = reader["Arbeitspaket_Beschreibung"].ToString();    
                            task.FruehestmoeglicherAnfang = reader["FruehestmoeglicherAnfang"].ToString();   
                            task.FruehestmoeglichesEnde = reader["FruehestmoeglichesEnde"].ToString();   
                            task.SpaetmoeglichsterAnfang = reader["SpaetmoeglichsterAnfang"].ToString(); 
                            task.SpaetmoeglichstesEnde = reader["SpaetmoeglichstesEnde"].ToString();
                            task.Arbeitspaket_InsgeArbeitszeit = reader["Arbeitspaket_InsgeArbeitszeit"].ToString();
                            task.Arbeitspaket_Zustaendiger = reader["Arbeitspaket_Zustaendiger"].ToString();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
          
            return task;
        }
        public Task[] GetAllTasks(Project proj)
        {
           
            List<Task> tasks = new List<Task>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Arbeitspaket where Projekt_ID = @Projekt_ID"  , _connection))
                {
                    command.Parameters.AddWithValue("@Projekt_ID", proj.Projekt_ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Task task = new Task();
                            task.Arbeitspaket_ID = reader["Arbeitspaket_ID"].ToString();
                            task.Arbeitspaket_Name = reader["Arbeitspaket_Name"].ToString();
                            task.Projekt_ID = reader["Projekt_ID"].ToString();
                            task.Arbeitspaket_Beschreibung = reader["Arbeitspaket_Beschreibung"].ToString();
                            task.FruehestmoeglicherAnfang = reader["FruehestmoeglicherAnfang"].ToString();
                            task.FruehestmoeglichesEnde = reader["FruehestmoeglichesEnde"].ToString();
                            task.SpaetmoeglichsterAnfang = reader["SpaetmoeglichsterAnfang"].ToString();
                            task.SpaetmoeglichstesEnde = reader["SpaetmoeglichstesEnde"].ToString();
                            task.Arbeitspaket_InsgeArbeitszeit = reader["Arbeitspaket_InsgeArbeitszeit"].ToString();
                            task.Arbeitspaket_Zustaendiger = reader["Arbeitspaket_Zustaendiger"].ToString();
                            tasks.Add(task);

                        }
                    }
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
        public Kategorie[] GetAllKategorien()
        {
           
            List<Kategorie> kategorien = new List<Kategorie>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Kategorie", _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kategorien.Add(new Kategorie() { Kategorie_ID = reader["Kategorie_ID"].ToString(), Name = reader["Name"].ToString() }); 


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
        public Kategorie GetKategorie(int id)
        {
           
            Kategorie kat = new Kategorie();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Kategorie where Kategorie_ID = @Kategorie_ID " , _connection))
                {
                    command.Parameters.AddWithValue("@Kategorie_ID", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kat.Kategorie_ID = reader["Kategorie_ID"].ToString();   
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
        public Comment[] GetAllKommentare(Task task)
        {
           
            List<Comment> kommentare = new List<Comment>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Kommentar where Arbeitspaket_ID = @Arbeitspaket_ID" , _connection))
                {
                    command.Parameters.AddWithValue("@Arbeitspaket_ID", task.Arbeitspaket_ID);  
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kommentare.Add(new Comment() { Kommentar_ID = reader["Kommentar_ID"].ToString(), Ersteller_ID = reader["Ersteller_ID"].ToString(), Arbeitspaket_ID = reader["Arbeitspaket_ID"].ToString(), Inhalt = reader["Inhalt"].ToString() }); 

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
                using (SqlCommand command = new SqlCommand("Select * from Kommentar where Ersteller_ID = @Ersteller_ID" , _connection))
                {
                    command.Parameters.AddWithValue("@Ersteller_ID", user.Nutzer_ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kommentare.Add(new Comment() { Kommentar_ID = reader["Kommentar_ID"].ToString(), Ersteller_ID = reader["Ersteller_ID"].ToString(), Arbeitspaket_ID = reader["Arbeitspaket_ID"].ToString(), Inhalt = reader["Inhalt"].ToString() });

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
                string query = "Select * from AdminProjekt where Projekt_ID = @Projekt_ID " ;

                using (SqlCommand command = new SqlCommand(query, _connection))
                {
                    command.Parameters.AddWithValue("@Projekt_ID", proj.Projekt_ID);    
                    
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            adminIDs.Add(reader["Nutzer_ID"].ToString());   
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
                using (SqlCommand command = new SqlCommand("Select * from AdminProjekt where Nutzer_ID = @Nutzer_ID" , _connection))
                {
                    command.Parameters.AddWithValue("@Nutzer_ID", user.Nutzer_ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            projektIDs.Add(reader["Projekt_ID"].ToString());    

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
        public WorkSession[] GetAllArbeitsSitzungen(Task task)
        {
            List<WorkSession> arbeitsSitzungen = new List<WorkSession>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from ArbeitsSitzung where Arbeitspaket_ID = @Arbeitspaket_ID" , _connection))
                {
                    command.Parameters.AddWithValue("@Arbeitspaket_ID", task.Arbeitspaket_ID);  
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            WorkSession session   = new WorkSession();
                            session.Arbeitssitzung_ID = reader["Arbeitssitzung_ID"].ToString();
                                session.Arbeitspaket_ID = reader["Arbeitspaket_ID"].ToString();
                            session.Ersteller_ID = reader["Ersteller_ID"].ToString();
                            session.Arbeitszeit = reader["Arbeitszeit"].ToString(); 
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
        public Kategorie[] GetAllKategorien(Task task)
        {
            List<Kategorie> kategorien = new List<Kategorie>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from KategorieArbeitspaket where Arbeitspaket_ID = @Arbeitspaket_ID" , _connection))
                {
                    command.Parameters.AddWithValue("@Arbeitspaket_ID", task.Arbeitspaket_ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kategorien.Add(new Kategorie() { Kategorie_ID = reader["Kategorie_ID"].ToString(), Name = reader["Name"].ToString() }); 

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
        public Kategorie[] GetKategories()
        {
            List<Kategorie> kategorien = new List<Kategorie>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Kategorie", _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kategorien.Add(new Kategorie() { Kategorie_ID = reader["Kategorie_ID"].ToString(), Name = reader["Name"].ToString() });

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

        public Task[] GetDependecys(Task task)
        {
            List<Task> tasks = new List<Task>();
            List<string> taskIDs = new List<string>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from ArbeitspaketRelation where Nachfolger_ID = @Arbeitspaket_ID " , _connection))
                {
                    command.Parameters.AddWithValue("@Arbeitspaket_ID", task.Arbeitspaket_ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            taskIDs.Add(reader["Vorgaenger_ID"].ToString());    

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
            foreach (string item in taskIDs)
            {
                tasks.Add(GetTask(Convert.ToInt32(item))); 
            }
          
            return tasks.ToArray();
        }  
        
        public Task[] GetSuccessors(Task task)
        {
            List<Task> tasks = new List<Task>();
            List<string> taskIDs = new List<string>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from ArbeitspaketRelation where Vorgaenger_ID = @Arbeitspaket_ID" , _connection))
                {
                    command.Parameters.AddWithValue("@Arbeitspaket_ID", task.Arbeitspaket_ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            taskIDs.Add(reader["Nachfolger_ID"].ToString());    

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");
            }
            foreach (string item in taskIDs)
            {
                tasks.Add(GetTask(Convert.ToInt32(item))); 
            }
          
            return tasks.ToArray();
        }
        #endregion

        #region NutzerProjekt   

        public Project[] GetAllProjekte(User user)
        {
            List<Project> projekte = new List<Project>();
            List<string> projektIDs = new List<string>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from NutzerProjekt where Nutzer_ID = @Nutzer_ID" , _connection))
                {
                    command.Parameters.AddWithValue("@Nutzer_ID", user.Nutzer_ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            projektIDs.Add(reader["Projekt_ID"].ToString());    

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
                using (SqlCommand command = new SqlCommand("Select * from NutzerProjekt where Projekt_ID = @Projekt_ID" , _connection))
                {
                    command.Parameters.AddWithValue("@Projekt_ID", proj.Projekt_ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userIDs.Add(reader["Nutzer_ID"].ToString());    

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