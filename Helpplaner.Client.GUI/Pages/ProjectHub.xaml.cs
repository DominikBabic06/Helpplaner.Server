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
    /// Interaktionslogik für ProjektHub.xaml
    /// </summary>
    public partial class ProjectHub : Page
    {
        ServerCommunicator SC;
        ProjectViewModel pv;
        MainWindow mw;

        Project selectedProject;


        public ProjectHub(ServerCommunicator SC, ProjectViewModel pv, MainWindow mw)
        {
            InitializeComponent();
            this.SC = SC;
            this.pv = pv;
            this.mw = mw;
            this.DataContext = pv;  
            Projekts.ItemsSource = pv.projects; 
            ProjektsUser.ItemsSource = pv.globalUser;

            
            cbAktiveProjekte.IsChecked = true;  

        }
        public void  changeShown()

        {
            var search = Search.Text;
            if (search == "")
            {
                if (cbAktiveProjekte.IsChecked == false)
                    if (cbAdmin.IsChecked == true)
                        Projekts.ItemsSource = pv.projects.Where(p => p.UserIsAdmin == true);   
                    else
                    Projekts.ItemsSource = pv.projects;
                else
                {
                   
                    if(cbAdmin.IsChecked == true)
                    {
                        Projekts.ItemsSource = pv.projects.Where(p => p.Active == true && p.UserIsAdmin == true);
                    }
                    else
                    {
                        Projekts.ItemsSource = pv.projects.Where(p => p.Active == true);
                    }

                }
            }
            else
            {
                if (cbAktiveProjekte.IsChecked == false)
                    if (cbAdmin.IsChecked == true)
                        Projekts.ItemsSource = pv.projects.Where(p => p.Name.Contains(search) && p.UserIsAdmin == true);    
                    else
                    Projekts.ItemsSource = pv.projects.Where(p => p.Name.Contains(search));
                else
                    if (cbAdmin.IsChecked == true)
                    Projekts.ItemsSource = pv.projects.Where(p => p.Name.Contains(search) && p.Active == true && p.UserIsAdmin == true);    
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

        private void Projekts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            pv.curentProject = selectedProject; 
            pv.currentProjectID = Convert.ToInt32(selectedProject.ID);
            pv.currentProjectName = selectedProject.Name;
            mw.SelectProject(pv.curentProject);
        }

        private void Projekts_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (Projekts.SelectedItem != null)
            {
                selectedProject = (Project)Projekts.SelectedItem;
                
            }

        }

        private void NewProjButton_Click(object sender, RoutedEventArgs e)
        {
            NewProject newProject = new NewProject(SC, pv);
            newProject.ShowDialog();    

        }
        
       

        public void Reload()
        {

            Projekts.ItemsSource = null;
            Projekts.ItemsSource = pv.projects; 
            ProjektsUser.ItemsSource = null;    
            ProjektsUser.ItemsSource = pv.globalUser;

            
            changeShown();  
        }

        private void SearchUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = Search.Text;
            if (search == "")
            {
                ProjektsUser.ItemsSource = pv.globalUser.Where(p => p.Username.Contains(search));

            }

            }

        private void NewUser_Click(object sender, RoutedEventArgs e)
        {

            NewUser newUser = new NewUser(SC, pv);
            newUser.ShowDialog();
        }
    }
}




