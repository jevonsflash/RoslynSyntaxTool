using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
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
            Task.Factory.StartNew(() =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    ProgressWindow progressWindow = new ProgressWindow();
                    progressWindow.ShowDialog("正在初始化");

                });
                var SettingInfo = LocalDataService.ReadObjectLocal<SettingInfo>();
                if (SettingInfo == null)
                {
                    SettingInfo = new SettingInfo()
                    {
                       AvoidUsingStatic = true
                    };
                    LocalDataService.SaveObjectLocal(SettingInfo);

                }

             
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Messenger.Default.Send("", MessengerToken.CLOSEPROGRESS);

                });

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
