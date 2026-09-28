using lxwpf.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Services
{
    public interface IAlarmService
    {
        void Add(AlarmRecordModel record);
        List<AlarmRecordModel> Query(DateTime start, DateTime end);
        void CheckAndAlarm(string deviceName, ushort address, ushort value);
    }
}
