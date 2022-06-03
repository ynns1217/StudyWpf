﻿using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfNaverMovieFinder.models;

namespace WpfNaverMovieFinder
{
    /// <summary>
    /// TrailerWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TrailerWindow : MetroWindow
    {
        List<YoutubeItem> youtubeItems;         //유튜부 API 검색결과 담을 리스트
        public TrailerWindow()
        {
            InitializeComponent();
        }

        public TrailerWindow(string movieName) :this()
        {
            lblMovieName.Content = $"{movieName} 예고편" ;  //
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            youtubeItems = new List<YoutubeItem>();         //초기화
            SearchYoutubeApi();

        }

        private async void SearchYoutubeApi()
        {
            await LoadDataCollection();
            lsvYoutubeSearch.ItemsSource = youtubeItems;
        }

        private async Task LoadDataCollection()
        {
            var youtubeService = new YouTubeService(
                new BaseClientService.Initializer()
                { 
                    ApiKey = "AIzaSyBYBvLh3yPrSm8vwdOnJ5Cl6mrHCeMc5kE",
                    ApplicationName = this.GetType().ToString()
                });

            var request = youtubeService.Search.List("snippet");
            request.Q = lblMovieName.Content.ToString();
            request.MaxResults = 10;        //최대 검색결과수 10개로 제한

            var response = await request.ExecuteAsync();

            //MessageBox.Show(response.ToString());     넘어오는지 확인

            foreach(var item in response.Items)
            {
                if(item.Id.Kind.Equals("youtube#video"))
                {
                    YoutubeItem youtube = new YoutubeItem(
                        item.Snippet.Title,
                        item.Snippet.ChannelTitle,
                        $"https://www.youtube.com/watch?v={item.Id.VideoId}");

                    //섬네일 이미지
                    youtube.Thumbnail = new BitmapImage(new Uri(item.Snippet.Thumbnails.Default__.Url,
                        UriKind.RelativeOrAbsolute));

                    youtubeItems.Add(youtube);
                }
            }

        }

        private void lsvYoutubeSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(lsvYoutubeSearch.SelectedItems.Count == 0)
            {
                Commons.ShowMessageAsync("유튜브", "예고편을 볼 영화를 선택하세요");
                return;
            }
            if (lsvYoutubeSearch.SelectedItems.Count > 1)
            {
                Commons.ShowMessageAsync("유튜브", "예고편을 하나만 선택하세요");
                return;
            }
            
            if(lsvYoutubeSearch.SelectedItem is YoutubeItem)
            {
                var video = lsvYoutubeSearch.SelectedItem as YoutubeItem;
                brsYoutubeWatch.Address = video.URL;
            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            brsYoutubeWatch.Address = String.Empty;
            brsYoutubeWatch.Dispose();      //리소스해제
        }
    }
}
