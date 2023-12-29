using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpplaner.Service.Objects
{
    internal class Task
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
    }
}
