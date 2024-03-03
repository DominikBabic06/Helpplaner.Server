namespace Helpplaner.Service.Objects
{
    using System.Runtime.Serialization.Formatters.Binary;
    [Serializable]
    public class User
    {

        public User() { }   
        public string Nutzer_ID { get; set; }  
        public string Nutzer_Passwort { get; set; }
        public string Nutzernamen { get; set; }    
        public string Email { get; set; }   



        public override string ToString()
        {
            return $"{Nutzer_ID}, {Nutzernamen}" ;
        }
    
    }
}