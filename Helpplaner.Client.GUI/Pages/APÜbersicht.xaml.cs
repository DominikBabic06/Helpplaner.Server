using Helpplaner.Service.Objects;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaktionslogik für APÜbersicht.xaml
    /// </summary>
    public partial class APÜbersicht : Page
    {
        Project Project;
    Helpplaner.Service.Objects.WorkPackage [] APs;
        ServerCommunicator sr;

        ProjectViewModel pvm;   

        WorkPackage selectedTask;   
        MainWindow main;
        APÜbersicht aPÜbersicht;   
        

        


        public APÜbersicht(Project proj, ServerCommunicator sr , ProjectViewModel pvm, MainWindow main )
        {
            InitializeComponent();
            Project = proj;
            this.sr = sr;
            this.DataContext = pvm; 
            this.pvm = pvm; 
            this.main = main;   
           


           
            

        
        }


         

        private void APDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

     
        public void Reload()

        {

            
           
            AP.ItemsSource = null;
            AP.ItemsSource = pvm.Tasks;
            TextBox_TextChanged(null, null); 

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string cat;
            WorkPackage[] temp = pvm.Tasks.ToArray();
            if(SearchCategory.SelectedValue == null)
            {
              cat = "Name";
            }
            else
            {
                cat = SearchCategory.SelectedValue.ToString();
             }
            switch (cat)
            {
                case "ID":
                    temp = temp.Where(x => x.ID.ToString().Contains(SearchBox.Text)).ToArray(); 
                    break;
                case "Name":
                    temp = temp.Where(x => x.Name.Contains(SearchBox.Text)).ToArray(); 
                    break;
                case "Beschreibung":
                    temp = temp.Where(x => x.Description.Contains(SearchBox.Text)).ToArray(); 
                    break;
                case "Zuständiger":
                    temp = temp.Where(x => x.ReponsibleName.Contains(SearchBox.Text)).ToArray(); 
                    break;
                default:
                    temp = temp.Where(x => x.Name.ToString().Contains(SearchBox.Text)).ToArray();
                    break;



            }

            if(SearchBox.Text == "")
            {
                AP.ItemsSource = pvm.Tasks; 
            }
            else
            {
                AP.ItemsSource = temp; 
            }
            
        }

        private void AP_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            selectedTask = (WorkPackage)AP.SelectedItem;    
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewAP newAP = new NewAP(Project, sr, pvm);

            newAP.Activate();
            newAP.ShowDialog();


        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (selectedTask == null)
            {
                Waring.Content = "Bitte wählen Sie eine Aufgabe aus";
                return;
            }
            if (selectedTask.Successor == "" )
            {
                Waring.Content = "";
                sr.DeleteTask(selectedTask);

            }
            else
            {
                Waring.Content = "Die Aufgabe hat Nachfolger, bitte löschen Sie diese zuerst";
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            APDetail aPDetail = new APDetail(pvm, selectedTask, sr, main, this);   
            main.changeShowPage(aPDetail);
         
        }

        private void AP_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (selectedTask != null)
            {
                APDetail aPDetail = new APDetail(pvm, selectedTask, sr, main, this);
                main.changeShowPage(aPDetail);
            }
           
        }

        private void PopupButton_Click(object sender, RoutedEventArgs e)
        {   if(selectedTask == null)
            {
                Waring.Content = "Bitte wählen Sie eine Aufgabe aus";
                return;
            }
            Popup popup = new Popup(sr, main, selectedTask);
            popup.Activate();
            popup.ShowDialog();

        }
    }
}
