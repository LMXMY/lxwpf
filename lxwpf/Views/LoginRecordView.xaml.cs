using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lxwpf.Views
{
    /// <summary>
    /// LoginRecordView.xaml 的交互逻辑
    /// </summary>
    /// 参数:IRegionManager regionManager 
    public partial class LoginRecordView : UserControl
    {
        public LoginRecordView()
        {
            InitializeComponent();

            
            //RegionManager.SetRegionManager(this, regionManager);
            //RegionManager.UpdateRegions();
        }
    }
}
