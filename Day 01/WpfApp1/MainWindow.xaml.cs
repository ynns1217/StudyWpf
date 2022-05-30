using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitClass();    //먼저 선언하고 ALT+ENTER으로 메서드 만들기
        }

        private void InitClass()
        {
            Car car = new Car();
        }
    }
}
