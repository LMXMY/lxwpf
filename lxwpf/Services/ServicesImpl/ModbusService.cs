using System;
using System.Collections.Generic;
using System.Text;
using Modbus.Device;
using System.IO.Ports;

namespace lxwpf.Services.ServicesImpl
{


    public class ModbusService : IModbusService
    {
        private SerialPort? _serialPort;
        private IModbusSerialMaster? _master;

        private readonly ILogService _logService;

        public bool IsConnected => _serialPort?.IsOpen ?? false;


        public bool Connect(string portName, int baudRate)
        {
            // 防重复已连接，先断开
            if (_serialPort != null && _serialPort.IsOpen)
            {
                Disconnect();
            }
            try
            {
                _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
                _serialPort.Open();

                _master = ModbusSerialMaster.CreateRtu(_serialPort);
                _master.Transport.ReadTimeout = 2000;
                _master.Transport.Retries = 3;

                return true;
            }
            catch (Exception ex)
            {
                _logService.Error("通信", $"连接 {portName} 失败", ex);
                return false;
            }
        }

        public void Disconnect()
        {
            _master?.Dispose();
            _serialPort?.Close();
            _serialPort = null;
            _master = null;
        }

        public ushort[] ReadHoldingRegisters(byte slaveId, ushort start, ushort count)
        {
            if (_master == null) throw new InvalidOperationException("未连接");
            return _master.ReadHoldingRegisters(slaveId, start, count);
        }

        public void WriteSingleRegister(byte slaveId, ushort address, ushort value)
        {
            if (_master == null) throw new InvalidOperationException("未连接");
            _master.WriteSingleRegister(slaveId, address, value);
        }

        public bool[] ReadCoils(byte slaveId, ushort start, ushort count)
        {
            if (_master == null) throw new InvalidOperationException("未连接");
            return _master.ReadCoils(slaveId, start, count);
        }
    }
}
