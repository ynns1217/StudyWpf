using MahApps.Metro.Controls;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Input;

namespace WpfNaverNewsSearch 
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                //Commons.ShowMessageAsync("실행", "뉴스검색 실행!");
                SearchNaverNews();

            }
        }

        private void SearchNaverNews()
        {
            string keyword = txtSearch.Text;
            string clientID = "_spuQorY5ZmHUrSJL1fz";
            string clientSecret = "6AL8wofCD0";
            string openApiUri = $"https://openapi.naver.com/v1/search/news.json?start=1&display=5&query={keyword}";
            string result;

            WebRequest request = null;
            WebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;

            //Naver OpenAPI 실제 요청
            try
            {
                request = WebRequest.Create(openApiUri);
                request.Headers.Add("X-Naver-Client-Id", clientID); //중요
                request.Headers.Add("X-Naver-Client-Secret", clientSecret); //중요

                response = request.GetResponse();
                stream = response.GetResponseStream();
                reader = new StreamReader(stream);

                result = reader.ReadToEnd();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                reader.Close();
                stream.Close();
                response.Close();
            }

            MessageBox.Show(result);

        }
    }
}
