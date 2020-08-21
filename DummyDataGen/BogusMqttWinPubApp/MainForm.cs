using Bogus;
using MetroFramework.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;

namespace BogusMqttWinPubApp
{
    public partial class MainForm : MetroForm
    {
        public static string MqttBrokerUrl { get; private set; }
        public static MqttClient BrokerClient { get; private set; }
        private static Thread MqttThread { get; set; }
        private static Faker<SensorInfo> SensorFaker { get; set; }
        private static string CurrValue { get; set; }

        BackgroundWorker MqttWorker;

        public MainForm()
        {
            InitializeComponent();
            InitializeAll(); // 전체 초기화
        }

        private void InitializeAll()
        {
            MqttWorker = new BackgroundWorker();
            MqttWorker.DoWork += MqttWorker_DoWork;
            MqttWorker.WorkerReportsProgress = false;
            MqttWorker.WorkerSupportsCancellation = true;

            MqttBrokerUrl = "localhost"; // or 127.0.0.1 or my IpAddress

            string[] Rooms = new[] { "DiningRoom", "LivingRoom", "BathRoom", "BedRoom", "GuestRoom" };

            SensorFaker = new Faker<SensorInfo>()
                            .RuleFor(s => s.Dev_Id, f => f.PickRandom(Rooms))
                            .RuleFor(s => s.Curr_Time, f => f.Date.Past(0).ToString("yyyy-MM-dd HH:mm:ss.ff")) // 
                            .RuleFor(s => s.Temp, f => float.Parse(f.Random.Float(19.0f, 32f).ToString("0.00")))
                            .RuleFor(s => s.Humid, f => float.Parse(f.Random.Float(40.0f, 70f).ToString("0.0")))
                            .RuleFor(s => s.Press, f => float.Parse(f.Random.Float(899.0f, 1010.0f).ToString("0.0")));
        }

        private void MqttWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            LoopPublish();
        }

        private void ConnectMqttBroker()
        {
            MqttBrokerUrl = TxtBrokerIP.Text;
            BrokerClient = new MqttClient(MqttBrokerUrl);
            BrokerClient.Connect("FakerDaemon");
        }

        private void StartPublish()
        {
            MqttThread = new Thread(new ThreadStart(LoopPublish));
            //MqttThread = new Thread(() => LoopPublish());
            MqttThread.Start();
        }

        private void LoopPublish()
        {
            while (true)
            {
                SensorInfo value = SensorFaker.Generate();
                CurrValue = JsonConvert.SerializeObject(value, Formatting.Indented);
                BrokerClient.Publish("home/device/data", Encoding.Default.GetBytes(CurrValue));
                this.Invoke(new Action(() =>
                {
                    RtbLog.AppendText($"Published: {CurrValue}\n");
                    RtbLog.ScrollToCaret();
                }));
                
                Thread.Sleep(1000);
            }
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            ConnectMqttBroker(); // MQTT 브로커 접속
            //StartPublish(); // Fake 센싱 메시지 전송
            MqttWorker.RunWorkerAsync();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MqttWorker.IsBusy)
            {
                MqttWorker.CancelAsync();
                MqttWorker.Dispose();
            }
            Environment.Exit(0);
        }
    }
}
