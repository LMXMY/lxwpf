using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace lxwpf.ViewModels
{
    public class ChartViewModel : BindableBase
    {
        private readonly ObservableCollection<double> _values = new();
        private readonly Random _random = new();

        public ISeries[] Series { get; set; }

        public ChartViewModel()
        {
            Series = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = _values,
                    Name = "测试数据",
                    Fill = null
                }
            };

            //Series = new ISeries[]
            //{
            //    new LineSeries<double> { Values = new double[] { 1, 2, 3, 5, 3, 4, 6 }, Name = "设备1" },
            //    new LineSeries<double> { Values = new double[] { 6, 2, 4, 5, 1, 3, 7 }, Name = "设备2" },
            //};


        // 定时加数据
        var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            timer.Tick += (s, e) =>
            {
                _values.Add(_random.Next(0, 100));

                if (_values.Count > 50)
                    _values.RemoveAt(0);
            };
            timer.Start();
        }
    }
}