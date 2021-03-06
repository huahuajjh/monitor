﻿using System;
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

namespace MonitorWindows.Windows.DeviceSettting
{
    /// <summary>
    /// DeviceSetting.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceSettingWindow : Window
    {
        public DeviceSettingWindow()
        {
            InitializeComponent();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.DragMove();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Label_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new Windows.SearchIP.SearchIPWin().ShowDialog();
        }

    }
}
