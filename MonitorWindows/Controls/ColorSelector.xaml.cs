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

namespace MonitorWindows.Controls
{
    public delegate void ColorChangeEventHandler(object sender, SolidColorBrush e);

    /// <summary>
    /// ColorSelector.xaml 的交互逻辑
    /// </summary>
    public partial class ColorSelector : UserControl
    {
        private static DependencyProperty selectColor = DependencyProperty.Register("SelectColor", typeof(SolidColorBrush), typeof(ColorSelector),
            new PropertyMetadata(new SolidColorBrush(new System.Windows.Media.Color() { R = 0, G = 0, B = 0, A = 255 })));

        public event ColorChangeEventHandler ColorChange;

        public ColorSelector()
        {
            InitializeComponent();
        }

        public SolidColorBrush SelectColor
        {
            get { return (SolidColorBrush)GetValue(selectColor); }
            set { SetValue(selectColor, value); }
        }

        private void SelectColor_Click(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
                if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SelectColor = new SolidColorBrush() { Color = new System.Windows.Media.Color() { R = colorDialog.Color.R, G = colorDialog.Color.G, B = colorDialog.Color.B, A = 255 } };
                }
                if (ColorChange != null) ColorChange.Invoke(this, SelectColor);
            }
        }
    }
}
