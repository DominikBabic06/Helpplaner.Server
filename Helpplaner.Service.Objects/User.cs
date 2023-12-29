namespace Helpplaner.Service.Objects
{
    public class User
    {
        public User() { }   
        public string Id { get; set; }  
        public string Username { get; set; }
        public string Password { get; set; }    
        public string Email { get; set; }   

        public override string ToString()
        {
            return $"Id: {Id}, Username: {Username}, Password: {Password}, Email: {Email}";
        }
    
    }
}