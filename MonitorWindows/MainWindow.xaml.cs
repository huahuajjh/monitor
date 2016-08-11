using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MonitorWindows
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.Login == (Button)sender)
            {
                this.Login.Style = this.FindResource("Bule3DBtn") as Style;
                this.Out.Style = this.FindResource("DefaultBule3DBtn") as Style;
            }
            else
            {
                this.Login.Style = this.FindResource("DefaultBule3DBtn") as Style;
                this.Out.Style = this.FindResource("Bule3DBtn") as Style;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string ip = this.ipString.Text;
            string userName = this.UserName.Text;
            string password = this.Password.Password;
            Components.Alert.ShowAlear("Alert_Login_Hint");
        }
    }
}
