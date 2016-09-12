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

namespace MonitorWindows.Windows.ShowVideo
{
    /// <summary>
    /// ShowVideoWin.xaml 的交互逻辑
    /// </summary>
    public partial class ShowVideoWin : Window
    {
        public ShowVideoWin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void Maximize_Win(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                this.MaxWidth = SystemParameters.WorkArea.Width + 13;
                this.MaxHeight = SystemParameters.WorkArea.Height + 13;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
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
    }
}
