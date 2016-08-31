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

namespace MonitorWindows.Windows.OSDSetting
{
    /// <summary>
    /// OSDSettingWin.xaml 的交互逻辑
    /// </summary>
    public partial class OSDSettingWin : Window
    {
        public OSDSettingWin()
        {
            InitializeComponent();
        }

        private void Close_Win(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}
