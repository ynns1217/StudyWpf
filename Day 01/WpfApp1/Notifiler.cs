using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Notifiler : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;  //이 이벤트를 쓰기위해 메서드를 하나 만든다

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
