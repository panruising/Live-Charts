using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Feng
{
    public partial class ColumnRangeExample : UserControl
    {
        public ColumnRangeExample()
        {
            InitializeComponent();

            List<BoltValue> boltValues = new List<BoltValue>
            {
                new BoltValue(20, 30, 10),
                new BoltValue(30, 40, 50),
                new BoltValue(40, 60, 39),
                new BoltValue(60, 75, 50),
                new BoltValue(80, 90, 40)
            };

            
            SeriesCollection = new SeriesCollection
            {
                new ColumnRangeSeries
                {
                    Title = "2015",
                    Values = new ChartValues<BoltValue>(boltValues)
                }
            };
            //also adding values updates and animates the chart automatically
            SeriesCollection[0].Configuration =
                new LiveCharts.Configurations.ColumnRangeMapper<BoltValue>()
                .From((b) => b.From)
                .To((b) => b.To)
                .Y((b) => b.Value);

            //Labels = new[] {"Maria", "Susan", "Charles", "Frida"};
            Formatter = value => value.ToString("N");

            DataContext = this;
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
}
