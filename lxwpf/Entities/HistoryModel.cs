using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace lxwpf.Entities
{
    [Table("history_records")]
    public class HistoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("device_name")]
        public string? DeviceName { get; set; }

        [Column("slave_id")]
        public byte SlaveId { get; set; }

        [Column("address")]
        public ushort Address { get; set; }

        [Column("value")]
        public ushort Value { get; set; }

        [Column("record_time")]
        public DateTime RecordTime { get; set; }
    }
}
