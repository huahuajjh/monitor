using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MonitorWindows.Windows.CaptionSetting
{
    public class TempItem
    {
        public int Number { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// CaptionSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CaptionSettingWindow : Window
    {
        public ObservableCollection<TempItem> Data { get; set; }

        public CaptionSettingWindow()
        {
            InitializeComponent();
            Data = new ObservableCollection<TempItem>();
            for (int i = 0; i < 100; i++)
            {
                Data.Add(new TempItem() { Index = i, Name = i.ToString(), Number = i });
            }
            DataContext = this;
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new Windows.AddCaptionSetting.AddCaptionSettingWin().ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            new Windows.EditCaptionSetting.EditCaptionSettingWin().ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "图片(*.jpg)|图片(*.jpeg)|图片(*.png)|*.txt|所有文件(*.*)|*.*";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                imgVal.Text = ofd.FileName;
            }
        }
    }
}
