using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpplaner.Service.Objects
{
    public class Task
    {
        public int Arbeitspaket_ID { get; set; }
        public string Arbeitspaket_Name { get; set; }
        public int Projekt_ID { get; set; }
        public string Arbeitspaket_Beschreibung { get; set; }
        public DateTime FruehestmoeglicherAnfang { get; set; }
        public DateTime FruehestmoeglichesEnde { get; set; }
        public DateTime SpaetmoeglichsterAnfang { get; set; }
        public DateTime SpaetmoeglichstesEnde { get; set; }
        public int Arbeitspaket_InsgeArbeitszeit { get; set; }
        public string Arbeitspaket_Zustaendiger { get; set; }

        public List<string> KommentarIDs { get; set; } = new List<string>();    
        public List<string> ArbeitsSitzungsIDs { get; set; } = new List<string>();

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
