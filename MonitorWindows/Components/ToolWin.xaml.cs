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
using System.Windows.Threading;

namespace MonitorWindows.Components
{
    public class ToolItem
    {
        private double bigWidth = 190;
        private double bigHeight = 130;
        private double smallWidth = 80;
        private double smallHeight = 60;

        private string _title = "";
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (label != null) label.Content = value;
                _title = value;
            }
        }

        private bool isSmall = false;
        public bool IsSmall
        {
            get { return this.isSmall; }
            set
            {
                this.isSmall = value;
                if(this.isSmall && border != null)
                {
                    border.Width = smallWidth;
                    border.Height = smallHeight;
                }
                else if(border != null)
                {
                    border.Width = bigWidth;
                    border.Height = bigHeight;
                }
            }
        }

        Border border;
        Grid grid;
        Canvas canvas;
        Label label;

        int i;

        public Border GetBorder()
        {
            if (border == null)
            {
                border = new Border();
                grid = new Grid();
                canvas = new Canvas();
                label = new Label();
                canvas.Cursor = System.Windows.Input.Cursors.Hand;
                canvas.PreviewMouseDown += (object sender, MouseButtonEventArgs e) =>
                {
                    i += 1;
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
                    timer.Tick += (s, e1) => { timer.IsEnabled = false; i = 0; };
                    timer.IsEnabled = true;
                    if (i % 2 == 0)
                    {
                        timer.IsEnabled = false;
                        i = 0;
                        new Windows.PreviewSurveillance.PreviewSurveillanceWin().ShowDialog();
                    }
                };
                border.Margin = new Thickness(5);
                border.BorderBrush = new SolidColorBrush(new Color() { R = 28, G = 104, B = 141, A = 255 });
                border.BorderThickness = new Thickness(1);
                border.CornerRadius = new CornerRadius(3);
                if (IsSmall)
                {
                    border.Width = smallWidth;
                    border.Height = smallHeight;
                }
                else
                {
                    border.Width = bigWidth;
                    border.Height = bigHeight;
                }

                border.Child = grid;

                canvas.Background = Brushes.Transparent;
                grid.Children.Add(canvas);

                label.Content = _title;
                label.Height = 25;
                label.VerticalAlignment = VerticalAlignment.Bottom;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.Background = new SolidColorBrush(new Color() { R = 0, G = 0, B = 0, A = 76 });
                label.Foreground = Brushes.White;
                grid.Children.Add(label);
            }
            return border;
        }
    }

    /// <summary>
    /// ToolWin.xaml 的交互逻辑
    /// </summary>
    public partial class ToolWin : UserControl
    {
        public ToolWin()
        {
            this.searchVal = DependencyProperty.Register("SearchVal", typeof(string), typeof(ToolWin), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            {
                if (string.IsNullOrEmpty(e.NewValue.ToString()))
                {
                    foreach (var item in items)
                    {
                        item.GetBorder().Visibility = System.Windows.Visibility.Visible;
                    }
                }
                else
                {
                    foreach (var item in items)
                    {
                        if (item.Title.Contains(e.NewValue.ToString()))
                        {
                            item.GetBorder().Visibility = System.Windows.Visibility.Visible;
                        }
                        else
                        {
                            item.GetBorder().Visibility = System.Windows.Visibility.Collapsed;
                        }
                    }
                }
            }));
            this.showState = DependencyProperty.Register("ShowState", typeof(bool), typeof(ToolWin), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            {
                foreach (var item in items)
                {
                    item.IsSmall = (bool)e.NewValue;
                }
            }));
            InitializeComponent();
        }


        private List<ToolItem> items = new List<ToolItem>();

        private DependencyProperty searchVal = null;
        public string SearchVal
        {
            get { return (string)GetValue(searchVal); }
            set { SetValue(searchVal, value); }
        }

        public void Add(ToolItem item)
        {
            if (items.Contains(item)) return;
            item.IsSmall = ShowState;
            items.Add(item);
            toolList.Children.Add(item.GetBorder());
        }

        public void Remove(ToolItem item)
        {
            if (!this.items.Contains(item)) return;
            items.Remove(item);
            toolList.Children.Remove(item.GetBorder());
        }

        private DependencyProperty showState = null;
        public bool ShowState
        {
            get { return (bool)GetValue(showState); }
            set { SetValue(showState, value); }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            int d = e.Delta;
            if (d < 0)
            {
                ((ScrollViewer)sender).LineRight();
            }
            if (d > 0)
            {
                ((ScrollViewer)sender).LineLeft();
            }
        }
    }
}
