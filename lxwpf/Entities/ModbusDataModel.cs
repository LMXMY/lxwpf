using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Entities
{
    public class ModbusDataModel
    {
        public string? DeviceName { get; set; }
        public int Address { get; set; }
        public ushort Value { get; set; }
    }
}
