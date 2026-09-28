using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Services
{
    public interface IModbusService
    {
        bool IsConnected { get; }
        bool Connect(string portName, int baudRate);
        void Disconnect();
        ushort[] ReadHoldingRegisters(byte slaveId, ushort start, ushort count);
        void WriteSingleRegister(byte slaveId, ushort address, ushort value);
        bool[] ReadCoils(byte slaveId, ushort start, ushort count);
    }
}
