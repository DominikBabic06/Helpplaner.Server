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
        Helpplaner.Service.Objects.WorkPackage[] APs;
        ServerCommunicator sr;

        ProjectViewModel pvm;

         User responsible;
        List<WorkPackage> dependencies;
        int time = 0;   


        public NewAP(Project proj, ServerCommunicator sr, ProjectViewModel pvm)
        {
            InitializeComponent();
            dependencies = new List<WorkPackage>(); 

            Project = proj;
            this.sr = sr;
            this.DataContext = pvm;
            this.pvm = pvm;





        }

        private void Speichern_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Service.Objects.WorkPackage task = new Service.Objects.WorkPackage();
                task.ProjectID = Project.ID;
                task.Name = Name.Text;
                task.Description = Beschreibung.Text;
                task.RealTime = "0";
                task.ExpectedTime = Dauer.Text; 
                task.Responsible = responsible.ID;
                task.Status = "Aktiv";  
                task.IdInProject = ""+ sr.GetFristAvalibleIdinProject(Project);
                if (dependencies.Count == 0)
                {
                    dependencies.Add(pvm.Tasks.Where(x => x.Name == "Start").First());
                }

                task.ID = sr.AddTaskToProject(task, Convert.ToInt32(Project.ID));

                sr.addDependency(dependencies.ToArray(), task);

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
            if (Name.Text != "" && Beschreibung.Text != "" && responsible != null && time != null)
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

        private void Zubutton_Click(object sender, RoutedEventArgs e)
        {
            Zubutton.Foreground = Brushes.Black;
            AbButton.Foreground = Brushes.Gray;
            Zuständiger.Visibility = Visibility.Visible;    
           Dependants.Visibility = Visibility.Collapsed;   
            Search.Text = "";
        }

        private void AbButton_Click(object sender, RoutedEventArgs e)
        {
            Zubutton.Foreground = Brushes.Gray;
            AbButton.Foreground = Brushes.Black;
            Zuständiger.Visibility = Visibility.Collapsed;
            Dependants.Visibility = Visibility.Visible;
            Search.Text = "";




        }

        private void Dauer_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Dauer.Text != "")
            {
                try
                {
                    Convert.ToInt32(Dauer.Text);
                    Waring.Visibility = Visibility.Collapsed;   
                    EnableButton();
                }
                catch (Exception)
                {
                    Waring.Visibility = Visibility.Visible; 
                    Waring.Content = "Bitte geben Sie eine ganze Zahl ein";
                    Dauer.Text = "";
                }
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            if (Search.Text != "")
            {
                if(Zuständiger.IsVisible == true)
                {
                    List<User> Temp = new List<User>();
                    Temp.AddRange(pvm.users.Where(x => x.Username.Contains(Search.Text)));
                    Zuständiger.ItemsSource = Temp;
                }
                if(Dependants.IsVisible == true)
                {
                    List<WorkPackage> Temp = new List<WorkPackage>();
                    Temp.AddRange(pvm.Tasks.Where(x => x.Name.Contains(Search.Text)));
                    Dependants.ItemsSource = Temp;
                }   
                

            }

            else
            {
                Zuständiger.ItemsSource = pvm.users;    
                Dependants.ItemsSource = pvm.Tasks; 

            }

        }
    }
}
