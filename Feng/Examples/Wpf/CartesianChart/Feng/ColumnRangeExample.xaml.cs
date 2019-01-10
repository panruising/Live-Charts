using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System.Linq;

namespace Wpf.CartesianChart.Feng
{
    public partial class ColumnRangeExample : UserControl
    {
        List<double> datas = new List<double>();
        List<Bolt> boltmap = new List<Bolt>();

        public ColumnRangeExample()
        {
            InitializeComponent();

            Init();
            List<BoltValue> boltValues = new List<BoltValue>();
            foreach (Bolt bolt in boltmap)
            {
                IEnumerable<double> a = datas.Skip(bolt.Begin - 1).Take(bolt.End - bolt.Begin + 1);

                a = from d in a where !double.IsNaN(d) select d;
                
                boltValues.Add(new BoltValue(bolt.Begin, bolt.End, a.Average()));
            }


            //Values = "{Binding Values}" LineSmoothness = "1" StrokeThickness = "10"
            //                    DataLabels = "True" FontSize = "20" Foreground = "#6B303030"
            //                    Stroke = "White" Fill = "Transparent" PointGeometrySize = "0"
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Line",
                    Values = new ChartValues<double>(datas),
                    StrokeThickness = 2,
                    Stroke = new SolidColorBrush(System.Windows.Media.Colors.Blue),
                    Fill = new SolidColorBrush(System.Windows.Media.Colors.Transparent),
                    PointGeometry = null,
                },

                new ColumnRangeSeries
                {
                    Title = "ColumnRange",
                    Values = new ChartValues<BoltValue>(boltValues),
                    StrokeThickness = 2,
                    Stroke = new SolidColorBrush(System.Windows.Media.Colors.DarkRed),
                    Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(0x80, 0xff,0,0)),
                    DataLabels = true,
                    Configuration = Mappers.Weighted<BoltValue>()
                    .X(b=>(b.From + b.To)/2)
                    .Weight(b=> Math.Abs(b.From-b.To))
                    .Y(b=>b.Value)


                }
            };
            
            //Labels = new[] {"Maria", "Susan", "Charles", "Frida"};
            Formatter = value => value.ToString("N");
            chart.AxisX[0].MinValue = 0;
            chart.AxisX[0].MaxValue = 100;

            DataContext = this;
        }

        void Init()
        {
            double target = 150;
            double tolerance = 2;

            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                double d = (random.NextDouble() - 0.5) * 2 * tolerance + target;
                datas.Add(d);
            }
            //把其中一部分数据变为无效
            for (int i = 30; i < 40; i++)
                datas[i] = double.NaN;

            boltmap.Add(new Bolt(20, 30));
            boltmap.Add(new Bolt(28, 42));
            boltmap.Add(new Bolt(40, 60));
            boltmap.Add(new Bolt(60, 75));
            boltmap.Add(new Bolt(80, 90));
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

    }

    public class BoltValue:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double Value { get; set; }
        public double From { get; set; }
        public double To { get; set; }

        public BoltValue() { }
        public BoltValue(double from, double to, double value)
        {
            From = from;
            To = to;
            Value = value;
        }
    }

    public class Bolt
    {
        public int Begin;
        public int End;
        public Bolt(int b,int e)
        {
            Begin = b;
            End = e;
        }
    }
}
