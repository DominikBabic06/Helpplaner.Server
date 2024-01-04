using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpplaner.Service.Objects
{
    public class WorkSession
    {
        public WorkSession() { }    
        public string Arbeitssitzung_ID { get; set; }   
        public string Arbeitspaket_ID { get; set; }
        public string Ersteller_ID { get; set; }

        public string Arbeitszeit { get; set; }   

        public List<Comment> Kommentare { get; set; } = new List<Comment>();    

        public override string ToString()
        {
            return
             "Arbeitssitzung_ID: " + Arbeitssitzung_ID + "\n" +
             "Arbeitspaket_ID: " + Arbeitspaket_ID + "\n" +
             "Ersteller_ID: " + Ersteller_ID + "\n" +
             "Arbeitszeit: " + Arbeitszeit + "\n"; 
        
        
                
        }   

    }
}
