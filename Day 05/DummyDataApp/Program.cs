using Bogus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DummyDataApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var Rooms = new[] {"DINNING","LIVING","BATH","BED"};      //배열로 네가지
            var sensorDummy = new Faker<SensorInfo>()
                .RuleFor(r => r.DevId, f => f.PickRandom(Rooms))
                .RuleFor(r => r.CurrTime, f => f.Date.Past(0))
                .RuleFor(r => r.Temp, f => f.Random.Float(19.0f, 20.9f))        //이 온도 사이에서 데이터가 만들얼지게한다
                .RuleFor(r => r.Humid, f => f.Random.Float(40.0f, 63.9f));

            var currValue = sensorDummy.Generate();

            Console.WriteLine(JsonConvert.SerializeObject(currValue,Formatting.Indented));

            Thread.Sleep(1000);
        }
    }
}
