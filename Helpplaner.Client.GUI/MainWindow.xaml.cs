using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Helpplaner.Client.GUI.Pages;
using Helpplaner.Service.Objects;
using Helpplaner.Service.Shared;

namespace Helpplaner.Client.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        User user;
        public Login login;
        public MainWindow()
        {


            InitializeComponent();
            user = new User();  

            ServerCommunicator sc = new ServerCommunicator(new ConsoleLogger());

            login = new Login(sc, ref user);
            login.Userfound += Login_Userfound;
            Main.Content = login;


        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Login_Userfound(object sender, EventArgs e)
        {
            Main.Content = null;
            user = (User)sender;    

            Username.Text = user.Nutzernamen;   
        }   
    }
}