﻿using System.Windows;namespace WpfBasicApp{    /// <summary>    /// MainWindow.xaml에 대한 상호 작용 논리    /// </summary>    public partial class MainWindow : Window    {        public MainWindow()        {            InitializeComponent();        }

        private void btn(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello,WPF!!", "First WPF");
        }
    }}