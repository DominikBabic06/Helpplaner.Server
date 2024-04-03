using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpplaner.Service.Objects
{
    [Serializable]
    public class WorkSession
    {
        public WorkSession() { }    
        public string ID { get; set; }   
        public string WorkPackageID { get; set; }
        public string CreatorID { get; set; }

        public string WorkTime { get; set; }   

        public List<Comment> Kommentare { get; set; } = new List<Comment>();    

        public override string ToString()
        {
            return
             "Arbeitssitzung_ID: " + ID + "\n" +
             "Arbeitspaket_ID: " + WorkPackageID + "\n" +
             "Ersteller_ID: " + CreatorID + "\n" +
             "Arbeitszeit: " + WorkTime + "\n"; 
        
        
                
        }   

    }
}
