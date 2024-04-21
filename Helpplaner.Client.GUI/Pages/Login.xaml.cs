using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Helpplaner.Service.Objects;   
using Helpplaner.Service.Shared;    
using System.Security.Cryptography;
using System.IO;


namespace Helpplaner.Client.GUI.Pages
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public bool inputblocked = false;   
        ServerCommunicator server;
        string username;
        string password;
        User user;
        string[] remeber;
        Aes Crypt;
        public Login( ServerCommunicator server , ref User user)
        {
            InitializeComponent();
            this.server  = server;
            Crypt  = Aes.Create();
            if (!File.Exists("UserData/remember.txt"))
            {
             
                Directory.CreateDirectory("UserData");
                File.Create("UserData/remember.txt").Close();
                Console.WriteLine("Ordner erstellt: " + "UserData/remember.txt");
            }
            else
            {
                Console.WriteLine("Ordner existiert bereits: " + "UserData/remember.txt");
            }

            remeber = File.ReadLines("UserData/remember.txt").ToArray();

            if (!String.IsNullOrEmpty(remeber[0]))
            {
                if (remeber[1].Contains("hashed:"))
                {
                    User.Text = remeber[0];
                    Password.Password = "DummyPassowrd";
                    RememberMe.IsChecked = true;
                }

            }   


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(remeber[1]))
                password = HashPassword(Password.Password);
            else
                password = remeber[1].Replace("hashed:","").Trim(' ');
            
            username = User.Text;
            if(RememberMe.IsChecked == true)
            {
                StreamWriter sw = new StreamWriter("UserData/remember.txt");
                sw.WriteLine(username);
                sw.WriteLine("hashed: " + password);
                sw.Close();  
            }
            else
            {
                StreamWriter sw = new StreamWriter("UserData/remember.txt");
                sw.WriteLine("");
                sw.Close();

            }
            User userlog = server.TryLogin(username, password);
            if (userlog == null)
            {
                Warning.Content= "Login failed";
                return;
            }
            user = userlog;    
            OnUserfound();  

        }

        public string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public event EventHandler Userfound;

        public void BlockInput()
        {
            User.IsEnabled = false;
            Password.IsEnabled = false;
            LoginButton.IsEnabled = false;
            inputblocked = true;    
        }

        public void UnblockInput()
        {
            User.IsEnabled = true;
            Password.IsEnabled = true;
            LoginButton.IsEnabled = true;
            inputblocked = false;
        }    
        
        public void ChangeWarning(string warning)
        {
            Warning.Content = warning;   
        }   


        protected virtual void OnUserfound()
        {
            if (Userfound != null)
            {
                Userfound(user, EventArgs.Empty);
            }
        }   



    }
}
