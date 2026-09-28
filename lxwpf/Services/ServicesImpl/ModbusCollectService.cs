using lxwpf.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Services.ServicesImpl
{
    public class ModbusCollectService : IModbusCollectService
    {
        private readonly IDeviceConfigService _deviceConfigService;
        private readonly IHistoryService _historyService;
        private readonly IAlarmService _alarmService;

        private readonly Dictionary<string, IModbusService> _services = new();
        private readonly Dictionary<string, ushort[]> _lastValues = new();
        private CancellationTokenSource? _cts;

        public bool IsRunning => _cts != null && !_cts.IsCancellationRequested;

        public event Action<string, ushort[]>? DataReceived;
        public event Action<string, bool>? DeviceStatusChanged;

        public ModbusCollectService(
            IDeviceConfigService deviceConfigService,
            IHistoryService historyService,
            IAlarmService alarmService)
        {
            _deviceConfigService = deviceConfigService;
            _historyService = historyService;
            _alarmService = alarmService;
        }

        public void Start()
        {
            if (IsRunning) return;

            _cts = new CancellationTokenSource();
            _ = Task.Run(async () =>
            {
                try
                {
                    while (!_cts.IsCancellationRequested)
                    {
                        await CollectOnce();
                        await Task.Delay(1000, _cts.Token);
                    }
                }
                catch (TaskCanceledException) { }
            });
        }

        public void Stop()
        {
            _cts?.Cancel();
            _cts = null;

            foreach (var service in _services.Values)
                service.Disconnect();
            _services.Clear();
        }

        private async Task CollectOnce()
        {
            var configs = _deviceConfigService.GetEnabled();
            if (configs == null || configs.Count == 0) return;

            foreach (var config in configs)
            {
                try
                {
                    var service = GetOrConnectService(config);
                    if (service == null)
                    {
                        DeviceStatusChanged?.Invoke(config.DeviceName ?? "", false);
                        continue;
                    }

                    var values = await Task.Run(() => service.ReadHoldingRegisters(
                        config.SlaveId, config.StartAddress, config.ReadCount));

                    if (values.Length == 0)
                    {
                        DeviceStatusChanged?.Invoke(config.DeviceName ?? "", false);
                        continue;
                    }

                    // 通知在线
                    DeviceStatusChanged?.Invoke(config.DeviceName ?? "", true);

                    // 存历史
                    SaveHistoryIfChanged(config, values);

                    // 判断报警
                    for (int i = 0; i < values.Length; i++)
                    {
                        _alarmService.CheckAndAlarm(
                            config.DeviceName ?? "",
                            (ushort)(config.StartAddress + i),
                            values[i]);
                    }

                    // 通知界面
                    DataReceived?.Invoke(config.DeviceName ?? "", values);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"采集失败：{ex.Message}");
                    DeviceStatusChanged?.Invoke(config.DeviceName ?? "", false);
                }
            }
        }

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

        private void SaveHistoryIfChanged(DeviceConfigModel config, ushort[] values)
        {
            string key = config.DeviceName ?? "";

            if (_lastValues.TryGetValue(key, out var last) && last.SequenceEqual(values))
                return;

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
    }
}
