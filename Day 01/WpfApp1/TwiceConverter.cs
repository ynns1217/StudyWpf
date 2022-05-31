using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApp1
{
    public class TwiceConverter : IValueConverter 
    {
        //Ivalue안에 선언되어있는 것들(인터페이스 구현: alt+enter ) 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.Parse(value.ToString())*2;         //인트 파스는 스트링밖에 안되기때문에 투스트링으로 바꿔준다
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
