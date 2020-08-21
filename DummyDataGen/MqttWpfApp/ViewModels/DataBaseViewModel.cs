using Caliburn.Micro;
using MqttMoniteringWpfApp.Helpers;
using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttMoniteringWpfApp.ViewModels
{
    public class DataBaseViewModel : Conductor<object>
    {
        private string brokerUrl;
        public string BrokerUrl
        {
            get => brokerUrl;
            set
            {
                brokerUrl = value;
                NotifyOfPropertyChange(() => BrokerUrl);
            }
        }

        private string topic;
        public string Topic
        {
            get => topic;
            set
            {
                topic = value;
                NotifyOfPropertyChange(() => Topic);
            }
        }

        private string connString;
        public string ConnString
        {
            get => connString;
            set
            {
                connString = value;
                NotifyOfPropertyChange(() => ConnString);
            }
        }

        private string dbLog;
        public string DbLog
        {
            get => dbLog;
            set
            {
                dbLog = value;
                NotifyOfPropertyChange(() => DbLog);
            }
        }

        private bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                isConnected = value;
                NotifyOfPropertyChange(() => IsConnected);
            }
        }

        public DataBaseViewModel()
        {
            BrokerUrl = Commons.BROKERHOST;
            Topic = Commons.PUB_TOPIC;
            Commons.CONNSTRING = ConnString = "Server=localhost;Port3306;" +
                "Database=iot_sensordata;Uid=root;Pwd=mysql_p@ssw0rd";
        }


        public void Connect()
        {
            if (IsConnected) //토글버튼 온
            {
                Commons.BROKERCLIENT = new MqttClient(BrokerUrl);
                try
                {
                    if (Commons.BROKERCLIENT.IsConnected != true)
                    {
                        Commons.BROKERCLIENT.MqttMsgPublishReceived += BROKERCLIENT_MqttMsgPublishReceived; //메세지 받을때마다 이벤트 시작
                        Commons.BROKERCLIENT.Connect("MqttMonitor"); //연결
                        Commons.BROKERCLIENT.Subscribe(new string[] { Commons.PUB_TOPIC }
                        , new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
                        UpdateText(">>> Message : Boroker Connected");
                    }
                }
                catch (Exception)
                {
                }
            }
            else //토글버튼 오프
            {
                try
                {
                    if (Commons.BROKERCLIENT.IsConnected)
                    {
                        Commons.BROKERCLIENT.MqttMsgPublishReceived -= BROKERCLIENT_MqttMsgPublishReceived; //메세지 받을때마다 이벤트 시작
                        Commons.BROKERCLIENT.Disconnect();//연결 끊기
                        UpdateText(">>> Message : Boroker Disconnected...");
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void BROKERCLIENT_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            UpdateText($">>> Message : {message}");
            InsertDataBase(message);
        }

        private void InsertDataBase(string message)
        {
            //TODO
        }

        private void UpdateText(string message)
        {
            DbLog += $"{message}\n";
        }
    }
}
