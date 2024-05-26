using System.Net;

namespace Helpplaner.Service.Core
{
    public class Config
    {
        public string ConnectionString { get; set; }
        public int MaxConnections { get; set; }

        public string _ipAddress;
        public int _port;


        public Config config;

        public Config()
        {
            if (File.Exists("../../../../config.txt"))
            {
                string[] configs = File.ReadAllLines("../../../../config.txt");

                ConnectionString = configs.Where(x => x.Contains("ConnectionString")).FirstOrDefault().Trim(' ').Split(":")[1];
                MaxConnections = int.Parse(configs.Where(x => x.Contains("MaxConnections")).FirstOrDefault().Trim(' ').Split(":")[1]);
                _ipAddress = configs.Where(x => x.Contains("IPAddress")).FirstOrDefault().Trim(' ').Split(":")[1];
                _port = int.Parse(configs.Where(x => x.Contains("Port")).FirstOrDefault().Split(":")[1]);




            }
            else
            {
               

                File.AppendAllLines("../../../../config.txt", new string[] { "ConnectionString: ", "MaxConnections:10 ", "IPAddress:127.0.0.1", "Port:5000" });

            }
        }






    }
}
