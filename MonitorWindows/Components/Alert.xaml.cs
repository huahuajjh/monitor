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

namespace MonitorWindows.Components
{
    public delegate void CallBack();

    /// <summary>
    /// Alert.xaml 的交互逻辑
    /// </summary>
    public partial class Alert : Window
    {
        private CallBack okBack;
        private CallBack noBack;

        public static void ShowAlear(string msg, CallBack okBack = null, CallBack noBack = null)
        {
            Alert alert = new Alert(msg, okBack, noBack);
            alert.ShowDialog();
        }

        private Alert(string msg, CallBack okBack, CallBack noBack)
        {
            this.okBack = okBack;
            this.noBack = noBack;
            InitializeComponent();
            this.ShowText.Text = this.FindResource(msg).ToString();
        }

        private void Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.Btn_OK == (Button)sender)
            {
                this.Btn_OK.Style = this.FindResource("Red3DBtn") as Style;
                this.Btn_NO.Style = this.FindResource("DefaultRed3DBtn") as Style;
            }
            else
            {
                this.Btn_OK.Style = this.FindResource("DefaultRed3DBtn") as Style;
                this.Btn_NO.Style = this.FindResource("Red3DBtn") as Style;
            }
        }

        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            if (this.okBack != null) this.okBack.Invoke();
            this.Close();
        }

        private void Btn_NO_Click(object sender, RoutedEventArgs e)
        {
            if (this.noBack != null) this.okBack.Invoke();
            this.Close();
        }

        private void Label_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
