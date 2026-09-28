using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace lxwpf.Entities
{
    [Table("users")]
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(name: "id")]
        public int Id { get; set; }

        [Column(name: "user_name")]
        public string? UserName { get; set; }

        [Column(name: "password")]
        public string? Password { get; set; }

        [Column(name: "state")]
        public int State { get; set; }
    }
}
