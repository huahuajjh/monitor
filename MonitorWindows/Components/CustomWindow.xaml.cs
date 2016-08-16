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

        private Point? point = null;
        private string _Title;

        public CustomWindow(CustomWindows windows)
        {
            this.windows = windows;
            InitializeComponent();
            this.Title = "窗口";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.windows.ReleaseWin(this);
        }

        private void Win_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            windows.ActionWin = this;
            this.point = e.GetPosition(this);
        }

        private void Win_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
            if (point == null) return;
            if (e.LeftButton == MouseButtonState.Released) return;
            windows.ActionWin = this;
            Point winPoint = e.GetPosition(windows.WinPanel);
            Point inPoint = new Point();
            inPoint.X = winPoint.X - point.Value.X;
            inPoint.Y = winPoint.Y - point.Value.Y;
            windows.CalculateWinPoint(this, inPoint);
        }

        private void Win_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            point = null;
        }

        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this._Title = value;
                this.TitleVaule.Content = value;
            }
        }
    }
}
