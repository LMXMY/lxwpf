using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Services
{
    public interface IModbusCollectService
    {
        void Start();
        void Stop();
        bool IsRunning { get; }
        event Action<string, ushort[]>? DataReceived;
        event Action<string, bool>? DeviceStatusChanged;
    }
}
