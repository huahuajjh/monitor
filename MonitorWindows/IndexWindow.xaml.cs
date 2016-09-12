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
        private System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();

        public IndexWindow()
        {
            InitializeComponent();
            InitialTray();
            // 屏幕最大化
            this.WindowState = WindowState.Maximized;
            this.MaxWidth = SystemParameters.WorkArea.Width + 13;
            this.MaxHeight = SystemParameters.WorkArea.Height + 13;
            // 增加预览监视
            for (int i = 0; i < 10; i++)
            {
                toolWin.Add(new Components.ToolItem() { Title = i.ToString() });
            }
        }

        private void InitialTray()
        {
            Uri iconUri = new Uri("/AppIcon.ico", UriKind.Relative);
            //设置托盘的各个属性
            notifyIcon.Icon = new System.Drawing.Icon(Application.GetResourceStream(iconUri).Stream);
            notifyIcon.Visible = false;
            notifyIcon.Text = this.FindResource("MainWin_Title").ToString();
            notifyIcon.MouseClick += (object sender, System.Windows.Forms.MouseEventArgs e) => {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (this.Visibility == Visibility.Visible)
                    {
                        this.Visibility = Visibility.Hidden;
                        notifyIcon.Visible = true;
                    }
                    else
                    {
                        notifyIcon.Visible = false;
                        this.Visibility = Visibility.Visible;
                        this.Activate();
                    }
                }
            };
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem(this.FindResource("MainWin_Out").ToString());
            exit.Click += new EventHandler((object sender, EventArgs e) =>
            {
                this.Close();
            });

            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);
        }

        private void Close_Win(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Minimize_Win(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            notifyIcon.Visible = true;
        }

        private void Maximize_Win(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
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

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (this.WindowState == WindowState.Normal)
            {
                ((Button)Maximize_Btn).Style = this.FindResource("Maximize_Btn") as Style;
            }
            else if (this.WindowState == WindowState.Maximized)
            {
                ((Button)Maximize_Btn).Style = this.FindResource("Narrow_Btn") as Style;
            }
        }

        /*打开设备设置窗口*/
        DeviceSettingWindow deviceSettingWindow;
        private void OpenDeviceSettingWindow(object sender, MouseButtonEventArgs e)
        {
            if (deviceSettingWindow == null || !deviceSettingWindow.Activate())
            {
                deviceSettingWindow = new DeviceSettingWindow();
                deviceSettingWindow.Show();
                return;
            }
        }

        BgImgSettingWindow bg_setting_win;
        private void OpenBgImgSettingWindow(object sender, RoutedEventArgs e)
        {
            if (bg_setting_win == null || !bg_setting_win.Activate())
            {
                bg_setting_win = new BgImgSettingWindow();
                bg_setting_win.Show();
                return;
            }
        }

        ExtOptWindow extOptWindow;
        private void OpenExtOptWindow(object sender, RoutedEventArgs e)
        {
            if (extOptWindow == null || !extOptWindow.Activate())
            {
                extOptWindow = new ExtOptWindow();
                extOptWindow.Show();
                return;
            }
        }

        SysMonitorWindow sysMonitorWindow;
        private void OpenSysMonitorWindow(object sender, RoutedEventArgs e)
        {
            if (sysMonitorWindow == null || !sysMonitorWindow.Activate())
            {
                sysMonitorWindow = new SysMonitorWindow();
                sysMonitorWindow.Show();
            }
        }

        InterfaceSettingWindow interfaceSettingWindow;
        private void OpenInterfaceSettingWindow(object sender, RoutedEventArgs e)
        {
            if (interfaceSettingWindow == null || !interfaceSettingWindow.Activate())
            {
                interfaceSettingWindow = new InterfaceSettingWindow();
                interfaceSettingWindow.Show();
            }
        }

        CaptionSettingWindow captionSettingWindow;
        private void OpenCpsWindow(object sender, RoutedEventArgs e)
        {
            if (captionSettingWindow == null || !captionSettingWindow.Activate())
            {
                captionSettingWindow = new CaptionSettingWindow();
                captionSettingWindow.Show();
            }
        }

        UserManagementWindow userManagementWindow;
        private void OpenUserMgtWindow(object sender, RoutedEventArgs e)
        {
            if (userManagementWindow == null || !userManagementWindow.Activate())
            {
                userManagementWindow = new UserManagementWindow();
                userManagementWindow.Show();
            }
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
