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
        public Useroverview(Project proj , ServerCommunicator sr)
        {
            InitializeComponent();
            Project = proj; 
            
        }

        private void FillUser()
        {
            foreach (User item in Project.Users)
            {
              
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
