using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        ScanGraphViewModel viewModel = new ScanGraphViewModel();

        public ScanGraphExample()
        {
            InitializeComponent();
            Init();
        }
        void Init()
        {
            this.DataContext = viewModel;
        }

        private void Button_info_click(object sender, RoutedEventArgs e)
        {

        }
    }
    public class ScanGraphViewModel : INotifyPropertyChanged
    {

        public double YMax {
            get {
                return Config.Target + Config.YRange;
            }
        }
        public double YMin
        {
            get {
                return Config.Target - Config.YRange;
            }
        }
        
        public double ToleranceYMax
        {
            get {
                return Config.Target + Config.Tolerance;
            }
        }
        public double ToleranceYMin
        {
            get {
                return Config.Target - Config.Tolerance;
            }
        }
        public double CtrllineYMax
        {
            get {
                return Config.Target + Config.Ctrlline;
            }
        }
        public double CtrllineYMin
        {
            get {
                return Config.Target - Config.Ctrlline;
            }
        }

        public ChartValues<double> Values { get; } = new ChartValues<double>();
        
        public object Mapper { get; set; }

        public ScanGraphConfig Config { get; } = new ScanGraphConfig();

        public Func<double, string> Formatter { get; set; }


        public ScanGraphViewModel()
        {

            Config.PropertyChanged += Config_PropertyChanged;

            Datas2Values();

            Mapper = Mappers.Xy<double>()
                .X((value, index) => index)
                .Y((value) => value)
                .Fill((value) =>
                {
                    if (value > ToleranceYMax)
                    {
                        
                        return Config.AreaColors.ElementAt(0);
                    }
                    else if (value > CtrllineYMax)
                    {
                        return Config.AreaColors.ElementAt(1);
                    }
                    else if (value >= CtrllineYMin)
                    {
                        return Config.AreaColors.ElementAt(2);
                    }
                    else if (value >= ToleranceYMin)
                    {
                        return Config.AreaColors.ElementAt(3);
                    }
                    else
                    {
                        return Config.AreaColors.ElementAt(4);
                    }
                });

            Formatter = (y) =>
            {
                if (y == Config.Target)
                {
                    return "目标值 " + y.ToString("F1");
                }
                else if (y == ToleranceYMax)
                {
                    return "规格线 " + y.ToString("F1");
                }
                else if (y == ToleranceYMin)
                {
                    return "规格线 " + y.ToString("F1");
                }
                else if (y == CtrllineYMax)
                {
                    return "控制线 " + y.ToString("F1");
                }
                else if (y == CtrllineYMin)
                {
                    return "控制线 " + y.ToString("F1");
                }
                else
                {
                    return y.ToString("F1");
                }
            };
        }

        private void Config_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Target")
            {
                NotifyPropertyChanged("ToleranceYMax");
                NotifyPropertyChanged("ToleranceYMin");
                NotifyPropertyChanged("CtrllineYMax");
                NotifyPropertyChanged("CtrllineYMin");
                NotifyPropertyChanged("YMax");
                NotifyPropertyChanged("YMin");
            }
            else if (e.PropertyName == "Ctrlline")
            {
                NotifyPropertyChanged("CtrllineYMax");
                NotifyPropertyChanged("CtrllineYMin");
            }
            else if (e.PropertyName == "Tolerance")
            {
                NotifyPropertyChanged("ToleranceYMax");
                NotifyPropertyChanged("ToleranceYMin");
            }
            else if(e.PropertyName == "YRange")
            {
                NotifyPropertyChanged("YMax");
                NotifyPropertyChanged("YMin");
            }
        }

        void Datas2Values()
        {
            Values.Clear();
            Values.AddRange(Config.Datas);
        }

        protected void NotifyPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class ScanGraphConfig : INotifyPropertyChanged
    {
        private double _target;
        private double _ctrlline;
        private double _tolerance;
        private double _yRange;
        private int _xMin;
        private int _xMax;
        private int _analyze_XMin;
        private int _analyze_XMax;

        /// <summary>
        /// 目标值
        /// </summary>
        public double Target
        {
            get => _target;
            set
            {
                if (_target != value)
                {
                    _target = value;
                    NotifyPropertyChanged("Target");
                }
            }
        }
        /// <summary>
        /// 控制线
        /// </summary>
        public double Ctrlline
        {
            get => _ctrlline;
            set
            {
                _ctrlline = value;
                NotifyPropertyChanged("Ctrlline");
            }
        }
        /// <summary>
        /// 规格线
        /// </summary>
        public double Tolerance
        {
            get => _tolerance;
            set
            {
                _tolerance = value;
                NotifyPropertyChanged("Tolerance");
            }
        }

        /// <summary>
        /// Y轴显示范围
        /// </summary>
        public double YRange
        {
            get => _yRange;
            set
            {
                _yRange = value;
                NotifyPropertyChanged("YRange");
            }
        }
        //X轴最小值
        public int XMin
        {
            get => _xMin;
            set
            {
                _xMin = value;
                NotifyPropertyChanged("XMin");
            }
        }
        /// <summary>
        /// X轴最大值
        /// </summary>
        public int XMax
        {
            get => _xMax;
            set
            {
                _xMax = value;
                NotifyPropertyChanged("XMax");
            }
        }
        /// <summary>
        /// 分析范围最小值
        /// </summary>
        public int Analyze_XMin
        {
            get => _analyze_XMin;
            set
            {
                _analyze_XMin = value;
                NotifyPropertyChanged("Analyze_XMin");
            }
        }
        /// <summary>
        /// 分析范围最大值
        /// </summary>
        public int Analyze_XMax
        {
            get => _analyze_XMax;
            set
            {
                _analyze_XMax = value;
                NotifyPropertyChanged("Analyze_XMax");
            }
        }
        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable<double> Datas { get; set; }

        /// <summary>
        /// 显示单位
        /// </summary>
        public string UnitString { get; set; }

        #region 颜色
        public IEnumerable<Brush> AreaColors { get; set; }
        #endregion

        public ScanGraphConfig()
        {
            UnitString = "g/m²";
            Target = 150;
            Tolerance = Target * 0.02;
            Ctrlline = Tolerance * 0.6;
            YRange = Tolerance * 2;
            ChangeDatas();

            XMin = 3;
            XMax = Datas.Count() - 3;
            Analyze_XMin = XMin + 2;
            Analyze_XMax = XMax - 5;


            AreaColors = new List<Brush>
            {
                new SolidColorBrush(Colors.Red),
                new SolidColorBrush(Colors.Orange),
                new SolidColorBrush(Colors.Green),
                new SolidColorBrush(Colors.Blue),
                new SolidColorBrush(Colors.Purple)
            };


        }
        public void ChangeDatas()
        {
            int size = 60;
            double[] datas = new double[size];
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                datas[i] = (Math.Sin(i * Math.PI / size) * 3 - 1.5) * Tolerance + Target + random.NextDouble() * 1;
            }
            Datas = datas;
        }
        protected void NotifyPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
