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

namespace MonitorWindows.Windows.EditScenarioMagmet
{
    public delegate void EditScenarioMagmetCallBack(string name);

    /// <summary>
    /// EditScenarioMagmetWin.xaml 的交互逻辑
    /// </summary>
    public partial class EditScenarioMagmetWin : Window
    {
        private EditScenarioMagmetCallBack callback;

        public EditScenarioMagmetWin(string name, EditScenarioMagmetCallBack callback)
        {
            InitializeComponent();
            this.callback = callback;
            this.nameDom.Text = name;
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string name = nameDom.Text;
            if (callback != null) callback.Invoke(name);
            this.Close();
        }
    }
}
