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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonitorWindows.Components
{
    /// <summary>
    /// CustomWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CustomWindow : UserControl
    {
        public CustomWindows windows;

        public CustomWindow(CustomWindows windows)
        {
            this.windows = windows;
            InitializeComponent();
        }

        private void Win_MouseDown(object sender, MouseButtonEventArgs e)
        {
            windows.ActionWin = this;
        }
    }
}
