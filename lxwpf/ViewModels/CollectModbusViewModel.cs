using lxwpf.Entities;
using lxwpf.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace lxwpf.ViewModels
{
    //采集服务
    public class CollectModbusViewModel : BindableBase
    {
        private readonly IModbusCollectService _collectService;

        public CollectModbusViewModel(IModbusCollectService collectService)
        {
            _collectService = collectService;
            _collectService.DataReceived += OnDataReceived;
            _collectService.DeviceStatusChanged += OnDeviceStatusChanged;
        }

        public ObservableCollection<ModbusDataModel> Datas { get; } = new();

        private void OnDataReceived(string deviceName, ushort[] values)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Datas.Clear();
                for (int i = 0; i < values.Length; i++)
                {
                    Datas.Add(new ModbusDataModel
                    {
                        DeviceName = deviceName,
                        Address = i,
                        Value = values[i]
                    });
                }
            });
        }

        private void OnDeviceStatusChanged(string deviceName, bool isOnline)
        {
            // 更新状态
        }
    }
}
