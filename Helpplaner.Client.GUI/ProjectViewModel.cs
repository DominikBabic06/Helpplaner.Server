using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Helpplaner.Service.Objects;



namespace Helpplaner.Client.GUI
{
   public class ProjectViewModel 

    {

        public ProjectViewModel()
        { }


        public IEnumerable<Service.Objects.Task> Tasks { get; set; }

        public IEnumerable<User> users { get; set; }

        
        public int getFirstFreeIDForTask()
        {
            int id = 0;
            foreach (var task in Tasks)
            {
                if (Convert.ToInt32(task.Arbeitspaket_ID) > id)
                {
                    id = Convert.ToInt32(task.Arbeitspaket_ID);
                }
            }
            return id + 1;
        }   
    }
}
