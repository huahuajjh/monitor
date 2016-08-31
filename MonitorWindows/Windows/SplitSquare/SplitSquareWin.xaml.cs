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

namespace MonitorWindows.Windows.SplitSquare
{
    public delegate void SplitSquareWinCallBack(int row, int col);

    /// <summary>
    /// SplitSquareWin.xaml 的交互逻辑
    /// </summary>
    public partial class SplitSquareWin : Window
    {
        SplitSquareWinCallBack callback;

        public SplitSquareWin(SplitSquareWinCallBack callback)
        {
            this.callback = callback;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int row, col;
            if (!int.TryParse(rowDom.Text, out row)) row = 1;
            if (!int.TryParse(colDom.Text, out col)) col = 1;
            if (callback != null) callback.Invoke(row, col);
            this.Close();
        }
    }
}
