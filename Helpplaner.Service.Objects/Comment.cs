using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpplaner.Service.Objects
{
    public class Comment
    {
      public  string Kommentar_ID { get; set; } 
      public  string Ersteller_ID { get; set; } 
      
      public  string Arbeitspaket_ID { get; set; }  

      public  string Inhalt { get; set; }   


    }
}
