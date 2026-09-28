using lxwpf.Entities;
using lxwpf.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace lxwpf.ViewModels
{
    public class ParameterSettingViewModel : BindableBase
    {
        private readonly IDeviceConfigService _configService;

        public ParameterSettingViewModel(IDeviceConfigService configService)
        {
            _configService = configService;
            LoadCommand = new DelegateCommand(Load);
            AddCommand = new DelegateCommand(Add);
            SaveCommand = new DelegateCommand(Save);
            DeleteCommand = new DelegateCommand<DeviceConfigModel>(Delete);
            PageLoaded = new DelegateCommand(Load);
        }

        public ObservableCollection<DeviceConfigModel> Configs { get; } = new();
        public ICommand LoadCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand PageLoaded { get; }

        private void Load()
        {
            Configs.Clear();
            foreach (var c in _configService.GetAll())
                Configs.Add(c);
        }

        private void Add()
        {
            Configs.Add(new DeviceConfigModel { DeviceName = "新设备", IsEnabled = true });
        }

        private void Save()
        {
            foreach (var c in Configs)
            {
                if (c.Id == 0) _configService.Add(c);
                else _configService.Update(c);
            }
        }

        private void Delete(DeviceConfigModel config)
        {
            _configService.Delete(config.Id);
            Configs.Remove(config);
        }
    }
}
