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

namespace Helpplaner.Client.GUI.Pages
{
    /// <summary>
    /// Interaktionslogik für ProjektHub.xaml
    /// </summary>
    public partial class ProjectHub : Page
    {
        ServerCommunicator SC;
        ProjectViewModel pv;
        MainWindow mw;

        public ProjectHub(ServerCommunicator SC, ProjectViewModel pv, MainWindow mw)
        {
            InitializeComponent();
            this.SC = SC;
            this.pv = pv;
            this.mw = mw;
            this.DataContext = pv;  
            cbAktiveProjekte.IsChecked = true;  

        }
        public void  changeShown()

        {
            var search = Search.Text;
            if (search == "")
            {
                if (cbAktiveProjekte.IsChecked == false)
                    Projekts.ItemsSource = pv.projects;
                else
                {
                    Projekts.ItemsSource = pv.projects.Where(p => p.Active == true);
                }
            }
            else
            {
                if (cbAktiveProjekte.IsChecked == false)
                    Projekts.ItemsSource = pv.projects.Where(p => p.Name.Contains(search));
                else
                    Projekts.ItemsSource = pv.projects.Where(p => p.Name.Contains(search) && p.Active == true);
            }
        }

        private void cbAktiveProjekte_Checked(object sender, RoutedEventArgs e)
        {
            
            changeShown();
        }

        private void cbAktiveProjekte_Unchecked(object sender, RoutedEventArgs e)
        {
          
                changeShown();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            changeShown ();
        }
    }
}




