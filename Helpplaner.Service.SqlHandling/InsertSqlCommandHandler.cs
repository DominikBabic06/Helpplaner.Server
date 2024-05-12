using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Helpplaner.Service.Objects;
using Helpplaner.Service.Shared;
using System.Data.SqlClient;

namespace Helpplaner.Service.SqlHandling
{
    public class InsertSqlCommandHandler
    {
        SqlConnection _connection;
        IServiceLogger _logger;

       public InsertSqlCommandHandler( SqlConnection connection, IServiceLogger logger)
        { 
            _connection = connection;
            _logger = logger;   

        }



        public void InsertProject(Project project)
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO Project ( Name, Description , IsRunning) VALUES ( @Name, @Description, @IsRunning)", _connection);
             
                command.Parameters.AddWithValue("@Name", project.Name);
                command.Parameters.AddWithValue("@Description", project.Description);
                command.Parameters.AddWithValue("@IsRunning", Convert.ToInt32(project.Active));  
                command.ExecuteNonQuery();
             
            }
            catch (Exception e)
            {
                _logger.Log(e.Message,"red");
            }
        }  
        
        public void InsertNutzer(User user)
        {             try
            {
                SqlCommand command = new SqlCommand("INSERT INTO Nutzer ( Username, Password, Email) VALUES ( @Username, @Password, @Email)", _connection);
               
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Email", user.Email);  
               
                command.ExecuteNonQuery();
            }
                       catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }  
        public void InsertArbeitspaket(Objects.WorkPackage task)
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO WorkPackage ( Name, ProjectID, Description, ExpectedTime, RealTime,  Responsible, Status, IDinProject) VALUES ( @Name, @ProjectID, @Description, @ExpectedTime, @RealTime, @Responsible, @Status, @IDinProject)", _connection);
               
                command.Parameters.AddWithValue("@Name", task.Name);
                command.Parameters.AddWithValue("@ProjectID", task.ProjectID);
                command.Parameters.AddWithValue("@Description", task.Description);
                command.Parameters.AddWithValue("@ExpectedTime", task.ExpectedTime);
                command.Parameters.AddWithValue("@RealTime", task.RealTime);
                command.Parameters.AddWithValue("@Responsible", task.Responsible);
                command.Parameters.AddWithValue("@Status", task.Status);   
                command.Parameters.AddWithValue("@IDinProject", task.IdInProject);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        } 
        public void InsertArbeitsSitzung(WorkSession workSession)
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO WorkSession ( WorkPackageID, CreatorID,  WorkTime) VALUES ( @WorkPackageID, @CreatorID,  @WorkTime)", _connection);
           
                command.Parameters.AddWithValue("@WorkPackageID", workSession.WorkPackageID);
                command.Parameters.AddWithValue("@CreatorID", workSession.CreatorID); 
                command.Parameters.AddWithValue("@WorkTime", workSession.WorkTime);
             
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }   
        public void InsertProjektNutzer(Project project, User userid, bool Admin)
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO ProjectUser (UserID, ProjectID, Administrator ) VALUES (@UserID, @ProjectID , @Administrator)", _connection);
                command.Parameters.AddWithValue("@ProjectID", project.ID);
                command.Parameters.AddWithValue("@UserID", userid.ID);
                command.Parameters.AddWithValue("@Administrator", Admin);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }  
   

        public void InsertKommentar(Comment comment)


        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO Comment ( CreatorID, WorkPackageID, Text) VALUES ( @CreatorID, @WorkPackageID, @Text )", _connection);
             
                command.Parameters.AddWithValue("@CreatorID", comment.CreatorID);
                command.Parameters.AddWithValue("@WorkPackageID", comment.WorkPackageID);
                command.Parameters.AddWithValue("@Text", comment.Text);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }

        public void InsertKategorie(Category kategorie)
        {             try
            {
                SqlCommand command = new SqlCommand("INSERT INTO Category ( Name) VALUES ( @Name)", _connection);
              
                command.Parameters.AddWithValue("@Name", kategorie.Name);
                command.ExecuteNonQuery();
            }
                       catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }  
        
        public void InsertArbeitspaketKategorie(Objects.WorkPackage task, Category kategorie)
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO WorkpackageCategory (WorkPackageID, Kategorie_ID) VALUES (@WorkPackageID, @CategoryID)", _connection);
                command.Parameters.AddWithValue("@WorkPackageID", task.ID);
                command.Parameters.AddWithValue("@CategoryID", kategorie.ID);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }   

       public void InsertRelation(Objects.WorkPackage Pre, Objects.WorkPackage After )
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO WorkpackageRelation (PredecessorID, SuccessorID) VALUES (@PredecessorID, @SuccessorID)", _connection);
                command.Parameters.AddWithValue("@PredecessorID", Pre.ID);
                command.Parameters.AddWithValue("@SuccessorID", After.ID);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }   


    }
}
