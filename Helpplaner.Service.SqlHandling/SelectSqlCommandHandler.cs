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
        public void HandleSelectSqlCommand()
        { 
        
        
        }
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
                            user.Id = "" + reader.GetInt32(0);
                            user.Username = reader.GetString(1);
                            user.Password = reader.GetString(2);
                            user.Email = reader.GetString(3);
                            users.Add(user);


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

        public User GiveUser(int id )
        {
            User user = new User();
            try
            {
                using (SqlCommand command = new SqlCommand("Select * from Nutzer where Nutzer_ID = " + id, _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           
                            user.Id = "" + reader.GetInt32(0);
                            user.Username = reader.GetString(1);
                            user.Password = reader.GetString(2);
                            user.Email = reader.GetString(3);
                       


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
                using (SqlCommand command = new SqlCommand("Select * from Nutzer where Nutzername = '" + username + "'", _connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            user.Id = "" + reader.GetInt32(0);
                            user.Username = reader.GetString(1);
                            user.Password = reader.GetString(2);
                            user.Email = reader.GetString(3);
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


    }
}