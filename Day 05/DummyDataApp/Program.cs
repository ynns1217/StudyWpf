using Bogus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace DummyDataApp
{
    class Program
    {
        public static string MqttBrokerUrl { get; set; }
        public static MqttClient Client { get; set; }
        private static Thread MqttThread { get; set; }
        private static Faker<SensorInfo> SensorData { get; set; }
        private static string CurrValue { get; set; }
        static void Main(string[] args)
        {
            //
            //var Rooms = new[] {"DINNING","LIVING","BATH","BED"};      //배열로 네가지
            //var sensorDummy = new Faker<SensorInfo>()
            //    .RuleFor(r => r.DevId, f => f.PickRandom(Rooms))
            //    .RuleFor(r => r.CurrTime, f => f.Date.Past(0))
            //    .RuleFor(r => r.Temp, f => f.Random.Float(19.0f, 20.9f))        //이 온도 사이에서 데이터가 만들얼지게한다
            //    .RuleFor(r => r.Humid, f => f.Random.Float(40.0f, 63.9f));

            //var currValue = sensorDummy.Generate();

            //Console.WriteLine(JsonConvert.SerializeObject(currValue,Formatting.Indented));

            //Thread.Sleep(1000);
            //
            InitializeConfig();     //구성 초기화
            ConnectMqttBroker();    //브로커에 접속
            StartPublicsh();        //배포(Publish 발행)
        }

        private static void StartPublicsh()
        {

            MqttThread = new Thread(() => LoopPublish());
            MqttThread.Start();     //스레드는 시작은 필요하지만 스탑은 필요없다

            //Thread thread2 = new Thread(()=> LoopPublish2());
            //MqttThread.Start();

            //Thread thread3 = new Thread(() => LoopPublish3());
            //MqttThread.Start();
        }

        private static void ConnectMqttBroker()
        {
            try
            {
                Client = new MqttClient(MqttBrokerUrl);
                Client.Connect("SMARTHOME01");
            }
            catch (Exception ex)
            {

                Console.Write($"접속불가 : {ex}");
                Environment.Exit(5);
            }

        }


        private static void InitializeConfig()
        {
            MqttBrokerUrl = "localhost";         //127.0.0.1 // OR //내 아이피 -> cmd -> ipconfig

            var Rooms = new[] { "DINNING", "LIVING", "BATH", "BED" };      //배열로 네가지
            SensorData = new Faker<SensorInfo>()
                .RuleFor(r => r.DevId, f => f.PickRandom(Rooms))
                .RuleFor(r => r.CurrTime, f => f.Date.Past(0))
                .RuleFor(r => r.Temp, f => f.Random.Float(19.0f, 20.9f))        //이 온도 사이에서 데이터가 만들얼지게한다
                .RuleFor(r => r.Humid, f => f.Random.Float(40.0f, 63.9f));

        }

        private static void LoopPublish()
        {
            while (true)
            {
                SensorInfo tempValue = SensorData.Generate();
                CurrValue = JsonConvert.SerializeObject(tempValue, Formatting.Indented);
                Client.Publish("home/device/fakedata/", Encoding.Default.GetBytes(CurrValue));
                Console.WriteLine($"Published : {CurrValue}");
                Thread.Sleep(3000);
            }
        }


    }
}
