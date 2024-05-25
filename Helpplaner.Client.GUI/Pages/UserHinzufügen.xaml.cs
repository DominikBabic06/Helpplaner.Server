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
using Helpplaner.Service.Objects;   


namespace Helpplaner.Client.GUI.Pages
{
    /// <summary>
    /// Interaktionslogik für UserHinzufügen.xaml
    /// </summary>
    public partial class UserHinzufügen : Window
    {
        Project PRO;    
        ServerCommunicator sr;
        ProjectViewModel pvm;

        User user;


        public UserHinzufügen()
        {
            InitializeComponent();
        }
        public UserHinzufügen(Project proj, ServerCommunicator sr, ProjectViewModel pvm)
        {
            InitializeComponent();
            PRO = proj; 
            this.sr = sr;
            this.pvm = pvm;
            FillUser(); 

        }

        public void FillUser()
        {

            List<User> User = new List<User>();
            User = pvm.globalUser.ToList(); 
            foreach (User u in pvm.globalUser)
            {
               foreach (User u2 in pvm.users)
                {
                    if (u.ID == u2.ID)
                    {
                       User.Remove(u);  
                        
                    }
                }
            }


            UserList.ItemsSource = User;
            
        }

        private void UserList_Selected(object sender, RoutedEventArgs e)
        {
            user = (User)UserList.SelectedItem; 

        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
         
        }

        private void UserList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            user = (User)UserList.SelectedItem;

            Save.IsEnabled = true;

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string err = sr.MakeUserProjectRelation(pvm.currentProjectID, Convert.ToInt32(user.ID),  Admin.IsChecked.Value);
            if ("done" != err)
            {
                Waring.Content = err;
            }
            else
            {
                this.Close();
            }   

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
           this.Close();

        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Search.Text != "")
            {
                List<User> User = new List<User>();
                User = pvm.globalUser.ToList();
                foreach (User u in pvm.globalUser)
                {
                    foreach (User u2 in pvm.users)
                    {
                        if (u.ID == u2.ID)
                        {
                            User.Remove(u);

                        }
                    }
                }

                List<User> User2 = new List<User>();
                foreach (User u in User)
                {
                    if (u.Username.Contains(Search.Text))
                    {
                        User2.Add(u);
                    }
                }
                UserList.ItemsSource = User2;
            }
            else
            {
                FillUser();
            }   
        }

        public void Reload()
        {
            Search_TextChanged(null, null); 
            
        }
    }
}
