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
            Users.DataContext = pvm;    
         

        }

        private void FillUser()
        {
            User = sr.GetUsersforProject(Convert.ToInt32(Project.ID));
          


        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PopupButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserHinzufügen uh = new UserHinzufügen(Project, sr, pvm);    
            uh.ShowDialog();    

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void AP_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void AP_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }


        public void Reload_Click()
        {
            Users.DataContext = null;
            Users.DataContext = pvm;

        }   
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (SearchBox.Text != "" )
            {
                Users.ItemsSource = pvm.users.Where(x => x.Username.Contains(SearchBox.Text));  
            }
            else
            {
                Users.ItemsSource = pvm.users; 
            }   
        }
    }
}
