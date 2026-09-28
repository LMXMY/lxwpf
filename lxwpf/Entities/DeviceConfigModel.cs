using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace lxwpf.Entities
{
    [Table("device_configs")]
    public class DeviceConfigModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("device_name")]
        public string? DeviceName { get; set; }

        [Column("slave_id")]
        public byte SlaveId { get; set; }

        [Column("start_address")]
        public ushort StartAddress { get; set; }

        [Column("read_count")]
        public ushort ReadCount { get; set; }

        [Column("interval_ms")]
        public int Interval { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        [Column("port_name")]
        public string? PortName { get; set; }

        [Column("baud_rate")]
        public int BaudRate { get; set; }
    }
}
