using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GeneralServiceHost.View;
using Workshop.Common;
using Workshop.Control;
using Workshop.Helper;
using Workshop.Model;
using Workshop.Service;
using Workshop.View;

namespace Workshop
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var task = InvokeHelper.InvokeOnUi("正在初始化", () =>
            {
                var SettingInfo = LocalDataService.ReadObjectLocal<SettingInfo>();
                if (SettingInfo == null)
                {
                    SettingInfo = new SettingInfo()
                    {
                       AvoidUsingStatic = true
                    };
                    LocalDataService.SaveObjectLocal(SettingInfo);

                }
            });
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void MainFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            var control = (sender as Frame).NavigationService.RemoveBackEntry();

        }
    }
}
