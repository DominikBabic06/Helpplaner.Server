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

namespace Helpplaner.Client.GUI.Pages
{
    /// <summary>
    /// Interaktionslogik für APDetail.xaml
    /// </summary>
    public partial class APDetail : Page
    {
        ProjectViewModel projectViewModel;
        WorkPackage workPackage;    
        ServerCommunicator serverCommunicator;  
        MainWindow main;
        APÜbersicht ap; 



        public APDetail( ProjectViewModel projectViewModel, WorkPackage workPackage, ServerCommunicator serverCommunicator, MainWindow main, APÜbersicht ap)
        {
            InitializeComponent();
            this.projectViewModel = projectViewModel;
            this.workPackage = workPackage;
            this.serverCommunicator = serverCommunicator;
            this.main = main;   
            this.ap = ap;   

            Intinalizeoverview();
        }
        public void Intinalizeoverview()
        {
            this.DataContext = workPackage;
            
        }
        public void Edit_Click(object sender, RoutedEventArgs e)
        {
         
        } 
        public void Save_Click(object sender, RoutedEventArgs e)
        {
           
        }   
        public void Delete_Click(object sender, RoutedEventArgs e)
        {
            main.changeShowPage(ap);    
        }   

    }
}
