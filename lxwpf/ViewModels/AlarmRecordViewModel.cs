using lxwpf.Entities;
using lxwpf.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace lxwpf.ViewModels
{
    public class AlarmRecordViewModel : BindableBase
    {
        private readonly IAlarmService _alarmService;

        private CancellationTokenSource? _cts;

        public AlarmRecordViewModel(IAlarmService alarmService)
        {
            _alarmService = alarmService;
            QueryCommand = new DelegateCommand(Query);
            ClearCommand = new DelegateCommand(Clear);
            PageLoaded = new DelegateCommand(async () => await StartAutoRefresh());
            PageUnLoaded = new DelegateCommand(StopAutoRefresh);
        }

        public ObservableCollection<AlarmRecordModel> Alarms { get; } = new();
        public ICommand QueryCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand PageLoaded { get; }

        public ICommand PageUnLoaded { get; }

        private void Query()
        {
            Alarms.Clear();
            foreach (var a in _alarmService.Query(DateTime.Today, DateTime.Now))
                Alarms.Add(a);
        }

        private void Clear()
        {
            Alarms.Clear();
        }


        private async Task StartAutoRefresh()
        {
            _cts = new CancellationTokenSource();
            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    Query();
                    await Task.Delay(3000, _cts.Token);   // 每 3 秒刷新
                }
            }
            catch (TaskCanceledException) { }
        }

        private void StopAutoRefresh()
        {
            _cts?.Cancel();
        }
    }
}
