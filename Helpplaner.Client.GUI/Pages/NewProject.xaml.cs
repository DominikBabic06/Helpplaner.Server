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
using System.Windows.Shapes;

namespace Helpplaner.Client.GUI.Pages
{
    /// <summary>
    /// Interaktionslogik für NewAP.xaml
    /// </summary>
    public partial class NewProject : Window
    {
  
        Helpplaner.Service.Objects.WorkPackage[] APs;
        ServerCommunicator sr;

        ProjectViewModel pvm;

         User responsible;
        List<WorkPackage> dependencies;
        int time = 0;   


        public NewProject(ServerCommunicator sr, ProjectViewModel pvm)
        {
            InitializeComponent();
            dependencies = new List<WorkPackage>(); 

    
            this.sr = sr;
            this.DataContext = pvm;
            this.pvm = pvm;





        }

        private void Speichern_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               Project newProject = new Project();
                newProject.Name = Name.Text;
                newProject.Description = Beschreibung.Text;
                newProject.Active = true;   
                sr.AddProject(newProject);  

              

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
            
            this.Close();

           


        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EnableButton()
        {
            if (Name.Text != "" && Beschreibung.Text != "")
            {
                Save.IsEnabled = true;
            }
            else
            {
                Save.IsEnabled = false;
            }
        }

        private void Name_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = "";   
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void Beschreibung_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        private void Zuständiger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            responsible = (User)Zuständiger.SelectedItem; 
            EnableButton();
        }
        private void Dependants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dependencies.Clear();   
            foreach (var item in Dependants.SelectedItems)
            {
                dependencies.Add((WorkPackage)item);
            }   
            EnableButton();

        }

     

      

       
    }
}
