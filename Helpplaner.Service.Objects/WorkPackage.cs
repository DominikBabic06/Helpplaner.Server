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

        public string ReponsibleName { get; set; }  

        public string Dependecy { get; set; }

        public string Successor { get; set; }   

        public string Status { get; set; }

        public string colorStatus { get; set; } 
         
        public List<WorkSession> ArbeitsSitzungs { get; set; } = new List<WorkSession>();

        public override string ToString()
        {
            return
             Name + $"({ID})";  
        
                
        }
    }
}
