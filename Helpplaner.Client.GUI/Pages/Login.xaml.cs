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

namespace Helpplaner.Client.GUI.Pages
{
    /// <summary>
    /// Interaktionslogik für Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        ServerCommunicator server;
        string username;
        string password;
        User user; 
        public Login( ServerCommunicator server , ref User user)
        {
            InitializeComponent();
            this.server  = server;   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            password = Password.Password;   
            username = User.Text;   
            User userlog = server.TryLogin(username, password);
            if (userlog == null)
            {
                MessageBox.Show("Login failed");
                return;
            }
            user = userlog;    
            OnUserfound();  

        }

        public event EventHandler Userfound;


        protected virtual void OnUserfound()
        {
            if (Userfound != null)
            {
                Userfound(user, EventArgs.Empty);
            }
        }   



    }
}
