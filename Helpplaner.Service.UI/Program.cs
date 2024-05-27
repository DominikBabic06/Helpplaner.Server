// See https://aka.ms/new-console-template for more information

using Helpplaner.Service.Core;
using Helpplaner.Service.Shared;
using System.Net;

class Program
{
    //hurensohn
    static void Main(string[] args)
    {
        ConsoleLogger cl = new ConsoleLogger();
        Config config = new Config();  
        IPAddress ip;   

        if (IPAddress.TryParse(config._ipAddress, out ip) && config.rightConfig)
        {
          
       
        Server server = new Server(cl, config._ipAddress, config._port);
        
        string input;
       /// server.Start();
        do
        {
            
           cl.Log("Type 'start' to start the server, 'stop' to stop the server" , "white");    
            input = cl.GetInfo("Enter command: ");  
            switch (input)
            {
                case "start":
                    if (server.isRunning)
                    {
                        cl.Log("Server is already running", "red");
                        break;
                    }
                    server.Start();
                    
                    break;
                case "stop":
                    if (!server.isRunning)
                    {
                        cl.Log("Server is not running", "red");
                        break;
                    }
                    server.Stop();
                    break;
                case "restart":
                    if (!server.isRunning)
                    {
                        cl.Log("Server is not running", "red");
                        break;
                    }
                    server.Restart();   
                    break;
                case "user":
               
                    server.GiveAllUsers();  
                    break;
                case "userid":
                    int id = int.Parse(cl.GetInfo("Enter id: "));
                    server.GiveUser(id);
                    break;
                case "Projektid":
                    int Projid = int.Parse(cl.GetInfo("Enter id: "));
                    server.GiveProject(Projid); 
                    break;  
                default:
                    cl.Log("Invalid command", "red");
                    break;
            }   

        } while (input != "exit");
        server.Stop();
        }
        else
        {
            Console.WriteLine("Invalid IP address or config file"); 
        }

    }
}