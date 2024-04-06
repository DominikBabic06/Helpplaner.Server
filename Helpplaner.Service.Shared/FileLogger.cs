using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Helpplaner.Service.Shared
{
    internal class FileLogger : IServiceLogger
    {
        public string GetInfo(string message)
        {
            throw new NotImplementedException();
        }

        public void Log(string message, string color)
        {
            StreamWriter sw = new StreamWriter("log.txt", true);    
            sw.WriteLine(message);
            sw.Close();

         
        }
    }
}
