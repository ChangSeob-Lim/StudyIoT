using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MqttMoniteringWpfApp.ViewModels
{
    public class MainViewModel : Conductor<object>
    {
        public MainViewModel()
        {
            DisplayName = "MQTT Monitoring App - v0.9";
        }

        public void ExitProgram()
        {
            Environment.Exit(0);
        }

        public void BtnExit()
        {
            ExitProgram();
        }

        public void LoadDataBaseView()
        {
            ActivateItem(new DataBaseViewModel());
        }

        public void LoadRealTimeView()
        {
            ActivateItem(new RealTimeViewModel());
        }

        public void LoadHistoryView()
        {
            ActivateItem(new HistoryViewModel());
        }

        public void PopInfoDialog()
        {
            TaskStart();
        }

        private void TaskStart()
        {
            var wManager = new WindowManager();
            var result = wManager.ShowDialog(new CustomPopupViewModel("New Broker"));

            if (result == true)
            {
                MessageBox.Show("Start Subscribe!!!"); // Here is in Logics...
            }
        }
    }
}
