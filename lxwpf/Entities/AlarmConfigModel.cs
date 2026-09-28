using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace lxwpf.Entities
{
    [Table("alarm_configs")]
    public class AlarmConfigModel
    {
        public int Id { get; set; }
        public string? DeviceName { get; set; }
        public ushort Address { get; set; }
        public ushort UpperLimit { get; set; }
        public ushort LowerLimit { get; set; }
        public bool IsEnabled { get; set; }
    }

    [Table("alarm_records")]
    public class AlarmRecordModel
    {
        public int Id { get; set; }
        public string? DeviceName { get; set; }
        public ushort Address { get; set; }
        public ushort Value { get; set; }
        public string? Message { get; set; }
        public DateTime AlarmTime { get; set; }
    }
}
