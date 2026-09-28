using lxwpf.Repository;
using lxwpf.Services;
using lxwpf.Services.ServicesImpl;
using lxwpf.ViewModels;
using lxwpf.Views;
using lxwpf.Views.Dialogs;
using Prism.DryIoc;   // 或者 using Prism.Unity;
using Prism.Ioc;
using System.Windows;

namespace lxwpf
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        //自动创建表
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);

        //    using var db = new AppDbContext();
        //    db.Database.EnsureCreated();   // ← 启动时建表
        //}

        //程序启动，采集服务就跑
        //protected override void OnInitialized()
        //{
        //    base.OnInitialized();

        //    var collectService = Container.Resolve<IModbusCollectService>();
        //    collectService.Start();
        //}

        //程序退出后，采集服务停止
        //protected override void OnExit(ExitEventArgs e)
        //{
        //    var collectService = Container.Resolve<IModbusCollectService>();
        //    collectService.Stop();

        //    base.OnExit(e);
        //}


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IUserService, UserService>();
            containerRegistry.Register<IModbusService, ModbusService>();

            containerRegistry.Register<IDeviceConfigService, DeviceConfigService>();
            containerRegistry.Register<IHistoryService, HistoryService>();
            containerRegistry.Register<IAlarmService, AlarmService>();



            containerRegistry.RegisterForNavigation<MainWindow, MainWindowViewModel>();
            containerRegistry.RegisterForNavigation<LoginRecordView, LoginRecordViewModel>();
            containerRegistry.RegisterForNavigation<ModbusView, ModbusViewModel>();

            containerRegistry.RegisterForNavigation<ParameterSettingView, ParameterSettingViewModel>();
            containerRegistry.RegisterForNavigation<HistoryRecordView, HistoryRecordViewModel>();
            containerRegistry.RegisterForNavigation<AlarmRecordView, AlarmRecordViewModel>();


            containerRegistry.RegisterDialog<AddUserView, AddUserViewModel>();

            //采集服务 Singleton，整个应用一个实例
            //containerRegistry.RegisterSingleton<IModbusCollectService, ModbusCollectService>();

        }
    }
}