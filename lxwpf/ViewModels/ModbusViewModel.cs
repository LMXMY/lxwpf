using lxwpf.Entities;
using lxwpf.Services;
using lxwpf.Services.ServicesImpl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace lxwpf.ViewModels
{
    public class ModbusViewModel : BindableBase
    {
        private readonly IModbusService _modbusService;
        private readonly IDeviceConfigService _deviceConfigService;

        private readonly IHistoryService _historyService;

        private readonly IAlarmService _alarmService; 

        //轮询
        private CancellationTokenSource? _cts;

        public ModbusViewModel(IModbusService modbusService, 
            IDeviceConfigService deviceConfigService,
            IHistoryService historyService,
            IAlarmService alarmService)
        {
            _modbusService = modbusService;
            _deviceConfigService = deviceConfigService;
            _historyService = historyService;
            _alarmService = alarmService;

            //链接
            ConnectCommand = new DelegateCommand(Connect);
            //断开链接
            DisconnectCommand = new DelegateCommand(Disconnect);
            //刷新
            //RefreshCommand = new DelegateCommand(Refresh);

            //RefreshCommand = new DelegateCommand(async () => await Refresh());

            RefreshCommand = new DelegateCommand(async () => await SignRefresh()); 
            //加载
            //PageLoaded = new DelegateCommand(StartPolling);
            PageLoaded = new DelegateCommand(async () => await StartPollingAsync());

            //离开界面
            PageUnLoaded = new DelegateCommand(StopPolling);

            // 加载可用串口
            PortNames = new ObservableCollection<string>(SerialPort.GetPortNames());
        }

        public ObservableCollection<string> PortNames { get; }

        private string? _selectedPort;
        public string? SelectedPort
        {
            get => _selectedPort;
            set { _selectedPort = value; RaisePropertyChanged(); }
        }

        private int _baudRate = 9600;
        public int BaudRate
        {
            get => _baudRate;
            set { _baudRate = value; RaisePropertyChanged(); }
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set { _isConnected = value; RaisePropertyChanged(); }
        }

        //Modbus列表数据
        public ObservableCollection<ModbusDataModel> Datas { get; } = new();

        public ICommand ConnectCommand { get; }
        public ICommand DisconnectCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand PageLoaded { get; }
        public ICommand PageUnLoaded { get; }

        //链接
        private void Connect()
        {
            if (string.IsNullOrEmpty(SelectedPort)) return;
            IsConnected = _modbusService.Connect(SelectedPort, BaudRate);
        }

        //断开
        private void Disconnect()
        {
            _modbusService.Disconnect();
            IsConnected = false;
        }

        //写法1
        //private void Refresh()
        //{
        //    if (!IsConnected) return;

        //    try
        //    {
        //        var values = _modbusService.ReadHoldingRegisters(1, 0, 8);
        //        Application.Current.Dispatcher.Invoke(() =>
        //        {
        //            Datas.Clear();
        //            for (int i = 0; i < values.Length; i++)
        //            {
        //                Datas.Add(new ModbusDataModel { Address = i, Value = values[i] });
        //            }
        //        });
        //    }
        //    catch { }
        //}

        //写法2
        //写死的取设备数据
        /*private async Task Refresh()
        {
            if (!IsConnected) return;
            try
            {
                var values = await Task.Run(() => _modbusService.ReadHoldingRegisters(1, 0, 8));

                // await 恢复后，再检查一次
                if (!IsConnected) return;   // ← 断开后不再更新

                Datas.Clear();
                for (int i = 0; i < values.Length; i++)
                {
                    Datas.Add(new ModbusDataModel { Address = i, Value = values[i] });
                }
            }
            catch (TaskCanceledException)
            {
                // 取消，不处理
            }
        }
        */

        //更新 刷新
        //单设备取数据
        private async Task SignRefresh()
        {
            if (!IsConnected) return;

            try
            {
                var values = await Task.Run(() => _modbusService.ReadHoldingRegisters(1, 0, 8));
                if (!IsConnected) return;   // ← 断开后不再更新

                Datas.Clear();
                for (int i = 0; i < values.Length; i++)
                {
                    Datas.Add(new ModbusDataModel { Address = i, Value = values[i] });
                }

                // 报警判断
                string deviceName = "单设备串口：" + SelectedPort;
                for (int i = 0; i < values.Length; i++)
                {
                    _alarmService.CheckAndAlarm(deviceName, (ushort)i, values[i]);
                }

                //存历史
                SignSaveHistoryIfChanged(values);

            }
            catch (Exception ex)
            {
                // 记录错误
            }

        }

        #region
        //根据配置取设备数据
        /*
        private readonly Dictionary<string, IModbusService> _services = new();

        private async Task Refresh()
        {
            var configs = _deviceConfigService.GetEnabled();
            if (configs == null || configs.Count == 0) return;

            Datas.Clear();

            foreach (var config in configs)
            {
                try
                {
                    string portName = config.PortName ?? config.DeviceName;
                    Console.WriteLine($"设备：{config.DeviceName}，串口：{portName}，从站：{config.SlaveId}");

                    if (!_services.TryGetValue(portName, out var service))
                    {
                        service = new ModbusService();
                        var ok = service.Connect(portName, config.BaudRate);
                        Console.WriteLine($"连接 {portName}：{ok}");
                        if (!ok) continue;
                        _services[portName] = service;
                    }

                    var values = await Task.Run(() => service.ReadHoldingRegisters(
                        config.SlaveId, config.StartAddress, config.ReadCount));
                    Console.WriteLine($"读到 {values.Length} 个值");

                    for (int i = 0; i < values.Length; i++)
                    {
                        Datas.Add(new ModbusDataModel
                        {
                            DeviceName = config.DeviceName,
                            Address = config.StartAddress + i,
                            Value = values[i]
                        });

                        // 存历史（值变化时才存）
                        SaveHistoryIfChanged(config, values);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"读取失败：{ex.Message}");
                }
            }
        }
        */
        #endregion

        #region
        //根据配置取设备数据
        private readonly Dictionary<string, IModbusService> _services = new();
        public ObservableCollection<DeviceDataModel> Devices { get; } = new();

        private async Task Refresh()
        {
            var configs = _deviceConfigService.GetEnabled();
            if (configs == null || configs.Count == 0) return;

            foreach (var config in configs)
            {
                try
                {
                    var service = GetOrConnectService(config);
                    if (service == null) continue;

                    var values = await Task.Run(() => service.ReadHoldingRegisters(
                        config.SlaveId, config.StartAddress, config.ReadCount));

                    UpdateDeviceData(config, values);
                }
                catch (TimeoutException)
                {
                    MarkDeviceOffline(config);   // 超时，标记离线，不弹窗
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"读取失败：{ex.Message}");
                    MarkDeviceOffline(config);
                }
            }
        }

        //多设备时获取在线的设备
        private IModbusService? GetOrConnectService(DeviceConfigModel config)
        {
            string portName = config.PortName ?? config.DeviceName ?? "";
            if (string.IsNullOrEmpty(portName)) return null;

            if (!_services.TryGetValue(portName, out var service))
            {
                service = new ModbusService();
                var ok = service.Connect(portName, config.BaudRate);
                if (!ok) return null;
                _services[portName] = service;
            }
            return service;
        }

        private async Task<ushort[]?> ReadWithRetry(IModbusService service, DeviceConfigModel config, int retryCount = 3)
        {
            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    return await Task.Run(() => service.ReadHoldingRegisters(
                        config.SlaveId, config.StartAddress, config.ReadCount));
                }
                catch (TimeoutException)
                {
                    if (i == retryCount - 1) return null;
                    await Task.Delay(200);
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        private void MarkDeviceOffline(DeviceConfigModel config)
        {
            var device = Devices.FirstOrDefault(d => d.DeviceName == config.DeviceName);
            if (device != null)
            {
                device.IsOnline = false;
            }
        }

        private void UpdateDeviceData(DeviceConfigModel config, ushort[] values)
        {
            var device = Devices.FirstOrDefault(d => d.DeviceName == config.DeviceName);
            if (device == null)
            {
                device = new DeviceDataModel { DeviceName = config.DeviceName };
                Devices.Add(device);
            }

            device.IsOnline = true;
            device.Values.Clear();

            for (int i = 0; i < values.Length; i++)
            {
                device.Values.Add(new ModbusDataModel
                {
                    Address = config.StartAddress + i,
                    Value = values[i]
                });
            }

            //多设备报警
            for (int i = 0; i < values.Length; i++)
            {
                _alarmService.CheckAndAlarm(config.DeviceName, (ushort)(config.StartAddress + i), values[i]);
            }

        }
        #endregion

        //开始轮询每秒刷新 写法1
        private void StartPolling()
        {
            _cts = new CancellationTokenSource();
            _ = Task.Run(async () =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    //Refresh();
                    SignRefresh();
                    await Task.Delay(1000);
                }
            }, _cts.Token);
        }


        //开始定时轮询每秒刷新 写法2
        private async Task StartPollingAsync()
        {
            _cts = new CancellationTokenSource();
            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    //await Refresh();
                    await SignRefresh();
                    await Task.Delay(1000, _cts.Token);
                }
            }
            catch (TaskCanceledException)
            {
                // 正常取消，忽略
            }
        }

        //停止时 停止轮询
        private void StopPolling()
        {
            _cts?.Cancel();
        }


        //数据变化时 记录的数据
        private readonly Dictionary<string, ushort[]> _lastValues = new();

        //多设备 数据变化时 保存记录数据与时间
        private void SaveHistoryIfChanged(DeviceConfigModel config, ushort[] values)
        {
            string key = config.DeviceName ?? "";

            if (_lastValues.TryGetValue(key, out var last) && last.SequenceEqual(values))
            {
                return;   // 值没变，不存
            }

            _lastValues[key] = values.ToArray();

            var records = new List<HistoryModel>();
            for (int i = 0; i < values.Length; i++)
            {
                records.Add(new HistoryModel
                {
                    DeviceName = config.DeviceName,
                    SlaveId = config.SlaveId,
                    Address = (ushort)(config.StartAddress + i),
                    Value = values[i],
                    RecordTime = DateTime.Now
                });
            }

            _historyService.AddRange(records);
        }


        private readonly Dictionary<int, ushort> _SlastValues = new();

        //单设备 数据变化时 保存记录数据与时间
        private void SignSaveHistoryIfChanged(ushort[] values)
        {
            var records = new List<HistoryModel>();

            for (int i = 0; i < values.Length; i++)
            {
                // 值没变，跳过
                if (_SlastValues.TryGetValue(i, out var last) && last == values[i])
                {
                    continue;
                }

                _SlastValues[i] = values[i];

                records.Add(new HistoryModel
                {
                    DeviceName = "单设备串口："+ SelectedPort,   // 单设备，写死或从配置读
                    SlaveId = 1,
                    Address = (ushort)i,
                    Value = values[i],
                    RecordTime = DateTime.Now
                });
            }

            if (records.Count > 0)
            {
                _historyService.AddRange(records);
            }
        }



    }
}
