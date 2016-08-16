using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MonitorWindows.Windows.BgImgSetting
{
    /// <summary>
    /// BgImgSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BgImgSettingWindow : Window
    {
        public BgImgSettingWindow()
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

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "图片(*.jpg)|图片(*.jpeg)|图片(*.png)|*.txt|所有文件(*.*)|*.*";
            ofd.ShowDialog();
        }
    }
}
