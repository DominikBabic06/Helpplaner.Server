namespace Helpplaner.Service.Shared
{
    public interface IServiceLogger
    {
        
        public void Log(string message, string color);    
       public string GetInfo(string message);
    }
}