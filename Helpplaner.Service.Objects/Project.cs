using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpplaner.Service.Objects
{
    public class Project
    {
        
        public string Projekt_ID { get; set; }   
        public string Projekt_Name { get; set; } 
        public string Projekt_Beschreibung { get; set; }  

        public List<string> UserIDs { get; set; } = new List<string>();

        public List<string> AdminUserIDs { get; set; } = new List<string>();  
            
        public List<string> KategorieIDs { get; set; } = new List<string>();    

                      
        public List<string> TaskIDs { get; set; } = new List<string>();   
      public  Project() { 
        
        
        }   

    }
}
