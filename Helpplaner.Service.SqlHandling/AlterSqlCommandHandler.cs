using Helpplaner.Service.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpplaner.Service.Objects;   


namespace Helpplaner.Service.SqlHandling
{
    public class AlterSqlCommandHandler
    {
        SqlConnection _connection;
        IServiceLogger _logger;

        public AlterSqlCommandHandler(SqlConnection connection, IServiceLogger logger)
        {
            _connection = connection;
            _logger = logger;

        }

        public void DeleteWorkTask(Objects.WorkPackage task)
        {
            try
            {
                SqlCommand command = new SqlCommand("DELETE FROM WorkPackage WHERE ID = @ID", _connection);
                command.Parameters.AddWithValue("@ID", task.ID);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }   
        public void DeleteUser(User user)
        {
            try
            {
                SqlCommand command = new SqlCommand("DELETE FROM Nutzer WHERE ID = @ID", _connection);
                command.Parameters.AddWithValue("@ID", user.ID);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }   
        public void DeleteProject(Project project)
        {
            try
            {
                SqlCommand command = new SqlCommand("DELETE FROM Project WHERE ID = @ID", _connection);
                command.Parameters.AddWithValue("@ID", project.ID);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }

        public void DeleteDependency(WorkPackage Predecessor, WorkPackage Successor)
        {
            try
            {
                SqlCommand command = new SqlCommand("DELETE FROM WorkpackageRelation WHERE PredecessorID = @PredecessorID AND SuccessorID = @SuccessorID", _connection);
                command.Parameters.AddWithValue("@PredecessorID", Predecessor.ID);
                command.Parameters.AddWithValue("@SuccessorID", Successor.ID);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }

        public void EditWorkPackage(WorkPackage task)
        {
            try
            {
                SqlCommand command = new SqlCommand("UPDATE WorkPackage SET Name = @Name, ProjectID = @ProjectID, Description = @Description, ExpectedTime = @ExpectedTime, RealTime = @RealTime, Responsible = @Responsible, Status = @Status WHERE ID = @ID", _connection);
                command.Parameters.AddWithValue("@ID", task.ID);
                command.Parameters.AddWithValue("@Name", task.Name);
                command.Parameters.AddWithValue("@ProjectID", task.ProjectID);
                command.Parameters.AddWithValue("@Description", task.Description);
                command.Parameters.AddWithValue("@ExpectedTime", task.ExpectedTime);
                command.Parameters.AddWithValue("@RealTime", task.RealTime);
                command.Parameters.AddWithValue("@Responsible", task.Responsible);
                command.Parameters.AddWithValue("@Status", task.Status);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }
        public void EditProject(Project project)
        {
            try
            {
                SqlCommand command = new SqlCommand("UPDATE Project SET Name = @Name, Description = @Description WHERE ID = @ID", _connection);
                command.Parameters.AddWithValue("@ID", project.ID);
                command.Parameters.AddWithValue("@Name", project.Name);
                command.Parameters.AddWithValue("@Description", project.Description);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }       
        public void EditUser(User user)
        {
            try
            {
                SqlCommand command = new SqlCommand("UPDATE Nutzer SET Username = @Username, Password = @Password, Email = @Email WHERE ID = @ID", _connection);
                command.Parameters.AddWithValue("@ID", user.ID);
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
        public void EditRelation(WorkPackage Predecessor, WorkPackage Successor, WorkPackage OriginalPredeccessor)
        {
            try
            {
                SqlCommand command = new SqlCommand("UPDATE WorkpackageRelation SET PredecessorID = @PredecessorID, SuccessorID = @SuccessorID WHERE PredecessorID = @OriginalPredecessorID AND SuccessorID = @SuccessorID", _connection);
                command.Parameters.AddWithValue("@PredecessorID", Predecessor.ID);
                command.Parameters.AddWithValue("@SuccessorID", Successor.ID);
                command.Parameters.AddWithValue("@OriginalPredecessorID", OriginalPredeccessor.ID);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }
       


    }
}
