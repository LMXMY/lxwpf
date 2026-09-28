using lxwpf.Entities;
using lxwpf.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace lxwpf.ViewModels
{
    public class DeviceDataModel : BindableBase
    {
        private bool _isOnline;
        public bool IsOnline
        {
            get => _isOnline;
            set { _isOnline = value; RaisePropertyChanged(); }
        }

        public string? DeviceName { get; set; }
        public ObservableCollection<ModbusDataModel> Values { get; } = new();
    }
}
