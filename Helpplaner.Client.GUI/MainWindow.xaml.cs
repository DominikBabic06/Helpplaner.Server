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
        Project[] adminprojects;
        Project selectetProj;
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
            foreach (MenuItem item in Projekte.Items)
            {
                

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
            adminprojects = sc.GetAdminProjekts();  
          


            foreach (Project item in projects)
            {
                if (IsAdmin(item) )
                {
                    Button btn = new Button();  
                    btn.Content = "Admin" + item.Projekt_Name + " (" + item.Projekt_ID + ")";
                    btn.Template = (ControlTemplate)FindResource("ReloadButton");
                    btn.Click += MenuButtonClick;
                    Projekte.Items.Add( btn);
                }
                else
                {
                    Button btn = new Button();
                    btn.Content = item.Projekt_Name + " (" + item.Projekt_ID + ")";
                    btn.Template = (ControlTemplate)FindResource("ReloadButton");
                    btn.Click += MenuButtonClick; 
                    Projekte.Items.Add(btn);
                }
               
            }

            



        }

        private void MenuButtonClick(object sender, RoutedEventArgs e)
        {
           Button btn =  (Button)sender;
            string[] split = btn.Content.ToString().Split('(');
            string id = split[1].Trim(')');
            foreach (Project item in projects)
            {
                if (item.Projekt_ID == id)
                {
                    selectetProj = item;
                }
            }
            Useroverview useroverview = new Useroverview(selectetProj, sc);
            Main.Content = useroverview;
            //MessageBox.Show(id);  

        }

        public bool IsAdmin(Project project)
        {
            foreach (Project item in adminprojects)
            {
                if (item.Projekt_ID == project.Projekt_ID)
                {
                  return true;
                }
            }   
          return false; 
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

       

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            sc.Logout();
            user = null;
            login = new Login(sc, ref user);
            login.Userfound += Login_Userfound;
            sc.ServerMessage += Sc_ServerMessage;
            Projekte.Items.Clear();
           
            Main.Content = login;
        }
    }
}