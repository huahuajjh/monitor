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

namespace MonitorWindows.Windows.UserManagement
{
    public class TempItem
    {
        public int Number { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// UserManagementWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserManagementWindow : Window
    {
        public ObservableCollection<TempItem> Data { get; set; }

        public UserManagementWindow()
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

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            new Windows.AddUser.AddUserWin().ShowDialog();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            new Windows.EditUser.EditUserWin().ShowDialog();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            new Windows.AddPermission.AddPermissionWin().ShowDialog();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            new Windows.EditPermission.EditPermissionWin().ShowDialog();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            userManageCheckDom.IsChecked =
                yuanChengCheckDom.IsChecked =
                yuanChengDuanKouCheckDom.IsChecked =
                yuanCongQiCheckDom.IsChecked =
                yuanRiZhiCheckDom.IsChecked =
                yuanPeiZhiCheckDom.IsChecked = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            userManageCheckDom.IsChecked =
                    yuanChengCheckDom.IsChecked =
                    yuanChengDuanKouCheckDom.IsChecked =
                    yuanCongQiCheckDom.IsChecked =
                    yuanRiZhiCheckDom.IsChecked =
                    yuanPeiZhiCheckDom.IsChecked = false;
        }
    }
}
