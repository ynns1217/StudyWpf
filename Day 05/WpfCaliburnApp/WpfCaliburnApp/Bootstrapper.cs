using Caliburn.Micro;
using System.Windows;
using WpfCaliburnApp.ViewModels;

namespace WpfCaliburnApp
{

    //시작 윈도우 지정하기 위한 클래스
    class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            //base.OnStartup(sender, e);
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
