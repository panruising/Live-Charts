﻿using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf.CartesianChart.Feng
{
    /// <summary>
    /// ScanGraphExample.xaml 的交互逻辑
    /// </summary>
    public partial class ScanGraphExample : UserControl
    {
        ScanGraphViewModel viewModel;

        public ScanGraphExample()
        {
            InitializeComponent();
            Init();
        }
        ChartValues<double> Values = new ChartValues<double>();

        void Init()
        {
            viewModel = new ScanGraphViewModel();
            this.DataContext = viewModel;

            Datas2Values();
            column2Series.Values = Values;

            DataContext = this;
        }
        void Datas2Values()
        {
            Values.Clear();
            Values.AddRange(viewModel.Datas);
        }
        private void Button_info_click(object sender, RoutedEventArgs e)
        {

        }
    }
    public class ScanGraphViewModel : INotifyPropertyChanged
    {


        /// <summary>
        /// 目标值
        /// </summary>
        public double Target { get; set; }
        /// <summary>
        /// 控制线
        /// </summary>
        public double Ctrlline { get; set; }
        /// <summary>
        /// 规格线
        /// </summary>
        public double Tolerance { get; set; }

        /// <summary>
        /// Y轴显示范围
        /// </summary>
        public double YRange { get; set; }
        //X轴最小值
        public int XMin { get; set; }
        /// <summary>
        /// X轴最大值
        /// </summary>
        public int XMax { get; set; }
        /// <summary>
        /// 分析范围最小值
        /// </summary>
        public int Analyze_XMin { get; set; }
        /// <summary>
        /// 分析范围最大值
        /// </summary>
        public int Analyze_XMax { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<double> Datas { get; set; }

        /// <summary>
        /// 显示单位
        /// </summary>
        public string UnitString { get; set; }




        #region 颜色
        public IEnumerable<Color> AreaColors { get; set; }
        #endregion

        public ScanGraphViewModel()
        {
            UnitString = "g/m²";
            Target = 150;
            Tolerance = Target * 0.02;
            Ctrlline = Tolerance * 0.6;

            ChangeDatas();

            XMin = 3;
            XMax = Datas.Count() - 3;
            Analyze_XMin = XMin + 2;
            Analyze_XMax = XMax - 5;


            AreaColors = new List<Color>
            {
                Colors.Red,
                Colors.Orange,
                Colors.Green,
                Colors.Blue,
                Colors.Purple
            };
        }
        public void ChangeDatas()
        {
            int size = 80;
            double[] datas = new double[size];
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                datas[i] = (Math.Sin(i * Math.PI / size) * 3 - 1.5) * Tolerance + Target + random.NextDouble() * 1;
            }
            Datas = datas;
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
