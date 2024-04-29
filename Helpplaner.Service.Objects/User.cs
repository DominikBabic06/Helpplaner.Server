namespace Helpplaner.Service.Objects
{
    using System.Runtime.Serialization.Formatters.Binary;
    [Serializable]
    public class User
    {

        public User() { }   
        public string ID { get; set; }  
        public string Password { get; set; }
        public string Username { get; set; }    
        public string Email { get; set; }   


        /// <summary>
        /// asdada
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{ID}, {Username}" ;
        }
    
    }
}