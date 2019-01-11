using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Configurations;
using System.Linq;
using System.Collections.Generic;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace Wpf.CartesianChart.ConstantChanges
{
    public partial class ConstantChangesChart : UserControl, INotifyPropertyChanged
    {
        private double _axisMax;
        private double _axisMin;
        private double _trend;

        public ConstantChangesChart()
        {
            InitializeComponent();

            //To handle live data easily, in this case we built a specialized type
            //the MeasureModel class, it only contains 2 properties
            //DateTime and Value
            //We need to configure LiveCharts to handle MeasureModel class
            //The next code configures MeasureModel  globally, this means
            //that LiveCharts learns to plot MeasureModel and will use this config every time
            //a IChartValues instance uses this type.
            //this code ideally should only run once
            //you can configure series in many ways, learn more at 
            //http://lvcharts.net/App/examples/v1/wpf/Types%20and%20Configuration

            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);

            Datas = new List<ChartValues<MeasureModel>>();
            Series = new SeriesCollection();
            for (int i = 0; i < 5; i++) {

                ChartValues<MeasureModel> cv = new ChartValues<MeasureModel>();
                Datas.Add(cv);
                Series.Add(
                    new LineSeries()
                    {
                        Values = cv,
                        PointGeometry = null,
                        LineSmoothness = 1,
                        StrokeThickness = 6,
                        Fill = new SolidColorBrush( System.Windows.Media.Colors.Transparent)
                    });
            }
            //the values property will store our values array
            //ChartValues = new ChartValues<MeasureModel>();
            //ChartValues2 = new ChartValues<MeasureModel>();
            //lets set how to display the X Labels
            DateTimeFormatter = value => new DateTime((long) value).ToString("mm:ss");

            //AxisStep forces the distance between each separator in the X axis
            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            //AxisUnit forces lets the axis know that we are plotting seconds
            //this is not always necessary, but it can prevent wrong labeling
            AxisUnit = TimeSpan.TicksPerSecond;

            SetAxisLimits(DateTime.Now);

            //The next code simulates data changes every 300 ms
            
            IsReading = false;

            DataContext = this;
        }
        public SeriesCollection Series { get; set; }
        public List<ChartValues<MeasureModel>> Datas { get; set; }
        public ChartValues<MeasureModel> ChartValues { get; set; }
        public ChartValues<MeasureModel> ChartValues2 { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }

        TimeSpan elapsed;
        public TimeSpan Elapsed
        {
            get
            {
                return elapsed;
            }
            set {
                if (elapsed != value)
                {
                    elapsed = value;
                    OnPropertyChanged("Elapsed");
                }
            }
        }
        public double AxisMax
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                OnPropertyChanged("AxisMax");
            }
        }
        public double AxisMin
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                OnPropertyChanged("AxisMin");
            }
        }

        double yaxismax=100;
        double yaxismin=-10;
        public double YAxisMax
        {
            get { return yaxismax; }
            set
            {
                if (yaxismax != value)
                {
                    yaxismax = value;
                    OnPropertyChanged("YAxisMax");
                }
            }
        }
        public double YAxisMin
        {
            get { return yaxismin; }
            set
            {
                if (yaxismin != value)
                {
                    yaxismin = value;
                    OnPropertyChanged("YAxisMin");
                }
            }
        }
        public bool IsReading { get; set; }

        private void Read()
        {

            List<MeasureModel>[] lists = new List<MeasureModel>[Datas.Count()];
            for (int i = 0; i < lists.Count(); i++)
            {
                lists[i] = new List<MeasureModel>();
            }
            int cnt = 0;
            var r = new Random();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (IsReading)
            {
                var now = DateTime.Now;
                
                _trend = r.Next(-8, 10);
                for (int i = 0; i < lists.Count(); i++)
                {
                    lists[i].Add(new MeasureModel
                    {
                        DateTime = now,
                        Value = r.Next(-8, 10) + i*10
                    });
                }

                cnt++;
                if (stopwatch.Elapsed > TimeSpan.FromSeconds(1))
                {
                    for (int i = 0; i < lists.Count(); i++)
                    {
                        Datas[i].AddRange(lists[i]);
                        lists[i].Clear();

                        while (Datas[i].First().DateTime < now - TimeSpan.FromSeconds(8))
                        {
                            Datas[i].RemoveAt(0);
                        }
                    }

                    

                    SetAxisLimits(now);

                    if (cnt > 0)
                    {
                        Elapsed = TimeSpan.FromTicks(stopwatch.Elapsed.Ticks / cnt);
                        cnt = 0;
                    }
                    else
                    {
                        Elapsed = TimeSpan.MaxValue;
                    }
                    stopwatch.Restart();
                }
                //double max = ChartValues.Max(m => m.Value);
                //double min = ChartValues.Min(m => m.Value);
                //if (Math.Abs(YAxisMax - max) > Math.Abs(max - min) / 10)
                //{
                //    YAxisMax = max;
                //    YAxisMin = min;
                //}
                

                Thread.Sleep(3);
            }
        }

        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(1).Ticks; // lets force the axis to be 1 second ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(8).Ticks; // and 8 seconds behind
        }
        
        private void InjectStopOnClick(object sender, RoutedEventArgs e)
        {
            IsReading = !IsReading;
            if (IsReading) Task.Factory.StartNew(Read);
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
