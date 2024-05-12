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
    /// Interaktionslogik für ProjektOverview.xaml
    /// </summary>
    public partial class ProjektOverview : Page
    {

        ProjectViewModel pvm;   
        Project project;    
        ServerCommunicator sr;
        MainWindow mw;
        
        public ProjektOverview(Project proj, ServerCommunicator sr, ProjectViewModel pvm,MainWindow mw)
        {
            InitializeComponent();
             this.pvm = pvm;  
            project = proj;
            this.sr = sr;
            this.mw = mw;
            this.DataContext = project; 



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {




        }
    }
}
