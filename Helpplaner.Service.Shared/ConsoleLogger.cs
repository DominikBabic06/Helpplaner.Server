using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpplaner.Service.Shared
{
    public class ConsoleLogger : IServiceLogger
    {
      public  void Log(string message, string color)
        { 
        switch (color)
            {
            case "red":
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            case "green":
                Console.ForegroundColor = ConsoleColor.Green;
                break;
            case "yellow":
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case "blue":
                Console.ForegroundColor = ConsoleColor.Blue;
                break;
            case "magenta":
                Console.ForegroundColor = ConsoleColor.Magenta;
                break;
            case "cyan":
                Console.ForegroundColor = ConsoleColor.Cyan;
                break;
            case "white":
                Console.ForegroundColor = ConsoleColor.White;
                break;
            default:
                Console.ForegroundColor = ConsoleColor.White;
                break;
        }
        Console.WriteLine(message);
        Console.ResetColor();
        }
        public string GetInfo(string message)
        {
            return Console.ReadLine();
        }
    }
}
