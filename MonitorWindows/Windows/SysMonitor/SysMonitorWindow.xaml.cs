﻿using System;
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

namespace MonitorWindows.Windows.SysMonitor
{
    public class TempItem
    {
        public int Number { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// SysMonitorWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SysMonitorWindow : Window
    {
        public ObservableCollection<TempItem> Data { get; set; }

        public SysMonitorWindow()
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
    }
}
