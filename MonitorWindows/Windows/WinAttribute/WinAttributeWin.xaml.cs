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

namespace MonitorWindows.Windows.WinAttribute
{
    public delegate void WinAttributeCallback(string title, double x, double y, double w, double h);

    /// <summary>
    /// WinAttributeWin.xaml 的交互逻辑
    /// </summary>
    public partial class WinAttributeWin : Window
    {
        private WinAttributeCallback callback;

        public WinAttributeWin(string title, double x, double y, double w, double h, WinAttributeCallback callback)
        {
            this.callback = callback;
            InitializeComponent();
            winTitleDom.Text = title;
            winXDom.Text = x.ToString();
            winYDom.Text = y.ToString();
            winWDom.Text = w.ToString();
            winHDom.Text = h.ToString();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void Close_Win(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            string title = winTitleDom.Text;
            double x = 0, y = 0, w = 0, h = 0;
            bool state = double.TryParse(winXDom.Text, out x) && double.TryParse(winYDom.Text, out y) && double.TryParse(winWDom.Text, out w) && double.TryParse(winHDom.Text, out h);
            if (state && callback != null) callback.Invoke(title, x, y, w, h);
            this.Close();
        }
    }
}
