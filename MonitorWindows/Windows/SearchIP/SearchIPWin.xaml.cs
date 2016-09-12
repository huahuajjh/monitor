using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MonitorWindows.Windows.SearchIP
{

    public class TempItem
    {
        public int Number { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// SearchIPWin.xaml 的交互逻辑
    /// </summary>
    public partial class SearchIPWin : Window
    {
        public ObservableCollection<TempItem> Data { get; set; }

        private List<BackgroundWorker> threadPool = new List<BackgroundWorker>();

        public SearchIPWin()
        {
            InitializeComponent();
            Data = new ObservableCollection<TempItem>();
            for (int i = 0; i < 100; i++)
            {
                Data.Add(new TempItem() { Index = i, Name = i.ToString(), Number = i });
            }
            DataContext = this;
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

        /// <summary>
        /// 文本框失去焦点，下拉列表隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_LostFocus(object sender, RoutedEventArgs e)
        {
            popupContent.IsOpen = false;
        }

        /// <summary>
        /// 文字内容改变，下拉类表出现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (popupContent == null) return;
            popupContent.IsOpen = true;
        }

        private void contentItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBox1.Text = (e.AddedItems[0] as TempItem).Name;
            popupContent.IsOpen = false;
        }
    }
}
