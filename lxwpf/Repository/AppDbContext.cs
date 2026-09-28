using lxwpf.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace lxwpf.Repository
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<DeviceConfigModel> DeviceConfigs { get; set; }
        public DbSet<HistoryModel> HistoryRecords { get; set; }
        public DbSet<AlarmConfigModel> AlarmConfigs { get; set; }
        public DbSet<AlarmRecordModel> AlarmRecords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=C:\\XM\\lxwpf\\lxwpf.db");
        }
    }
}
