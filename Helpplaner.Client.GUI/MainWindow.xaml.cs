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
using Helpplaner.Client.GUI.SVG;
using Helpplaner.Service.Objects;
using Helpplaner.Service.Shared;

namespace Helpplaner.Client.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool connected = false; 
        User user;
        public Login login;
        public NotImplemented nt;
        ServerCommunicator sc;
        Project[] projects;
        Project[] adminprojects;
        Project selectetProj;
        NoPlug noplug = new NoPlug();   
        Plug plug = new Plug();
        APÜbersicht aPÜbersicht;
        ProjectViewModel pvm;
        ProjektOverview ProjektOverview;
        ProjectHub ProjektHub;

        
        public MainWindow()
        {


            InitializeComponent();
            nt = new NotImplemented();
            user = new User();  
            pvm = new ProjectViewModel();
             sc = new ServerCommunicator(new ConsoleLogger());

            //Thread thread = new Thread(Request_Info);
            //thread.IsBackground = true;
            //thread.Start();


            login = new Login(sc, ref user);
            login.Userfound += Login_Userfound;
            sc.ServerMessage += Sc_ServerMessage;
            Main.Content = login;
            //Leagecy Shit 
            //UserIconu.Source = new Uri(@"SVG/Leave.xaml", UriKind.Relative);
            //Leave.Source = new Uri(@"SVG/UserIcon.xaml", UriKind.Relative);
            //DiagrammIcon.Source = new Uri(@"SVG/Diagramm.xaml", UriKind.Relative);
            //StatsIcon.Source = new Uri(@"SVG/Stats.xaml", UriKind.Relative);
            //APIcon.Source = new Uri(@"SVG/Database.xaml", UriKind.Relative);

            CheckServer();


            Thread checkThread = new Thread(CheckServerRepeating); 
            checkThread.IsBackground = true;
          
            checkThread.Start();
            
           


        }

        public void CheckServerRepeating()
        {
          
            while (true)
            {
                Thread.Sleep(1000);
                CheckServer();
            }
        }   

        private  void CheckServer()
        {
            
          connected = sc.tryConnect();
           
            if (connected)
            {


                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate () { ConnectionEstablished.Content = plug; });
                if (login.inputblocked)
                {
                   Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate () { login.Warning.Content = ""; });
                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate () { ConnectionEstablished.Source = new Uri(@"SVG/Plug.xaml", UriKind.Relative); });
                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate () { login.UnblockInput(); }); 
              
                }
                if(selectetProj != null)
                if (sc.NeedsToBeReloaded(int.Parse(selectetProj.ID)))
                {
                    pvm.Tasks = sc.GetTasksforProject(Convert.ToInt32(selectetProj.ID));
                    pvm.users = sc.GetUsersforProject(Convert.ToInt32(selectetProj.ID));
                        pvm.BindUsersToTasks();
                        if (aPÜbersicht != null)
                        {
                            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate () { aPÜbersicht.Reload(); });

                        }


                    } 

                
            }
            else
            {
               
                
                if (!login.inputblocked)
                {
                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate () { login.Warning.Content = "Server is offline"; });
                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate () { ConnectionEstablished.Source = new Uri(@"SVG/NoPlug.xaml", UriKind.Relative); });

                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate () { login.BlockInput(); });    
                }
            }   
           

        }   

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Login_Userfound(object sender, EventArgs e)
        {
            projects = sc.GetProjectsforUser();
            adminprojects = sc.GetAdminProjekts();
            pvm.projects = projects;

            ProjektHub = new ProjectHub(sc, pvm, this);
            Main.Content = ProjektHub;
            user = (User)sender;    

            Username.Text = user.Username;


           
          


            foreach (Project item in projects)
            {
                if (IsAdmin(item) )
                {
                    Button btn = new Button();  
                    btn.Content = "Admin" + item.Name + " (" + item.ID + ")";
                    btn.Template = (ControlTemplate)FindResource("ReloadButton");
                    btn.Click += MenuButtonClick;
                    Projekte.Items.Add( btn);
                }
                else
                {
                    Button btn = new Button();
                    btn.Content = item.Name + " (" + item.ID + ")";
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
                if (item.ID == id)
                {
                    selectetProj = item;
                }
            }

            pvm.users = sc.GetUsersforProject(Convert.ToInt32(selectetProj.ID));
            pvm.Tasks = sc.GetTasksforProject(Convert.ToInt32(selectetProj.ID));
            pvm.BindUsersToTasks();
            Useroverview useroverview = new Useroverview(selectetProj, sc, pvm  );

            APu.Visibility = Visibility.Visible;
            Pro.Visibility = Visibility.Visible;    
            Dia.Visibility = Visibility.Visible;

            Main.Content = useroverview;
            //MessageBox.Show(id);  

        }

        public bool IsAdmin(Project project)
        {
            foreach (Project item in adminprojects)
            {
                if (item.ID == project.ID)
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


            APu.Visibility = Visibility.Hidden;
            Pro.Visibility = Visibility.Hidden;
            Dia.Visibility = Visibility.Hidden;
            Main.Content = login;
        }


        private void NotImplemented_Click(object sender, RoutedEventArgs e)
        {
           
            Main.Content = nt;
        }

   

        private void ApOverView_Click(object sender, RoutedEventArgs e)
        {
             aPÜbersicht = new APÜbersicht(selectetProj, sc, pvm, this);    

            Main.Content = aPÜbersicht;
        }

        private void Dia_MouseEnter(object sender, MouseEventArgs e)
        {
               DiagramIcon.Stroke = Brushes.Black;  
               DiagramIcon.Fill = FindResource("BackgroundColorTextHighlight") as Brush;    
        }

        private void Dia_MouseLeave(object sender, MouseEventArgs e)
        {
            DiagramIcon.Stroke = Brushes.Black;
                DiagramIcon.Fill = FindResource("ColorText") as Brush;
        }


        private void Pro_MouseEnter(object sender, MouseEventArgs e)
        {
            StatsIcon.Stroke = Brushes.Black;
            StatsIcon.Fill = FindResource("BackgroundColorTextHighlight") as Brush;
        }

        private void Pro_MouseLeave(object sender, MouseEventArgs e)
        {
            StatsIcon.Stroke = Brushes.Black;
            StatsIcon.Fill = FindResource("ColorText") as Brush;
        }

        private void AB_MouseEnter(object sender, MouseEventArgs e)
        {
            DiagramIcon.Stroke = FindResource("BackgroundColorTextHighlight") as Brush;
            
        }

        private void Ab_MouseLeave(object sender, MouseEventArgs e)
        {
            Database1.Stroke = FindResource("ColorText") as Brush;
          
        }


        public void changeShowPage(Page page)
        {
           Dispatcher.Invoke( System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate () { Main.Content = page; });
        }   
    }
}