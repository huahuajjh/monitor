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

namespace MonitorWindows.Windows.RoundSetting
{
    public class TempItem
    {
        public int Number { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// RoundSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RoundSettingWindow : Window
    {
        public ObservableCollection<TempItem> Data { get; set; }

        public RoundSettingWindow()
        {
            InitializeComponent();
            Data = new ObservableCollection<TempItem>();
            for (int i = 0; i < 100; i++)
            {
                Data.Add(new TempItem() { Index = i, Name = i.ToString(), Number = i });
            }
            DataContext = this;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            this.dataGridDom.SelectAll();
        }

        private void NotSelectAll_Click(object sender, RoutedEventArgs e)
        {
            this.dataGridDom.UnselectAll();
        }

        private void UpData_Click(object sender, RoutedEventArgs e)
        {
            var data = this.dataGridDom.SelectedItem;
            if (data == null) return;
            int index = Data.IndexOf(data as TempItem);
            if (index < 1) return;
            foreach (var item in this.dataGridDom.SelectedItems)
            {
                int itemIndex = Data.IndexOf(item as TempItem) - 1;
                TempItem tempData = Data[itemIndex];
                Data.Remove(tempData);
                Data.Insert(itemIndex + 1, tempData);
            }
        }

        private void DownData_Click(object sender, RoutedEventArgs e)
        {
            var data = this.dataGridDom.SelectedItem;
            if (data == null) return;
            int index = Data.IndexOf(data as TempItem) + this.dataGridDom.SelectedItems.Count - 1;
            if (index >= Data.Count - 1) return;
            for (int i = this.dataGridDom.SelectedItems.Count - 1; i >= 0; i--)
            {
                TempItem item = this.dataGridDom.SelectedItems[i] as TempItem;
                int itemIndex = Data.IndexOf(item as TempItem) + 1;
                TempItem tempData = Data[itemIndex];
                Data.Remove(tempData);
                Data.Insert(itemIndex - 1, tempData);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new Windows.AddRoundSetting.AddRoundSettingWin().ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            new Windows.EditRoundSetting.EditRoundSettingWin().ShowDialog();
        }
    }
}
