using lxwpf.Entities;
using lxwpf.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Services.ServicesImpl
{
    public class AlarmService : IAlarmService
    {
        public void Add(AlarmRecordModel record)
        {
            using var db = new AppDbContext();
            db.AlarmRecords.Add(record);
            db.SaveChanges();
        }

        public List<AlarmRecordModel> Query(DateTime start, DateTime end)
        {
            using var db = new AppDbContext();
            return db.AlarmRecords
                .Where(r => r.AlarmTime >= start && r.AlarmTime <= end)
                .OrderByDescending(r => r.AlarmTime)
                .ToList();
        }

        public void CheckAndAlarm(string deviceName, ushort address, ushort value)
        {
            using var db = new AppDbContext();
            var config = db.AlarmConfigs
                .FirstOrDefault(c => c.DeviceName == deviceName
                                  && c.Address == address
                                  && c.IsEnabled);

            if (config == null) return;

            if (value > config.UpperLimit || value < config.LowerLimit)
            {
                var record = new AlarmRecordModel
                {
                    DeviceName = deviceName,
                    Address = address,
                    Value = value,
                    Message = $"值 {value} 超出范围 [{config.LowerLimit}, {config.UpperLimit}]",
                    AlarmTime = DateTime.Now
                };
                db.AlarmRecords.Add(record);
                db.SaveChanges();
            }
        }
    }
}
