using Caliburn.Micro;
using System;
using System.Threading;
using System.Threading.Tasks;
using WPFSmartHomeMonitoringApp.Helpers;

namespace WPFSmartHomeMonitoringApp.ViewModels
{
    public class MainViewModel :    Conductor<object> //screen에는 Activateitem이라는 메서드 없음
    {
        public MainViewModel()
        {
            DisplayName = "SmartHomeMonitoring v2.0"; //윈도우 타이틀
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if(Commons.MQTT_CLIENT.IsConnected)
            {
                Commons.MQTT_CLIENT.Disconnect();
                Commons.MQTT_CLIENT = null;
            }//비활성화 처리

            return base.OnDeactivateAsync(close, cancellationToken);
        }

        public void LoadDataBaseView()
        {
            ActivateItemAsync(new DataBaseViewModel()); 
        }

        public void LoadRealTimeView()
        {
            ActivateItemAsync(new RealTimeViewModel());
        }

        public void LoadHistoryView()
        {
            ActivateItemAsync(new HistoryViewModel());
        }

        public void ExitProgram()
        {
            Environment.Exit(0); //오류없이 종료
        }
        public void ExitToolbar()
        {
            Environment.Exit(0); //오류없이 종료
        }
    
        //Start 메뉴, 아이콘눌렀을 때 처리할 이벤트
        public void PopInfoDialog()
        {
            TaskPopup();
        }

        public void StartSubscribe()
        {
            TaskPopup();
        }

        private void TaskPopup()
        {
            //CustomPopupView
            var winManager = new WindowManager();
            var result = winManager.ShowDialogAsync(new CustomPopupViewModel("New Broker"));

            if (result.Result == true)
            {
                ActivateItemAsync(new DataBaseViewModel());   //화면 전환
            }
        }

        public void PopInfoView()
        {
            var winManager = new WindowManager();
            winManager.ShowDialogAsync(new CustomInfoViewModel("About"));
        }

        public void ToolBarStopSubscribe()
        {
            StopSubscribe();
        }

        public void MenuStopSubscribe()
        {
            StopSubscribe();

        }

        private void StopSubscribe()
        {
            if(this.ActiveItem is DataBaseViewModel)
            {
                DataBaseViewModel activeModel = (this.ActiveItem as DataBaseViewModel);
                try
                {
                    if(Commons.MQTT_CLIENT.IsConnected)
                    {
                        Commons.MQTT_CLIENT.MqttMsgPublishReceived -= activeModel.MQTT_CLIENT_MqttMsgPublishReceived;
                        Commons.MQTT_CLIENT.Disconnect();
                        activeModel.IsConnected = Commons.IS_CONNECT = false;
                    }
                }
                catch (Exception ex)
                {

                    //Pass
                }
                DeactivateItemAsync(this.ActiveItem, true);
            }
        }
    }
}
