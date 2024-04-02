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
        string remeber;
        public Login( ServerCommunicator server , ref User user)
        {
            InitializeComponent();
            this.server  = server;
            if (!File.Exists("UserData/remember.txt"))
            {
             
                Directory.CreateDirectory("UserData");
                File.Create("UserData/remember.txt");
                Console.WriteLine("Ordner erstellt: " + "UserData/remember.txt");
            }
            else
            {
                Console.WriteLine("Ordner existiert bereits: " + "UserData/remember.txt");
            }
            StreamReader sr = new StreamReader("UserData/remember.txt");
            remeber = sr.ReadLine();  
            sr.Close(); 
            if(!String.IsNullOrEmpty(remeber))
            {
                Password.Password = remeber.Split(";")[1];
                User.Text = remeber.Split(";")[0];  

            }   


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            password = Password.Password;   
            username = User.Text;
            if(RememberMe.IsChecked == true)
            {
                StreamWriter sw = new StreamWriter("UserData/remember.txt");
                sw.WriteLine(username + ";" + password);
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
