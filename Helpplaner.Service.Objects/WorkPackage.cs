using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpplaner.Service.Objects
{
    [Serializable]
    public class WorkPackage
    {
        public WorkPackage() { }  
        
        public string ID { get; set; }
        public string Name { get; set; }
        public string ProjectID { get; set; }
        public string Description { get; set; }

        public string ExpectedTime { get; set; }

        public string RealTime { get; set; }

        public string Responsible { get; set; }

         
        public List<WorkSession> ArbeitsSitzungs { get; set; } = new List<WorkSession>();

        public override string ToString()
        {
            return
             "Arbeitspaket_ID: " + ID + "\n" +
             "Arbeitspaket_Name: " + Name + "\n" +
             "Projekt_ID: " + ProjectID + "\n" +
             "Arbeitspaket_Beschreibung: " + Description + "\n" +
     
             "Arbeitspaket_InsgeArbeitszeit: " + ExpectedTime + "\n" +
             "Arbeitspaket_Zustaendiger: " + Responsible + "\n"; 
        
                
        }
    }
}
