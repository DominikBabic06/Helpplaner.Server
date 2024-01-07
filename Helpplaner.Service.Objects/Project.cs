using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpplaner.Service.Objects
{
    [Serializable]
    public class Project
    {
        
        public string Projekt_ID { get; set; }   
        public string Projekt_Name { get; set; } 
        public string Projekt_Beschreibung { get; set; }  

        public List<User> Users { get; set; } = new List<User>();

        public List<User> AdminUser { get; set; } = new List<User>();  
            
       

                      
        public List<Task> Tasks{ get; set; } = new List<Task>();

        public 
         override string ToString()
        {
            return "Projekt_ID: " + Projekt_ID + "\n" +
                    "Projekt_Name: " + Projekt_Name + "\n" +
                    "Projekt_Beschreibung: " + Projekt_Beschreibung + "\n";
        }
        public  Project() { 
        
        
        }   

    }
}
