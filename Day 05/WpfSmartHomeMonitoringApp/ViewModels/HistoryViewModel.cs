using Caliburn.Micro;
using OxyPlot;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using WPFSmartHomeMonitoringApp.Helpers;
using WPFSmartHomeMonitoringApp.Models;

namespace WPFSmartHomeMonitoringApp.ViewModels
{
    class HistoryViewModel : Screen
    {
        private BindableCollection<DivisionModel> divisions;
        private DivisionModel selectedDividion;
        private string startDate;
        private string initStartDate;
        private string endDate;
        private string initEndDate;
        private int totalcount;
        private PlotModel smartHomeModel;

        /*
         * Divisions
         * DivisionVal //DivisionModel 클래스
         * SelectedDivision
         * StartDate
         * InitStartDate
         * endDate
         * initEndDate;
         * totalcount
         * smartHomeModel
         */
       

        public string StartDate
        {
            get => startDate; 
            set
            {
                startDate = value;
                NotifyOfPropertyChange(() => StartDate);
            }
        }
        public string InitStartDate
        {
            get => initStartDate; 
            set
            {
                initStartDate = value;
                NotifyOfPropertyChange(() => InitStartDate);

            }
        }
        public string EndDate
        {
            get => endDate; 
            set
            {
                endDate = value;
                NotifyOfPropertyChange(() => EndDate);

            }
        }
        public string InitEndDate
        {
            get => initEndDate; 
            set
            {
                initEndDate = value;
                NotifyOfPropertyChange(() => InitEndDate);

            }
        }
        public int Totalcount
        {
            get => totalcount; 
            set
            {
                totalcount = value;
                NotifyOfPropertyChange(() => Totalcount);

            }
        }
        public PlotModel SmartHomeModel
        {
            get => smartHomeModel; 
            set
            {
                smartHomeModel = value;
                NotifyOfPropertyChange(() => SmartHomeModel);

            }
        }
        internal BindableCollection<DivisionModel> Divisions
        {
            get => divisions; 
            set
            {
                divisions = value;
                NotifyOfPropertyChange(() => Divisions);

            }
        }
        internal DivisionModel SelectedDividion
        {
            get => selectedDividion; 
            set
            {
                selectedDividion = value;
                NotifyOfPropertyChange(() => SelectedDividion);

            }
        }

        public HistoryViewModel()
        {
            Commons.CONNSTRING = "Data Source=PC01;Initial Catalog=OpenApiLab;Integrated Security=True;";
            InitControl();

        }

        private void InitControl()
        {
            //DB없이 툴바에 넣기
            Divisions = new BindableCollection<DivisionModel>   //콤보박스용 데이터 생성
            {
                new DivisionModel{ KeyVal = 0, DivisionVal ="-- Select --" },
            new DivisionModel { KeyVal = 1, DivisionVal = "DINNING" },
            new DivisionModel { KeyVal = 2, DivisionVal = "LIVING" },
            new DivisionModel { KeyVal = 3, DivisionVal = "BED" },
            new DivisionModel { KeyVal = 4, DivisionVal = "BATH" },
        };
            //Select를 선택해서 초기화
            selectedDividion = Divisions.Where(ValueTuple => ValueTuple.DivisionVal.Contains("Select")).FirstOrDefault();

            InitStartDate = DateTime.Now.ToShortDateString(); //2022-06-10
            InitEndDate = DateTime.Now.AddDays(+1).ToShortDateString();//2022-06-11
        }

        //검색 메서드
        public void SearchIoTData()
        {
            if(selectedDividion.KeyVal==0)      //Select
            {
                MessageBox.Show("검색할 방을 선택하세요.");
                return;
            }

            if(DateTime.Parse(StartDate) > DateTime.Parse(EndDate))
            {
                MessageBox.Show("시작일이 종료일보다 최신일 수 없습니다.");
                return;
            }

            totalcount = 0;
            using(SqlConnection conn = new SqlConnection(Commons.CONNSTRING))
            {
                string strQuery = @"SELECT Id, CurrTime, Temp, Humid
                                    FROM  TblSmartHome
                                    WHERE DevId = @DevId
                                     AND CurrTime BETWEEN @StartDate AND @EndDate
                                    ORDER BY Id ASC";

                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(strQuery, conn);

                    SqlParameter parmDevId = new SqlParameter("@DevId",SelectedDividion.DivisionVal);
                    cmd.Parameters.Add(parmDevId);

                    SqlParameter parmStartDate = new SqlParameter("@StartDate", StartDate);
                    cmd.Parameters.Add(parmStartDate);

                    SqlParameter parmEndDate = new SqlParameter("@EndDate", EndDate);
                    cmd.Parameters.Add(parmEndDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    var i = 0;
                    while(reader.Read())
                    {
                        var temp = reader["Temp"];
                        //Temp,Humid 차트데이터를 생성

                        i++;
                    }

                    Totalcount = i;

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error {ex.Message}");
                    return;

                    throw;
                }

            }
                 

        }
    }
}
