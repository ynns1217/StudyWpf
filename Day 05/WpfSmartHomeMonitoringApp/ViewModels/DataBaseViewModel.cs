using Caliburn.Micro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using WPFSmartHomeMonitoringApp.Helpers;
using WPFSmartHomeMonitoringApp.Models;

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
            Topic = Commons.PUB_TOPIC = "home/device/fakedata/";//Multiple Topic
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


        //Subscribe한 메시지 처리해주는 이벤트핸들러
        public void MQTT_CLIENT_MqttMsgPublishReceived(object sender,MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            UpdateText(message);   //센서데이터 출력
            SetDataBase(message,e.Topic); //DB에 저장

        }

        private void SetDataBase(string message , string topic)
        {
            var currDatas = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
            //


            Debug.WriteLine(currDatas);

            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                conn.Open();
                string strInQuery = @"INSERT INTO TblSmartHome
                                               (Devid
                                               ,CurrTime
                                               ,Temp
                                               ,Humid)
                                         VALUES
                                               (@Devid
                                               ,@CurrTime
                                               ,@Temp
                                               ,@Humid)";

                try
                {
                    SqlCommand cmd = new SqlCommand(strInQuery, conn);

                    SqlParameter parmDevId = new SqlParameter("@DevId", currDatas["DevId"]);
                    cmd.Parameters.Add(parmDevId);

                    SqlParameter parmCurrTime = new SqlParameter("@CurrTime", DateTime.Parse(currDatas["CurrTime"]));   //날짜형 변환 필요
                    cmd.Parameters.Add(parmCurrTime);

                    SqlParameter parmTemp = new SqlParameter("@Temp", currDatas["Temp"]);
                    cmd.Parameters.Add(parmTemp);

                    SqlParameter parmHumid = new SqlParameter("@Humid", currDatas["Humid"]);
                    cmd.Parameters.Add(parmHumid);

                    if (cmd.ExecuteNonQuery() == 1)
                        UpdateText(">>> DB Inserted.");     //저장성공
                    else
                        UpdateText(">>> DB Failed!!");      //저장 실패             

                }
                catch (Exception ex)
                {

                    UpdateText($">>> DB Error!! { ex.Message}");        //예외
                }
            }

        }
    }
}
