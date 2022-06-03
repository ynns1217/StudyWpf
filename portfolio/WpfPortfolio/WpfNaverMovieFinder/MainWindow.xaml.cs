using MahApps.Metro.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        bool IsFavorite = false;        //네이버 api로 검색한건지, 즐겨찾기 DB에서 가져온건지 확인하는 값
        //IsFavorite == True 면 DB에서 온 값
        //IsFavorite == False 면 네이버 api로 검색해서 나온 값
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
                IsFavorite = false;     //api로 검색했으므로 
            }
            catch (System.Exception ex)
            {

                Commons.ShowMessageAsync("예외", $"예외발생 :{ex}");
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
                request.Headers.Add("X-Naver-Client-Secret",clientSecret); //중요

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

        //즐겨찾기 추가
        private void btnAddWatchList_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(grdResult.SelectedItems.Count == 0)
            {
                Commons.ShowMessageAsync("오류", "즐겨찾기에 추가할 영화를 선택하세요(복수선택 가능)");
                return;
            }

            if (IsFavorite == true)
            {
                Commons.ShowMessageAsync("오류", "이미 즐겨찾기한 영화입니다!");
                return;
            }

            List<TblFavoriteMovies> list = new List<TblFavoriteMovies>();       //FavoriteMovieItem(x)
            foreach (MovieItem item in grdResult.SelectedItems)
            {
                TblFavoriteMovies temp = new TblFavoriteMovies()
                {
                    Title =item.Title,
                    Link = item.Link,
                    Image = item.Image,
                    SubTitle = item.SubTitle,
                    PubDate = item.PubDate,
                    Director = item.Director,
                    Actor = item.Actor,
                    UserRating = item.UserRating,
                    RegDate = DateTime.Now
                };


                list.Add(temp);
            }

            //EF 테이블 데이터 입력(INSERT)
            try
            {
                using (var ctx = new OpenApiLabEntities())
                {
                    foreach (var item in list)
                    {
                        ctx.Set<TblFavoriteMovies>().Add(item);     //쿼리INSERT문 대체
                    }
                    ctx.SaveChanges();      //commit
                }

                Commons.ShowMessageAsync("저장", "즐겨찾기 추가 성공!!");
            }
            catch (Exception ex)
            {

                Commons.ShowMessageAsync("예외", $"예외발생 : {ex}");
            }
        }

        //즐겨찾기 조회
        private void btnViewWatchList_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DataContext = null;
            txtSearchName.Text = String.Empty;

            List<TblFavoriteMovies> list = new List<TblFavoriteMovies>();           //리스트 초기화

            try
            {
                using (var ctx = new OpenApiLabEntities())
                {
                    list = ctx.TblFavoriteMovies.ToList();          //디비에서 데이터 갖고오고
                }

                this.DataContext = list;                            //리스트에 뿌리기
                stsResult.Content = $"즐겨찾기 {list.Count}개 조회";       //바에 출력
                Commons.ShowMessageAsync("즐겨찾기", "즐겨찾기 조회 완료!");
                IsFavorite = true; //db에서 가져왔으므로
            }
            catch (Exception ex)
            {

                Commons.ShowMessageAsync("예외", $"예외발생 : {ex}");
                IsFavorite = false; //예외가 생겼으므로 초기화해줌
            }
        }

        //즐겨찾기 삭제
        private void btnDelWatchList_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(IsFavorite == false)
            {
                Commons.ShowMessageAsync("오류", "즐겨찾기 내요이 아니면 삭제할 수 없습니다.");
                return;
            }
            if(grdResult.SelectedItems.Count == 0)
            {
                Commons.ShowMessageAsync("오류", "삭제할 영화를 선택하세요.");
                return;
            }
            foreach (TblFavoriteMovies item in grdResult.SelectedItems)
            {
                using (var ctx = new OpenApiLabEntities())
                {
                    var delItem = ctx.TblFavoriteMovies.Find(item.Idx);     //PK
                    ctx.Entry(delItem).State = System.Data.EntityState.Deleted;     //객체상태를 삭제상태로 변경
                    ctx.SaveChanges();      //commit
                }
            }

            btnViewWatchList_Click(sender, e);      //즐겨찾기보기 버튼 클릭 이벤트 실행
        }

        //유튜브 예고펴 동영상 보기
        private void btnWatchTrailer_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (grdResult.SelectedItems.Count == 0)
            {
                Commons.ShowMessageAsync("유튜브 영화", "영화를 선택하세요");
                return;
            }
            if (grdResult.SelectedItems.Count > 1)
            {
                Commons.ShowMessageAsync("유튜브 영화", "영화를 하나만 선택하세요");
                return;
            }

            string movieName = string.Empty; //""; 이랑 같다

            if (IsFavorite == true)     //즐겨찾기 db값이면
            {
                movieName = (grdResult.SelectedItem as TblFavoriteMovies).Title;
            }
            else
            {
                movieName = (grdResult.SelectedItem as MovieItem).Title;        //한글 영화제목을 가져온다
            }

            var trailerWindow = new TrailerWindow(movieName);       //영화제목 받는 생성자 변경
            trailerWindow.Owner = this;     //MainWindow
            trailerWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;    //trailerWindow 자식 윈도우를 띄우기위한 부모윈도우는 메인 윈도우// 부모윈도우 정중앙에 자식윈도우를 띄우겠다
            trailerWindow.ShowDialog();     //모달창 Modal (자식창이 열려있을 때 부모창은 클릭이 안된다)
       
            
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
            if(grdResult.SelectedItem is TblFavoriteMovies)  
            {
                var movie = grdResult.SelectedItem as TblFavoriteMovies;        //클래스 명만 바꿔주면된다
                if (string.IsNullOrEmpty(movie.Image))
                {
                    imgPoster.Source = new BitmapImage(new Uri("/resource/No_Picture.jpg", UriKind.RelativeOrAbsolute));
                }
                else
                {
                    imgPoster.Source = new BitmapImage(new Uri(movie.Image, UriKind.RelativeOrAbsolute));
                }
            }
        }

        //네이버 영화 웹브라우저 열기
        private void btnNaverMovie_Click(object sender, RoutedEventArgs e)
        {
            
            //검색하고 선택 후 네이버 영화들어가기
            if(grdResult.SelectedItems.Count == 0)
            {
                Commons.ShowMessageAsync("네이버 영화", "영화를 선택하세요");
                return;
            }
            if (grdResult.SelectedItems.Count > 1)
            {
                Commons.ShowMessageAsync("네이버 영화", "영화를 하나만 선택하세요");
                return;
            }


            string linkUrl = String.Empty;
            
            if(IsFavorite == true)
            {
                linkUrl = (grdResult.SelectedItem as TblFavoriteMovies).Link;
            }
            else
            {
                linkUrl = (grdResult.SelectedItem as MovieItem).Link;
            }

            Process.Start(linkUrl);
        }
    }
}

