using lxwpf.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lxwpf.Services
{
    public interface IDeviceConfigService
    {
        List<DeviceConfigModel> GetAll();
        List<DeviceConfigModel> GetEnabled();
        void Add(DeviceConfigModel config);
        void Update(DeviceConfigModel config);
        void Delete(int id);
    }
}
