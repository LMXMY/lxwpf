using lxwpf.Entities;
using lxwpf.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace lxwpf.ViewModels
{
    public class HistoryRecordViewModel : BindableBase
    {
        private readonly IHistoryService _historyService;

        public HistoryRecordViewModel(IHistoryService historyService)
        {
            _historyService = historyService;
            QueryCommand = new DelegateCommand(Query);
            ExportCommand = new DelegateCommand(Export);
            PageLoaded = new DelegateCommand(Query);
        }

        public ObservableCollection<HistoryModel> Records { get; } = new();

        private DateTime _startTime = DateTime.Today;
        public DateTime StartTime
        {
            get => _startTime;
            set { _startTime = value; RaisePropertyChanged(); }
        }

        private DateTime _endTime = DateTime.Now.AddDays(1);
        public DateTime EndTime
        {
            get => _endTime;
            set { _endTime = value; RaisePropertyChanged(); }
        }

        private string? _deviceName;
        public string? DeviceName
        {
            get => _deviceName;
            set { _deviceName = value; RaisePropertyChanged(); }
        }

        public ICommand QueryCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand PageLoaded { get; }

        private void Query()
        {
            Records.Clear();
            foreach (var r in _historyService.Query(StartTime, EndTime, DeviceName))
                Records.Add(r);
        }


        // 导出 CSV
        /*private void Export()
        {
            if (Records.Count == 0)
            {
                MessageBox.Show("没有数据可导出");
                return;
            }

            var dialog = new SaveFileDialog
            {
                Filter = "CSV 文件|*.csv",
                FileName = $"历史记录_{DateTime.Now:yyyyMMddHHmmss}.csv"
            };

            if (dialog.ShowDialog() == true)
            {
                _historyService.ExportToCsv(Records.ToList(), dialog.FileName);
                MessageBox.Show("导出成功");
            }
        }*/

        //Excel
        private void Export()
        {
            if (Records.Count == 0)
            {
                MessageBox.Show("没有数据可导出");
                return;
            }

            var dialog = new SaveFileDialog
            {
                Filter = "Excel 文件|*.xlsx",
                FileName = $"历史记录_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
            };

            if (dialog.ShowDialog() == true)
            {
                _historyService.ExportToExcel(Records.ToList(), dialog.FileName);
                MessageBox.Show("导出成功");
            }
        }
    }
}
