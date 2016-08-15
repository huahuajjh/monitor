using MonitorWindows.Windows.BgImgSetting;
using MonitorWindows.Windows.DeviceSettting;
using System;
using System.Collections.Generic;
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
            new DeviceSettingWindow().Show();
        }

        private void OpenBgImgSettingWindow(object sender, RoutedEventArgs e)
        {
            new BgImgSettingWindow().Show();
        }

    }
}
