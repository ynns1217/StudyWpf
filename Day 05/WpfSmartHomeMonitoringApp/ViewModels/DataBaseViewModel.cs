using Caliburn.Micro;
using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using WPFSmartHomeMonitoringApp.Helpers;

namespace WPFSmartHomeMonitoringApp.ViewModels
{
    public class DataBaseViewModel : Screen
    {
        public string brokerUrl;

        public string BrokerUrl
        {
            get { return brokerUrl;}
            set
            {
                brokerUrl = value;
                NotifyOfPropertyChange(() => BrokerUrl);
            }
        }

        public string topic;
        public string Topic
        {
            get { return topic; }
            set
            {
                topic = value;
                NotifyOfPropertyChange(() => Topic);
            }
        }

        public string connString;
        public string ConnString
        {
            get { return connString; }
            set
            {
                connString = value;
                NotifyOfPropertyChange(() => ConnString);
            }
        }

        public string dbLog;
        public string DbLog
        {
            get { return dbLog; }
            set
            {
                dbLog = value;
                NotifyOfPropertyChange(() => DbLog);
            }
        }

        public bool isConnected;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                isConnected = value;
                NotifyOfPropertyChange(() => IsConnected);
            }
        }

        public DataBaseViewModel()
        {
            BrokerUrl = Commons.BROKERHOST = "127.0.0.1"; //MQTT IP설정
            Topic = Commons.PUB_TOPIC = "home/device/fakedata/";
            connString = Commons.CONNSTRING = "Data Source=PC01;Initial Catalog=OpenApiLab;Integrated Security=True;";
            if (Commons.IS_CONNECT )
            {
                IsConnected = true;
                ConnectDb();
            } 
        }

        /// <summary>
        /// db연결 + mqtt broker 접속
        /// </summary>
        public void ConnectDb()
        {
            if(IsConnected)
            {
                Commons.MQTT_CLIENT = new MqttClient(BrokerUrl);

                try
                {
                    if (Commons.MQTT_CLIENT.IsConnected != true)
                    {
                        Commons.MQTT_CLIENT.MqttMsgPublishReceived += MQTT_CLIENT_MqttMsgPublishReceived;
                        Commons.MQTT_CLIENT.Connect("MONITOR");
                        Commons.MQTT_CLIENT.Subscribe(new string[] { Commons.PUB_TOPIC },
                            new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
                        UpdateText(">>> MQTT Broker Connected");
                        isConnected = Commons.IS_CONNECT = true;

                    }
                }
                catch (Exception ex)
                {

                   
                }
            }
            else //접속 종료 
            {
                try
                {
                    if (Commons.MQTT_CLIENT.IsConnected )
                    {
                        Commons.MQTT_CLIENT.MqttMsgPublishReceived -= MQTT_CLIENT_MqttMsgPublishReceived;
                        Commons.MQTT_CLIENT.Disconnect();
                        UpdateText(">>> MQTT Broker Disconnected...");
                        isConnected = Commons.IS_CONNECT = false;
                    }

                }
                catch (Exception ex)
                {

                
                }
            }
        }

        private void UpdateText(string message)
        {
            DbLog += $"{message}\n";
        }

        private void MQTT_CLIENT_MqttMsgPublishReceived(object sender,MqttMsgPublishEventArgs e)
        {
            var meassage = Encoding.UTF8.GetString(e.Message);
            UpdateText(meassage);

        }
    }
}
