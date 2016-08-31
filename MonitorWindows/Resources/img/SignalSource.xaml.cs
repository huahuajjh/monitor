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
    /// SignalSource.xaml 的交互逻辑
    /// </summary>
    public partial class SignalSource : UserControl
    {

        public SignalSource()
        {
            InitializeComponent();
        }

        private void RadioButton_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && mousePoint.X != e.GetPosition(null).X && mousePoint.Y != e.GetPosition(null).Y)
            {
                DataObject data = new DataObject(typeof(RadioButton), sender);
                DragDrop.DoDragDrop((RadioButton)sender, data, DragDropEffects.Move);
            }
        }

        private Point mousePoint;

        private void RadioButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            mousePoint = e.GetPosition(null);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new Windows.AddGQ.AddGQWin().ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            new Windows.EditGQ.EditGQWin().ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            new Windows.EditICPC.EditICPCWin().ShowDialog();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            new Windows.AddICPC.AddICPCWin().ShowDialog();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            new Windows.AddIP.AddIPWin().ShowDialog();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            new Windows.EditIP.EditIPWin().ShowDialog();
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            new Windows.AddIPStream.AddIPStreamWin().ShowDialog();
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            new Windows.EditIPStream.EditIPStreamWin().ShowDialog();
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            new Windows.AddOther.AddOtherWin().ShowDialog();
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            new Windows.EditOther.EditOtherWin().ShowDialog();
        }
    }
}
