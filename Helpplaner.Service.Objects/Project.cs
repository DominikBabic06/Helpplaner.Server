using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpplaner.Service.Objects
{
    internal class Project
    {
        
        public string ProjectId { get; set; }   
        public string ProjectName { get; set; } 
        public string ProjectDescription { get; set; }  

        public List<User> Users { get; set; } = new List<User>();

                      
        public List<Task> Tasks { get; set; } = new List<Task>();   
        Project() { 
        
        
        }   

    }
}
