using Caliburn.Micro;
using MqttMoniteringWpfApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttMoniteringWpfApp.ViewModels
{
    public class CustomPopupViewModel : Conductor<object>
    {
        string brokerIP;
        public string BrokerIP
        {
            get => brokerIP;
            set
            {
                brokerIP = value;
                NotifyOfPropertyChange(() => BrokerIP);
            }
        }

        string topic;
        public string Topic
        {
            get => topic;
            set
            {
                topic = value;
                NotifyOfPropertyChange(() => Topic);
            }
        }

        public CustomPopupViewModel(string title)
        {
            DisplayName = title;

            BrokerIP = "localhost";
            Topic = "home/device/data";
        }

        public void AcceptClose()
        {
            Commons.BROKERHOST = BrokerIP;
            Commons.PUB_TOPIC = Topic;

            TryClose(true);
        }
    }
}
