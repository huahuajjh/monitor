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
    /// PlanTrainingControl.xaml 的交互逻辑
    /// </summary>
    public partial class PlanTrainingControl : UserControl
    {
        public PlanTrainingControl()
        {
            InitializeComponent();
        }

        private Windows.RoundSetting.RoundSettingWindow roundSettingWindow;
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (roundSettingWindow == null)
            {
                roundSettingWindow = new Windows.RoundSetting.RoundSettingWindow();
                roundSettingWindow.Show();
            }
            else
            {
                roundSettingWindow.Visibility = System.Windows.Visibility.Visible;
                roundSettingWindow.Activate();
            }
        }
    }
}
