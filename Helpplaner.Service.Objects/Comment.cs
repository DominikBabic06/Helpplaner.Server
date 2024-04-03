using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpplaner.Service.Objects
{
    [Serializable]
    public class Comment
    {
      public  string ID { get; set; } 
      public  string CreatorID { get; set; } 
      
      public  string WorkPackageID { get; set; }  

      public  string Text { get; set; }   


    }
}
