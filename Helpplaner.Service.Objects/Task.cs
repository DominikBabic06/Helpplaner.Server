using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpplaner.Service.Objects
{
    public class Task
    {
        public Task() { }  
        public string Arbeitspaket_ID { get; set; }
        public string Arbeitspaket_Name { get; set; }
        public string Projekt_ID { get; set; }
        public string Arbeitspaket_Beschreibung { get; set; }
        public string FruehestmoeglicherAnfang { get; set; }
        public string FruehestmoeglichesEnde { get; set; }
        public string SpaetmoeglichsterAnfang { get; set; }
        public string SpaetmoeglichstesEnde { get; set; }
        public string Arbeitspaket_InsgeArbeitszeit { get; set; }
        public string Arbeitspaket_Zustaendiger { get; set; }

         
        public List<WorkSession> ArbeitsSitzungs { get; set; } = new List<WorkSession>();

        public override string ToString()
        {
            return
             "Arbeitspaket_ID: " + Arbeitspaket_ID + "\n" +
             "Arbeitspaket_Name: " + Arbeitspaket_Name + "\n" +
             "Projekt_ID: " + Projekt_ID + "\n" +
             "Arbeitspaket_Beschreibung: " + Arbeitspaket_Beschreibung + "\n" +
             "FruehestmoeglicherAnfang: " + FruehestmoeglicherAnfang + "\n" +
             "FruehestmoeglichesEnde: " + FruehestmoeglichesEnde + "\n" +
             "SpaetmoeglichsterAnfang: " + SpaetmoeglichsterAnfang + "\n" +
             "SpaetmoeglichstesEnde: " + SpaetmoeglichstesEnde + "\n" +
             "Arbeitspaket_InsgeArbeitszeit: " + Arbeitspaket_InsgeArbeitszeit + "\n" +
             "Arbeitspaket_Zustaendiger: " + Arbeitspaket_Zustaendiger + "\n"; 
        
                
        }
    }
}
