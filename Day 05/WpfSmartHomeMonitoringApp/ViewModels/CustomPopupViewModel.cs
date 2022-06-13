using Caliburn.Micro;
using WPFSmartHomeMonitoringApp.Helpers;

namespace WPFSmartHomeMonitoringApp.ViewModels
{
    public class CustomPopupViewModel : Conductor<object>
    {
        private string topic;
        private string brokerIp;

        public string Topic
        {
            get => topic; set
            {
                topic = value;
                NotifyOfPropertyChange(() => Topic);
            }
        }
        public string BrokerIp
        {
            get => brokerIp; set
            {
                brokerIp = value;
                NotifyOfPropertyChange(() => BrokerIp);

            }
        }
        public CustomPopupViewModel(string title)
        {
            this.DisplayName = title;
             
            BrokerIp = "127.0.0.1"; //MQTT IP설정
            Topic = "home/device/fakedata/"; ;
        }

        public void AcceptClose()
        {
            Commons.BROKERHOST = BrokerIp;
            Commons.PUB_TOPIC = Topic;
            //창닫기
            TryCloseAsync(true);
        }
    }
}
