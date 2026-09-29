using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace lxwpf.Entities
{
    [Table("logs")]
    public class LogModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("level")]
        public string? Level { get; set; }        // Info / Warn / Error

        [Column("module")]
        public string? Module { get; set; }       // 模块名：采集、登录、报警 

        [Column("message")]
        public string? Message { get; set; }      // 日志内容

        [Column("exception")]
        public string? Exception { get; set; }    // 异常堆栈

        [Column("log_time")]
        public DateTime LogTime { get; set; }
    }
}
