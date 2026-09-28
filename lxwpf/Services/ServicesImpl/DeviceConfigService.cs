using lxwpf.Entities;
using lxwpf.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Services.ServicesImpl
{
    public class DeviceConfigService : IDeviceConfigService
    {
        public List<DeviceConfigModel> GetAll()
        {
            using var db = new AppDbContext();
            return db.DeviceConfigs.ToList();
        }

        public List<DeviceConfigModel> GetEnabled()
        {
            using var db = new AppDbContext();
            return db.DeviceConfigs.Where(c => c.IsEnabled == true).ToList();
        }

        public void Add(DeviceConfigModel config)
        {
            using var db = new AppDbContext();
            db.DeviceConfigs.Add(config);
            db.SaveChanges();
        }

        public void Update(DeviceConfigModel config)
        {
            using var db = new AppDbContext();
            db.DeviceConfigs.Update(config);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            using var db = new AppDbContext();
            var config = db.DeviceConfigs.Find(id);
            if (config != null)
            {
                db.DeviceConfigs.Remove(config);
                db.SaveChanges();
            }
        }
    }
}
