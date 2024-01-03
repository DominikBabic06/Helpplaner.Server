namespace Helpplaner.Service.SqlHandling
   
{
    using System.Data.SqlClient;
    using System.Net.Security;
    using Helpplaner.Service.Objects;
    using Helpplaner.Service.Shared;


    public class SelectSqlCommandHandler
    {
        SqlConnection _connection;  
        IServiceLogger  _logger;    
        public SelectSqlCommandHandler(SqlConnection connection, IServiceLogger logger )
        {
            _connection = connection;
            _logger = logger;

        }   
    

        #region User
        public User[] GiveAllUsers()
        {
            _connection.Open();
            _logger.Log("Connection to database established", "green");
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
                            user.NutzerPasswort = reader["NutzerPasswort"].ToString();
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
          _connection.Close();
            _logger.Log("Connection to database closed", "red");
          return users.ToArray();
          
        }

        public User GiveUser(int id )
        {
            _connection.Open(); 
            _logger.Log("Connection to database established", "green"); 
            User user = new User();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Nutzer where Nutzer_ID = " + id, _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           user.Nutzer_ID = reader["Nutzer_ID"].ToString();
                            user.NutzerPasswort = reader["NutzerPasswort"].ToString();
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
            _connection.Close();
            _logger.Log("Connection to database closed", "red");
            return user;


        }
        public User GiveUser(string username)
        {
            _connection.Open();
            _logger.Log("Connection to database established", "green"); 
            User user = new User();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Nutzer where Nutzername = '" + username + "'", _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            user.Nutzer_ID = reader["Nutzer_ID"].ToString();    
                            user.NutzerPasswort = reader["NutzerPasswort"].ToString();  
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
            _connection.Close();
            _logger.Log("Connection to database closed", "red");    
            return user;    
        }
        #endregion
        #region Project
        public Project GiveProjekt(int id)
        {
            _connection.Open(); 
            _logger.Log("Connection to database established", "green"); 

            Project proj = new Project(); 
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Projekt where Projekt_ID = " + id , _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                     proj.Projekt_ID = reader["Projekt_ID"].ToString(); 
                        proj.Projekt_Name = reader["Projekt_Name"].ToString();  
                        proj.Projekt_Beschreibung = reader["Projekt_Beschreibung"].ToString();  
                        //noch die untergeordneten Objekte hinzufügen

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, "red");


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
                using (SqlCommand command = new SqlCommand("Select * from Arbeitspaket where Arbeitspaket_ID = " + id, _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                          
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
        public Task[] GetAllTasks()
        {
            List<Task> tasks = new List<Task>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Arbeitspaket", _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           
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
                using (SqlCommand command = new SqlCommand("Select * from Kategorie where Kategorie_ID = " + id, _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           
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
        public Comment[] GetAllKommentare()
        {
            List<Comment> kommentare = new List<Comment>();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Kommentar", _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           
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


    }
}