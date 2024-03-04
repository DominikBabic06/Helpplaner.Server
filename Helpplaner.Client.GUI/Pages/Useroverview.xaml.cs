using Helpplaner.Service.Objects;
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
    /// Interaktionslogik für Useroverview.xaml
    /// </summary>
    public partial class Useroverview : Page
    {
        Project Project;
        User[] User;
        ServerCommunicator sr;
        ProjectViewModel pvm;   
        public Useroverview(Project proj, ServerCommunicator sr,ProjectViewModel pvm )
        {
            InitializeComponent();
            Project = proj;
            this.sr = sr;
            this.pvm = pvm;
            this.DataContext = pvm; 
            FillUser();

        }

        private void FillUser()
        {
            User = sr.GetUsersforProject(Convert.ToInt32(Project.Projekt_ID));
            Users.ItemsSource = User;


        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
