using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Policy;
using System.Text;
using System.Windows.Input;

namespace lxwpf.ViewModels
{
    public class MainWindowViewModel: BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IDialogService _dialogService;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private static string _username;
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static string Username
        {
            get { return _username; }
            set { _username = value; StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(Username))); }
        }

        public MainWindowViewModel(IRegionManager regionManager, IDialogService dialogService)
        {
            _regionManager = regionManager;
            _dialogService = dialogService;
            //OverAllContext.modbusTcpServer = new ModbusTcpServer();
            //OverAllContext.modbusTcpServer.ServerStart(502, true);

            //OverAllContext.ModbusTcpLock = new ModbusTcpNet("192.168.1.102", 502);
            //OverAllContext.ModbusTcpStatusLight = new ModbusTcpNet("192.168.1.101", 502);
            //OverAllContext.ModbusTcpDoor = new ModbusTcpNet("192.168.1.100", 502);

            Username = "当前无登录账户";

            //RefreshLight();
            //RefreshIsTemperaturing();

            ParaSettingCommand = new DelegateCommand(() =>
                _regionManager.RequestNavigate("MainRegion", "ParameterSettingView"));

            HistoryCommand = new DelegateCommand(() =>
                _regionManager.RequestNavigate("MainRegion", "HistoryRecordView"));

            AlarmCommand = new DelegateCommand(() =>
                _regionManager.RequestNavigate("MainRegion", "AlarmRecordView"));

        }

        public ICommand LoginCommand
        {
            get => new DelegateCommand(() =>
            {
                NavigationParameters keyValuePairs = new NavigationParameters();
                //keyValuePairs.Add("Menu", menuItem);
                _regionManager.Regions["MainRegion"].RequestNavigate("LoginRecordView", keyValuePairs);
                //_regionManager.Regions["MainRegion"].RequestNavigate("LoginRecordView");

            });
        }

        public ICommand ModbusCommand
        {
            get => new DelegateCommand(() =>
            {
                _regionManager.RequestNavigate("MainRegion", "ModbusView");
                //_regionManager.Regions["MainRegion"].RequestNavigate("ModbusView");
            });
        }

        /// <summary>
        /// 页面加载跳转
        /// </summary>
        public ICommand PageLoaded
        {
            get => new DelegateCommand(() =>
            {
                var region = _regionManager.Regions["MainRegion"];

                //给region添加事件，当视图跳转完成触发
                region.NavigationService.Navigated += OnNavigated;
                _regionManager.Regions["MainRegion"].RequestNavigate("LoginRecordView");

            });
        }

        public ICommand ChartCommand
        {
            get => new DelegateCommand(() =>
            {
                _regionManager.RequestNavigate("MainRegion", "ChartView");
            });
        }

        //参数配置
        public ICommand ParaSettingCommand { get; }

        //历史记录
        public ICommand HistoryCommand { get; }

        //报警
        public ICommand AlarmCommand { get; }




        private void OnNavigated(object sender, RegionNavigationEventArgs e)
        {
            // 检查导航跳转条件
            if (ShouldCancelNavigation(e.Uri))
            {
                NavigationParameters keyValuePairs = new NavigationParameters();
                _regionManager.Regions["MainRegion"].RequestNavigate("LoginRecordView", keyValuePairs);
                //_dialogService.ShowDialog("MessageView", new DialogParameters() { { "Content", "请登录账户!" } }, null);
                //e.Cancel = true; // 取消导航
                // 可以在此处添加提示逻辑
            }

        }

        private bool ShouldCancelNavigation(Uri uri)
        {
            // 账号为空并且uri不是登录界面时，取消导航
            if (OverAllContext.User == null && !uri.OriginalString.Equals("LoginRecordView"))
            {
                return true;
            }
            return false;
        }
    }
}
