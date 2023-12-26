// See https://aka.ms/new-console-template for more information

using Helpplaner.Service.Core;
using Helpplaner.Service.Shared;
class Program
{
    static void Main(string[] args)
    {
        ConsoleLogger cl = new ConsoleLogger();
        Server server = new Server(cl);
        
        string input;
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
                default:
                    cl.Log("Invalid command", "red");
                    break;
            }   

        } while (input != "exit");
        server.Stop();  
        

    }
}