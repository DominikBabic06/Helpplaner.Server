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
                SqlCommand command = new SqlCommand("INSERT INTO Projekt (Projekt_ID, Projekt_Name, Projekt_Beschreibung) VALUES (@Projekt_ID, @Projekt_Name, @Projekt_Beschreibung)", _connection);
                command.Parameters.AddWithValue("@Projekt_ID", project.Projekt_ID);
                command.Parameters.AddWithValue("@Projekt_Name", project.Projekt_Name);
                command.Parameters.AddWithValue("@Projekt_Beschreibung", project.Projekt_Beschreibung);
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
                SqlCommand command = new SqlCommand("INSERT INTO Nutzer (Nutzer_ID, Nutzernamen, Nutzer_Passwort, Email) VALUES (@Nutzer_ID, @Nutzer_Name, @Nutzer_Passwort, @Email)", _connection);
                command.Parameters.AddWithValue("@Nutzer_ID", user.Nutzer_ID);
                command.Parameters.AddWithValue("@Nutzer_Name", user.Nutzernamen);
                command.Parameters.AddWithValue("@Nutzer_Passwort", user.Nutzer_Passwort);
                command.Parameters.AddWithValue("@Email", user.Email);  
                command.ExecuteNonQuery();
            }
                       catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }  
        public void InsertArbeitspaket(Objects.Task task)
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO Arbeitspaket (Arbeitspaket_ID, Arbeitspaket_Name, Projekt_ID, Arbeitspaket_Beschreibung, FruehestmoeglicherAnfang, FruehestmoeglichesEnde, SpaetmoeglichsterAnfang, SpaetmoeglichstesEnde, Arbeitspaket_InsgeArbeitszeit, Arbeitspaket_Zustaendiger) VALUES (@Arbeitspaket_ID, @Arbeitspaket_Name, @Projekt_ID, @Arbeitspaket_Beschreibung, @FruehestmoeglicherAnfang, @FruehestmoeglichesEnde, @SpaetmoeglichsterAnfang, @SpaetmoeglichstesEnde, @Arbeitspaket_InsgeArbeitszeit, @Arbeitspaket_Zustaendiger)", _connection);
                command.Parameters.AddWithValue("@Arbeitspaket_ID", task.Arbeitspaket_ID);
                command.Parameters.AddWithValue("@Arbeitspaket_Name", task.Arbeitspaket_Name);
                command.Parameters.AddWithValue("@Projekt_ID", task.Projekt_ID);
                command.Parameters.AddWithValue("@Arbeitspaket_Beschreibung", task.Arbeitspaket_Beschreibung);
                command.Parameters.AddWithValue("@FruehestmoeglicherAnfang", "1905-07-11 00:00:00.000");
                command.Parameters.AddWithValue("@FruehestmoeglichesEnde", "1905-07-11 00:00:00.000");
                command.Parameters.AddWithValue("@SpaetmoeglichsterAnfang", "1905-07-11 00:00:00.000");
                command.Parameters.AddWithValue("@SpaetmoeglichstesEnde", "1905-07-11 00:00:00.000");
                command.Parameters.AddWithValue("@Arbeitspaket_InsgeArbeitszeit", "40");
                command.Parameters.AddWithValue("@Arbeitspaket_Zustaendiger", task.Arbeitspaket_Zustaendiger);
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
                SqlCommand command = new SqlCommand("INSERT INTO Arbeitssitzung (ArbeitsSitzung_ID, Arbeitspaket_ID, Ersteller_ID,  ArbeitsSitzung_Dauer) VALUES (@ArbeitsSitzung_ID, @Arbeitspaket_ID, @Ersteller_ID,  @ArbeitsSitzung_Dauer)", _connection);
                command.Parameters.AddWithValue("@ArbeitsSitzung_ID", workSession.Arbeitssitzung_ID);
                command.Parameters.AddWithValue("@Arbeitspaket_ID", workSession.Arbeitspaket_ID);
                command.Parameters.AddWithValue("@Ersteller_ID", workSession.Ersteller_ID); 
                command.Parameters.AddWithValue("@ArbeitsSitzung_Dauer", workSession.Arbeitszeit);
             
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }   
        public void InsertProjektNutzer(Project project, User userid)
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO ProjektNutzer (Projekt_ID, Nutzer_ID) VALUES (@Projekt_ID, @Nutzer_ID)", _connection);
                command.Parameters.AddWithValue("@Projekt_ID", project.Projekt_ID);
                command.Parameters.AddWithValue("@Nutzer_ID", userid.Nutzer_ID);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }  
        public void InsertProjektAdmin(Project project, User userid)
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO AdminProjekt (Projekt_ID, Nutzer_ID) VALUES (@Projekt_ID, @Nutzer_ID)", _connection);
                command.Parameters.AddWithValue("@Projekt_ID", project.Projekt_ID);
                command.Parameters.AddWithValue("@Nutzer_ID", userid.Nutzer_ID);
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
                SqlCommand command = new SqlCommand("INSERT INTO Kommentar (Kommentar_ID, Ersteller_ID, ArbeitsSitzung_ID, Kommentar_Text) VALUES (@Kommentar_ID, @Ersteller_ID, @ArbeitsSitzung_ID, @Kommentar_Text )", _connection);
                command.Parameters.AddWithValue("@Kommentar_ID", comment.Kommentar_ID);
                command.Parameters.AddWithValue("@Ersteller_ID", comment.Ersteller_ID);
                command.Parameters.AddWithValue("@ArbeitsSitzung_ID", comment.Arbeitspaket_ID);
                command.Parameters.AddWithValue("@Kommentar_Text", comment.Inhalt);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }

        public void InsertKategorie(Kategorie kategorie)
        {             try
            {
                SqlCommand command = new SqlCommand("INSERT INTO Kategorie (Kategorie_ID, Name) VALUES (@Kategorie_ID, @Kategorie_Name)", _connection);
                command.Parameters.AddWithValue("@Kategorie_ID", kategorie.Kategorie_ID);
                command.Parameters.AddWithValue("@Kategorie_Name", kategorie.Name);
                command.ExecuteNonQuery();
            }
                       catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }  
        
        public void InsertArbeitspaketKategorie(Objects.Task task, Kategorie kategorie)
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO ArbeitspaketKategorie (Arbeitspaket_ID, Kategorie_ID) VALUES (@Arbeitspaket_ID, @Kategorie_ID)", _connection);
                command.Parameters.AddWithValue("@Arbeitspaket_ID", task.Arbeitspaket_ID);
                command.Parameters.AddWithValue("@Kategorie_ID", kategorie.Kategorie_ID);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }   

       public void InsertRelation(Objects.Task Pre, Objects.Task After )
        {
            try
            {
                SqlCommand command = new SqlCommand("INSERT INTO Relation (Vorgaenger_ID, Nachfolger_ID) VALUES (@Vorgaenger_ID, @Nachfolger_ID)", _connection);
                command.Parameters.AddWithValue("@Vorgaenger_ID", Pre.Arbeitspaket_ID);
                command.Parameters.AddWithValue("@Nachfolger_ID", After.Arbeitspaket_ID);
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, "red");
            }
        }   


    }
}
