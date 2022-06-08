using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyDataApp
{
    public class SensorInfo
    {
        public string DevId { get; set; }       //기기아이디
        public DateTime CurrTime { get; set; }    //현재 시간 
        public float Temp { get; set; }         //
        public float Humid { get; set; }        //
    }
}
