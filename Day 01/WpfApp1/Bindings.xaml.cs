﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Bindings.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Bindings : Page
    {
        public Bindings()
        {
            InitializeComponent();      //화면구성

            Car c = new Car
            {
                Speed = 100,
                Color = Colors.Crimson,
                Driver = new Human
                {
                    FirstName = "Nick",
                    HasDrivingLicense = true
                },
            };

           this.DataContext = c;       //필수필요! 이 데이터를 xaml로 보내야함
            //txtSpeed.Text = c.Speed.ToString();     //고전적인윈폼 방식

            var carlist = new List<Car>();
            for (int i = 0; i < 10; i++)
            {
                carlist.Add(new Car { 
                    Speed = i*10,
                    Color = Colors.Purple,
                });

            }

            lbxCars.DataContext = carlist;

        }
    }
}
