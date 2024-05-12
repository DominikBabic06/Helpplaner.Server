using Helpplaner.Service.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace Helpplaner.Client.GUI.Pages
{
    /// <summary>
    /// Interaktionslogik für Popup.xaml
    /// </summary>
    public partial class Popup : Window
    {
        Stopwatch sw = new Stopwatch(); 
        
        DispatcherTimer timer = new DispatcherTimer();  

        DispatcherTimer AlertTimer = new DispatcherTimer();
        DateTime StartTime ;
        DateTime IntervalTime;
        int Minutes = 0;
        int Horus = 0;
        int Seconds = 0;
        int TotalSecons = 0;
        bool firstTime = true;

        string stringTimer { get
            {
                if(Horus < 10 && Minutes < 10)
                {
                    return "0" + Horus + ":" + "0" + Minutes;
                }
                if (Horus < 10)
                {
                    return "0" + Horus + ":" + Minutes;
                }
                if (Minutes < 10)
                {
                    return Horus + ":" + "0" + Minutes;
                }

                return Horus + ":" + Minutes;   
            } }

        ServerCommunicator ServerCommunicator;  
        MainWindow MainWindow;  
        WorkPackage WorkPackage;
    
        public Popup(ServerCommunicator sc, MainWindow mw,WorkPackage workPackage)
        {
            InitializeComponent();
            ServerCommunicator = sc;
            MainWindow = mw;
            WorkPackage = workPackage;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Topmost = true;    
            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(firstTime)
            {
                ServerCommunicator.StartWorkSession(WorkPackage);
                StartTime = DateTime.Now;
               
                firstTime = false;
                timer.Tick += dispatcherTimer_Tick;
                timer.Interval = new TimeSpan(0, 0, 1);
                
            }
            if(!timer.IsEnabled)
            {
                ServerCommunicator.ContinueWorkSession();
                timer.Start();
                IntervalTime = DateTime.Now;    
                Play.Data = StreamGeometry.Parse("M144 479H48c-26.5 0-48-21.5-48-48V79c0-26.5 21.5-48 48-48h96c26.5 0 48 21.5 48 48v352c0 26.5-21.5 48-48 48zm304-48V79c0-26.5-21.5-48-48-48h-96c-26.5 0-48 21.5-48 48v352c0 26.5 21.5 48 48 48h96c26.5 0 48-21.5 48-48z");
                 sw.Start();
            }
            else
            {
                ServerCommunicator.PauseWorkSession();
                Play.Data = StreamGeometry.Parse("M424.4 214.7L72.4 6.6C43.8-10.3 0 6.1 0 47.9V464c0 37.5 40.7 60.1 72.4 41.3l352-208c31.4-18.5 31.5-64.1 0-82.6z");  
                timer.Stop();
                TotalSecons += Seconds;
                sw.Stop();
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Seconds = sw.Elapsed.Seconds;   
            if (Seconds == 60)
            {
                Seconds = 0;
                Minutes++;
            }
            if (Minutes == 60)
            {
                Minutes = 0;
                Horus++;
            }
            Timer.Content = stringTimer;    
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            sw.Stop();  
            ServerCommunicator.StopWorkSession();
            this.Close();   
        }
    }
}
