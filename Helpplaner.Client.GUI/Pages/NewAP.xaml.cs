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
    public partial class NewAP : Window
    {
        Project Project;
        Helpplaner.Service.Objects.Task[] APs;
        ServerCommunicator sr;

        ProjectViewModel pvm;



        public NewAP(Project proj, ServerCommunicator sr, ProjectViewModel pvm)
        {
            InitializeComponent();
            Project = proj;
            this.sr = sr;
            this.DataContext = pvm;
            this.pvm = pvm;

            Zuständiger.ItemsSource = pvm.users;    




        }

        private void Speichern_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Service.Objects.Task task = new Service.Objects.Task();
                task.Projekt_ID = Project.Projekt_ID;
                task.Arbeitspaket_Name = Name.Text;
                task.Arbeitspaket_Beschreibung = Beschreibung.Text;

                User user = (User)Zuständiger.SelectedItem;
                task.Arbeitspaket_Zustaendiger = user.Nutzer_ID;

                sr.AddTaskToProject(task, Convert.ToInt32(Project.Projekt_ID));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }   
            
            this.Close();

           


        }
    }
}
