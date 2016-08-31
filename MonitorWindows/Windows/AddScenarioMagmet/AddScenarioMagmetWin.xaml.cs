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

namespace MonitorWindows.Windows.AddScenarioMagmet
{
    public delegate void AddScenarioMagmetCallBack(string name);

    /// <summary>
    /// AddScenarioMagmetWin.xaml 的交互逻辑
    /// </summary>
    public partial class AddScenarioMagmetWin : Window
    {

        private AddScenarioMagmetCallBack callback;

        public AddScenarioMagmetWin(AddScenarioMagmetCallBack callback)
        {
            this.callback = callback;
            InitializeComponent();
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
