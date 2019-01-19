using System;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Feng
{
    public partial class BasicColumnAxisCrossingExample : UserControl
    {
        public BasicColumnAxisCrossingExample()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new Column2Series
                {
                    Title = "2015",
                    YAxisCrossing = 30,
                    Values = new ChartValues<double> { 10, 50, 39, 50 },
                    StrokeThickness =2,
                    Stroke =  new SolidColorBrush( Colors.Black)
                }
            };

            Labels = new[] {"Maria", "Susan", "Charles", "Frida"};
            Formatter = value => value.ToString("N");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

    }
}
