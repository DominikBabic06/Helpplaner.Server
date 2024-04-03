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
        
        public string ID { get; set; }   
        public string Name { get; set; } 
        public string Description { get; set; }  

        public List<User> Users { get; set; } = new List<User>();

        public List<User> AdminUser { get; set; } = new List<User>();  
            
       

                      
        public List<WorkPackage> Tasks{ get; set; } = new List<WorkPackage>();

        public 
         override string ToString()
        {
            return "Projekt_ID: " + ID + "\n" +
                    "Projekt_Name: " + Name + "\n" +
                    "Projekt_Beschreibung: " + Description + "\n";
        }
        public  Project() { 
        
        
        }   

    }
}
