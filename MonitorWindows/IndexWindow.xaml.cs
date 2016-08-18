using MonitorWindows.Windows.BgImgSetting;
using MonitorWindows.Windows.CaptionSetting;
using MonitorWindows.Windows.DeviceSettting;
using MonitorWindows.Windows.ExtOptSetting;
using MonitorWindows.Windows.InterfaceSetting;
using MonitorWindows.Windows.SysMonitor;
using MonitorWindows.Windows.UserManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MonitorWindows
{

    /// <summary>
    /// IndexWindow.xaml 的交互逻辑
    /// </summary>
    public partial class IndexWindow : Window
    {
        public IndexWindow()
        {            
            InitializeComponent();
        }

        private void Close_Win(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Minimize_Win(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Maximize_Win(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                this.MaxWidth = SystemParameters.WorkArea.Width + 13;
                this.MaxHeight = SystemParameters.WorkArea.Height + 13;
            }
            else {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                ((Button)Maximize_Btn).Style = this.FindResource("Maximize_Btn") as Style;
            }
            else
            {
                ((Button)Maximize_Btn).Style = this.FindResource("Narrow_Btn") as Style;                
            }
        }

        /*打开设备设置窗口*/
        private void OpenDeviceSettingWindow(object sender, MouseButtonEventArgs e)
        {
            new DeviceSettingWindow().ShowDialog();
        }

        private void OpenBgImgSettingWindow(object sender, RoutedEventArgs e)
        {
            BgImgSettingWindow bg_setting_win = new BgImgSettingWindow();
            bg_setting_win.Owner = this;
            bg_setting_win.ShowDialog();
        }

        private void OpenExtOptWindow(object sender, RoutedEventArgs e)
        {
            new ExtOptWindow().ShowDialog();
        }

        private void OpenSysMonitorWindow(object sender, RoutedEventArgs e)
        {
            new SysMonitorWindow().ShowDialog();
        }

        private void OpenInterfaceSettingWindow(object sender, RoutedEventArgs e)
        {
            new InterfaceSettingWindow().ShowDialog();
        }

        private void OpenCpsWindow(object sender, RoutedEventArgs e)
        {
            new CaptionSettingWindow().ShowDialog();
        }

        private void OpenUserMgtWindow(object sender, RoutedEventArgs e)
        {
            new UserManagementWindow().ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Windows.RoundSetting.RoundSettingWindow win = new Windows.RoundSetting.RoundSettingWindow();
            win.ShowDialog();
        }

        private void Share_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
	        {
                DataObject data = new DataObject(typeof(Button), sender);
                DragDrop.DoDragDrop((Button)sender, data, DragDropEffects.Move);
	        }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.CWin.FullScreen(this.CWin.ActionWin);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.CWin.PartialFullScreen(this.CWin.ActionWin);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if(this.PreviewToolState.ActualHeight > 0)
            {
                this.PreviewToolState.Height = new GridLength(0);
            }
            else
            {
                this.PreviewToolState.Height = new GridLength(200);
            }
        }

    }
}
