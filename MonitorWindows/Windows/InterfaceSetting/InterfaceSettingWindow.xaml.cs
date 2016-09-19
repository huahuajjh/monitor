using System;
using System.Collections.Generic;
using System.IO;
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

namespace MonitorWindows.Windows.InterfaceSetting
{
    

    /// <summary>
    /// InterfaceSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InterfaceSettingWindow : Window
    {
        public InterfaceSettingWindow()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog =
                new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "选择图片|*.jpg;*.png;*.gif";
            if (dialog.ShowDialog() == true)
            {
                ShareResources.Instance.IndexBG = new ImageBrush() { ImageSource = new BitmapImage(new Uri(dialog.FileName, UriKind.Relative)) };
                checkBGOne.IsChecked = false;
                checkBGTwo.IsChecked = false;
                checkBGThree.IsChecked = false;
                checkBGFour.IsChecked = false;
            }
        }

        private void UploadLOGO_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog =
                new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "选择图片|*.jpg;*.png;*.gif";
            if (dialog.ShowDialog() == true)
            {
                ShareResources.Instance.LOGO = new ImageBrush() { ImageSource = new BitmapImage(new Uri(dialog.FileName, UriKind.Relative)) };
            }
        }

        private void ColorSelector_ColorChange(object sender, SolidColorBrush e)
        {
            ShareResources.Instance.CellTextColor = e;
        }

        private void CellLinkColor_ColorChange(object sender, SolidColorBrush e)
        {
            ShareResources.Instance.CellLinkColor = e;
        }

        private void CellBGColor_ColorChange(object sender, SolidColorBrush e)
        {
            ShareResources.Instance.CellBGColor = e;
        }

        private void HDSourceWinColor_ColorChange(object sender, SolidColorBrush e)
        {
            Color color = e.Color;
            ShareResources.Instance.HDSourceWinColor = new SolidColorBrush(new Color() { R = color.R, G = color.G, B = color.B, A = 185 });
        }

        private void IPCSourceWinColor_ColorChange(object sender, SolidColorBrush e)
        {
            Color color = e.Color;
            ShareResources.Instance.IPCSourceWinColor = new SolidColorBrush(new Color() { R = color.R, G = color.G, B = color.B, A = 185 });
        }

        private void IPDesktopSourceWinColor_ColorChange(object sender, SolidColorBrush e)
        {
            Color color = e.Color;
            ShareResources.Instance.IPDesktopSourceWinColor = new SolidColorBrush(new Color() { R = color.R, G = color.G, B = color.B, A = 185 });
        }

        private void IPStreamSourceWinColor_ColorChange(object sender, SolidColorBrush e)
        {
            Color color = e.Color;
            ShareResources.Instance.IPStreamSourceWinColor = new SolidColorBrush(new Color() { R = color.R, G = color.G, B = color.B, A = 185 });
        }

        private void OtherSourceWinColor_ColorChange(object sender, SolidColorBrush e)
        {
            Color color = e.Color;
            ShareResources.Instance.OtherSourceWinColor = new SolidColorBrush(new Color() { R = color.R, G = color.G, B = color.B, A = 185 });
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShareResources.Instance.CellTextState = ((ComboBox)sender).SelectedIndex;
        }

        private void ButtonDefault_Click(object sender, RoutedEventArgs e)
        {
            ShareResources.Instance.IndexBG = Application.Current.Resources["Index_BG"] as ImageBrush;
            checkBGOne.IsChecked = false;
            checkBGTwo.IsChecked = false;
            checkBGThree.IsChecked = false;
            checkBGFour.IsChecked = false;
        }

        private void UploadLOGODefault_Click(object sender, RoutedEventArgs e)
        {
            ShareResources.Instance.LOGO = Application.Current.Resources["LOGO"] as ImageBrush;
        }

        private void CheckBG_Checked(object sender, RoutedEventArgs e)
        {
            ShareResources.Instance.IndexBG = ((RadioButton)sender).Background as ImageBrush;
        }
    }
}
