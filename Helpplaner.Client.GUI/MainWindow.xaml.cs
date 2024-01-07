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
        ServerCommunicator sc;
        Project[] projects;
        public MainWindow()
        {


            InitializeComponent();
            user = new User();  

             sc = new ServerCommunicator(new ConsoleLogger());

            //Thread thread = new Thread(Request_Info);
            //thread.IsBackground = true;
            //thread.Start();


            login = new Login(sc, ref user);
            login.Userfound += Login_Userfound;
            sc.ServerMessage += Sc_ServerMessage;
            Main.Content = login;


        }


        private void Request_Info()
        {
           
            if (user != null)
            {
                projects = sc.GetProjectsforUser();
            }
            
            
            foreach (Project item in projects)
            {
                Projekte.Items.Add(item.Projekt_Name + " (" + item.Projekt_ID + ")");
            }

        }   

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Login_Userfound(object sender, EventArgs e)
        {
            Main.Content = null;
            user = (User)sender;    

            Username.Text = user.Nutzernamen;


            projects = sc.GetProjectsforUser();
            foreach (Project item in projects)
            {
                Projekte.Items.Add(item.Projekt_Name +" (" + item.Projekt_ID + ")");
            }

            



        }
        private void Sc_ServerMessage(object sender, string e)
        {
            MessageBox.Show(e);
        }

        private void Reaload_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("test");    
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}