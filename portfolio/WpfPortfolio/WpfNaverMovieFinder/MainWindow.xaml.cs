using MahApps.Metro.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfNaverMovieFinder;
using WpfNaverMovieFinder.models;

namespace WpfMahAppTest
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public object Jobject { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void txtSearchName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter) btnSearch_Click(sender, e);
        }

        private void btnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            stsResult.Content = string.Empty;

          // if(txtSearchName.Text == ""|| txtSearchName.Text ==null)
          if (string.IsNullOrEmpty(txtSearchName.Text))
            {
                stsResult.Content = "검색할 영화명을 입력, 검색버튼을 눌러주세요";
                //MessageBox.Show("검색할 영화명을 입력, 검색버튼을 눌러주세요");
                Commons.ShowMessageAsync("검색", "검색할 영화명을 입력, 검색버튼을 눌러주세요");
                return;
            }
            //검색 시작
            //Commons.ShowMessageAsync("결과", $"{txtSearchName.Text}");
            try
            {
                SearchNaverOpenApi(txtSearchName.Text);
                Commons.ShowMessageAsync("검색", "영화 검색 완료!!");
            }
            catch (System.Exception)
            {

                //
            }

        }

        //네이버 실제 검색 메서드
        private void SearchNaverOpenApi(string searcName)
        {
            string clientID = "_spuQorY5ZmHUrSJL1fz";
            string clientSecret = "6AL8wofCD0";
            string openApiUri = $"https://openapi.naver.com/v1/search/movie?start=1&display=30&query={searcName}";
            string result = string.Empty; //빈값 초기화

            WebRequest request = null;
            WebResponse response = null;
            Stream stream = null;
            StreamReader reader = null;

            //Naver OpenAPI 실제 요청
            try
            {
                request = WebRequest.Create(openApiUri);
                request.Headers.Add("X-Naver-Client-Id",clientID); //중요
                request.Headers.Add("X-Naver-Client-SEcret",clientSecret); //중요

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

            //MessageBox.Show(result);

            var parsedJson = JObject.Parse(result);
            int total = Convert.ToInt32(parsedJson["total"]);           //전체 검색 결과 수
            int display = Convert.ToInt32(parsedJson["display"]);

            stsResult.Content = $"{total} 중 {display} 호출 성공!";     //상태바에서 확인

            //데이터 그리드에 검색결과 할당
            var items = parsedJson["items"];        //items밑에 것들을 배열로 저장
            var json_array = (JArray)items;

            List<MovieItem> movieItems = new List<MovieItem>();

            foreach (var item in json_array)
            {
                MovieItem movie = new MovieItem(
                    Regex.Replace(item["title"].ToString(), @"<(.|\n)*?>", string.Empty),
                    item["link"].ToString(),
                    item["image"].ToString(),
                    item["subtitle"].ToString(),
                    item["pubDate"].ToString(),
                    item["director"].ToString().Replace("|",","),
                    item["actor"].ToString().Replace("|", ","),
                    item["userRating"].ToString());
                movieItems.Add(movie);
            }
            this.DataContext = movieItems;
        }

        private void btnAddWatchList_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnViewWatchList_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnDelWatchList_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void btnWatchTrailer_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }


        //선택한 영화의 포스터 보이기
        private void grdResult_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if(grdResult.SelectedItem is MovieItem)
            {
                var movie = grdResult.SelectedItem as MovieItem;
                if(string.IsNullOrEmpty(movie.Image))
                {
                    imgPoster.Source = new BitmapImage(new Uri("/resource/No_Picture.jpg",UriKind.RelativeOrAbsolute));
                }
                else
                {
                    imgPoster.Source = new BitmapImage(new Uri(movie.Image, UriKind.RelativeOrAbsolute));
                }
            }
        }
    }
}

