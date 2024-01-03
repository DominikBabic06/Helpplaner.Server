namespace Helpplaner.Service.Objects
{
    public class User
    {
        public User() { }   
        public string Nutzer_ID { get; set; }  
        public string NutzerPasswort { get; set; }
        public string Nutzernamen { get; set; }    
        public string Email { get; set; }   



        public override string ToString()
        {
            return $"Id: {Nutzer_ID}, Username: {Nutzernamen}, Password: {NutzerPasswort}, Email: {Email}";
        }
    
    }
}