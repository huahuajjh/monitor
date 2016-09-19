using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonitorWindows.Components
{

    class ConverterBGColor : IValueConverter
    {
        private CustomWindows win;

        public ConverterBGColor(CustomWindows win)
        {
            this.win = win;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Color tempColor = (value as SolidColorBrush).Color;
            tempColor.A = 50;
            SolidColorBrush color = new SolidColorBrush(tempColor);
            if (win.BGColor == null && win.BGImg == null) win.WheelPanel.Background = color;
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// CustomWindows.xaml 的交互逻辑
    /// </summary>
    public partial class CustomWindows : UserControl
    {
        private double _scaleValue = 1;
        private Point openMenuPoint = new Point();

        private List<Square> _Squares = new List<Square>();
        public List<Square> Squares { get { return _Squares; } }

        private List<CustomWindow> wins = new List<CustomWindow>();

        #region 背景颜色
        private static DependencyProperty bgColorProperty = DependencyProperty.Register("_bgColor", typeof(SolidColorBrush), typeof(Square),
    new PropertyMetadata());
        private SolidColorBrush _bgColor = new SolidColorBrush(new System.Windows.Media.Color() { R = 135, G = 206, B = 235, A = 50 });
        private Color? bgColor;
        public Color? BGColor
        {
            get { return bgColor; }
            set
            {
                if(value == null)
                    bgColor = value;
                else
                {
                    bgColor = new Color() { R = value.Value.R, G = value.Value.G, B = value.Value.B, A = 50 };
                }
                if(bgColor == null)
                {
                    this.WheelPanel.Background = _bgColor;
                }
                else
                {
                    this.WheelPanel.Background = new SolidColorBrush(bgColor.Value);
                }
            }
        }
        #endregion

        #region 背景图片
        private ImageBrush bgImg;
        public ImageBrush BGImg
        {
            get { return bgImg; }
            set
            {
                bgImg = value;
                if (bgImg != null) this.WheelPanel.Background = value;
            }
        }
        #endregion

        #region 当前焦点窗口
        private CustomWindow _ActionWin = null;
        /// <summary>
        /// 当前焦点窗口
        /// </summary>
        public CustomWindow ActionWin
        {
            get { return _ActionWin; }
            set
            {
                foreach (var item in wins)
                {
                    item.Effect = null;
                }
                if (value != null)
                {
                    value.Effect = new DropShadowEffect() { ShadowDepth = 0 };
                }
                _ActionWin = value;
            }
        }
        #endregion

        #region 构造函数
        public CustomWindows()
        {
            InitializeComponent();
            this.Loaded += (object sender, RoutedEventArgs e) =>
            {
                InitDataGrid(3, 5);
            };
            this.SizeChanged += (object sender, SizeChangedEventArgs e) =>
            {
                foreach (var item in _Squares)
                {
                    item.Width = this.GetWinSize(true);
                    item.Height = this.GetWinSize(false);
                    item.UpdateLayout();
                }
                foreach (var item in wins)
                {
                    item.InitProportion();
                }
            };
            Binding bindingBG = new Binding();
            bindingBG.Source = ShareResources.Instance;
            bindingBG.Path = new PropertyPath("CellBGColor");
            bindingBG.Converter = new ConverterBGColor(this);
            bindingBG.Mode = BindingMode.OneWay;
            this.SetBinding(bgColorProperty, bindingBG);
        } 
        #endregion

        #region 设置窗口的行与列
        /// <summary>
        /// 设置窗口的行与列
        /// </summary>
        /// <param name="rows">行</param>
        /// <param name="columns">列</param>
        public void InitDataGrid(int rows, int columns)
        {
            DataGrid.Children.Clear();
            _Squares.Clear();
            DataGrid.Rows = rows;
            DataGrid.Columns = columns;
            int index = 1;
            for (int i = 0; i < DataGrid.Rows; i++)
            {
                for (int l = 0; l < DataGrid.Columns; l++)
                {
                    Square btn = new Square(i, l, int.Parse(((ComboBoxItem)splitWin.SelectedItem).DataContext.ToString()));
                    btn.Index = index++;
                    btn.Width = this.GetWinSize(true);
                    btn.Height = this.GetWinSize(false);
                    btn.UpdateLayout();
                    DataGrid.Children.Add(btn);
                    _Squares.Add(btn);
                }
            }
        } 
        #endregion

        #region 计算元素磁浮位置
        /// <summary>
        /// 计算元素磁浮位置
        /// </summary>
        /// <param name="elm">元素</param>
        /// <param name="point">元素的位置</param>
        /// <returns></returns>
        private Point GetCalculateWinPoint(FrameworkElement elm, Point point)
        {
            double x = point.X;
            double y = point.Y;
            Point start = new Point(x + WinPanel.Margin.Left, y + WinPanel.Margin.Top);
            Point end = new Point(x + elm.ActualWidth + WinPanel.Margin.Right, y + elm.ActualHeight + WinPanel.Margin.Bottom);
            // 计算出位置
            foreach (var item in this._Squares)
            {
                Point? topLeft = item.GetRimDirection(start);
                Point? topRight = item.GetRimDirection(new Point(end.X, start.Y));
                Point? bottomLeft = item.GetRimDirection(new Point(start.X, end.Y));
                Point? bottomRight = item.GetRimDirection(end);
                if (topLeft != null)
                {
                    x += topLeft.Value.X;
                    y += topLeft.Value.Y;
                    break;
                }
                else if (topRight != null)
                {
                    x += topRight.Value.X;
                    y += topRight.Value.Y;
                    break;
                }
                if (bottomLeft != null)
                {
                    x += bottomLeft.Value.X;
                    y += bottomLeft.Value.Y;
                    break;
                }
                else if (bottomRight != null)
                {
                    x += bottomRight.Value.X;
                    y += bottomRight.Value.Y;
                    break;
                }
            }
            x = x + elm.ActualWidth > this.GetWinPanelEndPoint.X ? this.GetWinPanelEndPoint.X - elm.ActualWidth : x < WinPanelStartPoint.X ? WinPanelStartPoint.X : x;
            y = y + elm.ActualHeight > this.GetWinPanelEndPoint.Y ? this.GetWinPanelEndPoint.Y - elm.ActualHeight : y < WinPanelStartPoint.Y ? WinPanelStartPoint.Y : y;
            return new Point(x, y);
        } 
        #endregion

        #region 设置指定窗口的位置
        /// <summary>
        /// 设置指定窗口的位置
        /// </summary>
        /// <param name="win">窗口</param>
        /// <param name="point">位置</param>
        public void CalculateWinPoint(CustomWindow win, Point point)
        {
            win.InitProportion(GetCalculateWinPoint(win, point));
        } 
        #endregion

        #region 移动窗口的状态变量
        private Rectangle rectangle = null;
        private Point? rectangleStartPoint = null;

        private Rectangle moveWinRectangle;
        private Point moveWinPoint;
        private CustomWindow moveWin;
        private bool isMoveWin = false; 
        #endregion

        #region 事件
        private void StackPanel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta == 0) return;
            double d = e.Delta / Math.Abs(e.Delta);
            if (_scaleValue < 0.5 && d < 0) return;
            if (_scaleValue > 10 && d > 0) return;
            _scaleValue += d * .2;
            sourceGridScaleTransform.CenterX = WheelPanel.ActualWidth / 2;
            sourceGridScaleTransform.CenterY = WheelPanel.ActualHeight / 2;
            sourceGridScaleTransform.ScaleX = _scaleValue;
            sourceGridScaleTransform.ScaleY = _scaleValue;
            e.Handled = true;
        }

        public void MoveWin(CustomWindow win, Point point)
        {
            isMoveWin = true;
            if (moveWinRectangle != null)
            {
                WinPanel.Children.Remove(moveWinRectangle);
                moveWinRectangle = null;
            }
            this.moveWin = win;
            this.moveWinPoint = point;
            moveWinRectangle = new Rectangle();
            moveWinRectangle.Width = win.ActualWidth;
            moveWinRectangle.Height = win.ActualHeight;
            Canvas.SetZIndex(moveWinRectangle, 999);
            Canvas.SetLeft(moveWinRectangle, Canvas.GetLeft(win));
            Canvas.SetTop(moveWinRectangle, Canvas.GetTop(win));
            moveWinRectangle.Stroke = Brushes.SkyBlue;
            moveWinRectangle.StrokeThickness = 1;
            WinPanel.Children.Add(moveWinRectangle);
        }

        private void WinPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMoveWin = false;
            if (rectangle != null)
            {
                WinPanel.Children.Remove(rectangle);
                rectangle = null;
            }
            rectangle = new Rectangle();
            rectangle.StrokeDashArray = new DoubleCollection(new double[] { 3 });
            rectangle.Stroke = Brushes.White;
            rectangle.StrokeThickness = 0.5;
            rectangle.RadiusX = 3;
            rectangle.RadiusY = 3;
            WinPanel.Children.Add(rectangle);
            this.rectangleStartPoint = e.GetPosition(WinPanel);
            Canvas.SetLeft(rectangle, rectangleStartPoint.Value.X);
            Canvas.SetTop(rectangle, rectangleStartPoint.Value.Y);
        }

        private void WinPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && moveWinRectangle != null && isMoveWin)
            {
                Point point = e.GetPosition(WinPanel);
                Point inPoint = new Point(point.X - moveWinPoint.X, point.Y - moveWinPoint.Y);
                inPoint = GetCalculateWinPoint(moveWinRectangle, inPoint);
                Canvas.SetLeft(moveWinRectangle, inPoint.X);
                Canvas.SetTop(moveWinRectangle, inPoint.Y);
                return;
            }
            if (e.LeftButton == MouseButtonState.Pressed && rectangle != null && rectangleStartPoint != null)
            {
                Point endPoint = e.GetPosition(WinPanel);
                Point startPoint = rectangleStartPoint.Value;
                if (startPoint.X > endPoint.X && startPoint.Y > endPoint.Y)
                {
                    startPoint = endPoint;
                    endPoint = rectangleStartPoint.Value;
                }
                else if (startPoint.X > endPoint.X && startPoint.Y < endPoint.Y)
                {
                    startPoint = new Point(endPoint.X, startPoint.Y);
                    endPoint = new Point(rectangleStartPoint.Value.X, endPoint.Y);
                }
                else if (startPoint.X < endPoint.X && startPoint.Y > endPoint.Y)
                {
                    startPoint = new Point(startPoint.X, endPoint.Y);
                    endPoint = new Point(endPoint.X, rectangleStartPoint.Value.Y);
                }
                Canvas.SetLeft(rectangle, startPoint.X);
                Canvas.SetTop(rectangle, startPoint.Y);
                Canvas.SetZIndex(rectangle, 9999);
                double x = endPoint.X - startPoint.X;
                rectangle.Width = x < 0 ? x * -1 : x;
                double y = endPoint.Y - startPoint.Y;
                rectangle.Height = y < 0 ? y * -1 : y;
                return;
            }
            if (rectangle != null)
            {
                WinPanel.Children.Remove(rectangle);
                rectangle = null;
            }
            if (rectangleStartPoint != null)
            {
                rectangleStartPoint = null;
            }
            if(moveWinRectangle != null)
            {
                WinPanel.Children.Remove(moveWinRectangle);
                rectangle = null;
            }
        }

        // 测试变量(可删除)
        private int tempState = 0;
        private void WinPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isMoveWin && moveWinRectangle != null)
            {
                moveWin.InitProportion(new Point(Canvas.GetLeft(moveWinRectangle), Canvas.GetTop(moveWinRectangle)));
                isMoveWin = false;
                WinPanel.Children.Remove(moveWinRectangle);
                moveWinRectangle = null;
                return;
            }
            if (rectangleStartPoint == null || rectangle == null) return;
            Point startPoint = rectangleStartPoint.Value;
            Point endPoint = e.GetPosition(WinPanel);
            if (startPoint.X > endPoint.X && startPoint.Y > endPoint.Y)
            {
                startPoint = endPoint;
                endPoint = rectangleStartPoint.Value;
            }
            else if (startPoint.X > endPoint.X && startPoint.Y < endPoint.Y)
            {
                startPoint = new Point(endPoint.X, startPoint.Y);
                endPoint = new Point(rectangleStartPoint.Value.X, endPoint.Y);
            }
            else if (startPoint.X < endPoint.X && startPoint.Y > endPoint.Y)
            {
                startPoint = new Point(startPoint.X, endPoint.Y);
                endPoint = new Point(endPoint.X, rectangleStartPoint.Value.Y);
            }
            if (endPoint.X - startPoint.X > 5 || endPoint.Y - startPoint.Y > 5)
            {
                var w = new CustomWindow(this, endPoint.X - startPoint.X, endPoint.Y - startPoint.Y, startPoint, "高清信号源", CustomWindowStateType.HDSource);
                switch (tempState)
                {
                    case 1:
                        w.WinType = CustomWindowStateType.IPCSource;
                        w.Title = "IPC信号源";
                        break;
                    case 2:
                        w.WinType = CustomWindowStateType.IPDesktopSource;
                        w.Title = "IP桌面信号源";
                        break;
                    case 3:
                        w.WinType = CustomWindowStateType.IPStreamSource;
                        w.Title = "IP流媒体信号源";
                        break;
                    case 4:
                        w.WinType = CustomWindowStateType.OtherSource;
                        w.Title = "其他信号源";
                        break;
                    default:
                        tempState = 0;
                        break;
                }
                WinPanel.Children.Add(w);
                wins.Add(w);
                this.ActionWin = w;
                tempState++;
            }
            // 清理数据
            WinPanel.Children.Remove(rectangle);
            rectangle = null;
            rectangleStartPoint = null;
        }

        private void WinPanel_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;
            if (dropTime.Second == DateTime.Now.Second) return;
            dropTime = DateTime.Now;
            IDataObject data = e.Data;
            if (data == null) return;
            e.Effects = DragDropEffects.None;
            Point dragPoint = e.GetPosition(this.WinPanel);
            Point? inPoint = null;
            foreach (var item in this._Squares)
            {
                inPoint = item.IsDargPoint(dragPoint);
                if (inPoint == null) continue;
                break;
            }
            if (inPoint == null) return;
            int num = int.Parse(((ComboBoxItem)splitWin.SelectedItem).DataContext.ToString());
            var w = new CustomWindow(this, this.GetWinSize(true) / num, this.GetWinSize(false) / num, new Point(inPoint.Value.X - this.WinPanel.Margin.Top, inPoint.Value.Y - this.WinPanel.Margin.Top), "窗口", CustomWindowStateType.HDSource);
            WinPanel.Children.Add(w);
            wins.Add(w);
            this.ActionWin = w;
        }

        private void PartialFullScreenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (null == this.ActionWin) return;
            this.ActionWin.SingleScreen();
        }

        private void FullScreenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (null == ActionWin) return;
            if(ActionWin.WinState == CustomWindowState.FullScreen)
            {
                ActionWin.WinState = CustomWindowState.NotFullScreen;
            }
            else
            {
                ActionWin.WinState = CustomWindowState.FullScreen;
            }
        }

        private void CloseWinBtn_Click(object sender, RoutedEventArgs e)
        {
            if (null == ActionWin) return;
            this.ReleaseWin(this.ActionWin);
        }

        private void CloseAllWinBtn_Click(object sender, RoutedEventArgs e)
        {
            while (this.wins.Count > 0)
            {
                this.ReleaseWin(this.wins.First());
            }
        }

        private void WinBottomBtn_Click(object sender, RoutedEventArgs e)
        {
            if (null == ActionWin) return;
            ActionWin.ZIndex = ActionWin.ZIndex - 1;
        }

        private void WinTopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (null == ActionWin) return;
            ActionWin.ZIndex = ActionWin.ZIndex + 1;
        }

        private void splitWin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int num = int.Parse(((ComboBoxItem)e.AddedItems[0]).DataContext.ToString());
            foreach (var item in _Squares)
            {
                item.Num = num;
            }
        }

        private void MenuItem_FourItem_Click(object sender, RoutedEventArgs e)
        {
            SortWin(2, 2);
        }

        private void MenuItem_NineItem_Click(object sender, RoutedEventArgs e)
        {
            SortWin(3, 3);
        }

        private void MenuItem_SixteenItem_Click(object sender, RoutedEventArgs e)
        {
            SortWin(4, 4);
        }

        private void MenuItem_DIYSplit_Click(object sender, RoutedEventArgs e)
        {
            new Windows.SplitSquare.SplitSquareWin((int row, int col) =>
            {
                SortWin(row, col);
            }).ShowDialog();
        }

        private void MenuItem_AutoSort_Click(object sender, RoutedEventArgs e)
        {
            Square square = null;
            foreach (var item in this.Squares)
            {
                if (item.StartPoint.X <= openMenuPoint.X && item.StartPoint.Y <= openMenuPoint.Y && item.EndPoint.X >= openMenuPoint.X && item.EndPoint.Y >= openMenuPoint.Y)
                {
                    square = item;
                    break;
                }
            }
            if (square == null) return;
            List<CustomWindow> sortWin = new List<CustomWindow>();
            foreach (var item in this.wins)
            {
                Point winStartPoint = new Point(Canvas.GetLeft(item) + WinPanel.Margin.Left, Canvas.GetTop(item) + WinPanel.Margin.Top);
                Point winEndPoint = new Point(winStartPoint.X + item.ActualWidth + WinPanel.Margin.Left, winStartPoint.Y + item.ActualHeight + WinPanel.Margin.Top);
                if (square.IsEndosphereRange(winStartPoint, winEndPoint))
                {
                    sortWin.Add(item);
                }
            }
            int row = 1;
            int col = 1;
        colIndex:
            if (row * col < sortWin.Count)
            {
                col += 1;
                row += 1;
                goto colIndex;
            }
            SortWin(row, col);
        }

        Windows.SignalAttribute.SignalAttributeWin signalAttributeWin;
        private void SignalAttribute_Click(object sender, RoutedEventArgs e)
        {
            if (signalAttributeWin == null || !signalAttributeWin.Activate())
            {
                signalAttributeWin = new Windows.SignalAttribute.SignalAttributeWin();
                signalAttributeWin.Show();
            }
        }

        Windows.WinAttribute.WinAttributeWin winAttributeWin;
        private void WinAttribute_Click(object sender, RoutedEventArgs e)
        {
            if (ActionWin == null) return;
            if (winAttributeWin == null || !winAttributeWin.Activate())
            {
                winAttributeWin = new Windows.WinAttribute.WinAttributeWin(
                ActionWin.Title,
                Canvas.GetLeft(ActionWin),
                Canvas.GetTop(ActionWin),
                ActionWin.ActualWidth,
                ActionWin.ActualHeight,
                (string title, double x, double y, double w, double h) =>
                {
                    ActionWin.Title = title;
                    ActionWin.InitProportion(w, h, new Point(x, y));
                });
                winAttributeWin.Show();
            }
        }

        Windows.OSDSetting.OSDSettingWin odsSettingWin;
        private void OSDSetting_Click(object sender, RoutedEventArgs e)
        {
            if (odsSettingWin == null || !odsSettingWin.Activate())
            {
                odsSettingWin = new Windows.OSDSetting.OSDSettingWin();
                odsSettingWin.Show();
            }
        }

        Windows.AlienSplice.AlienSpliceWin alienSpliceWin;
        private void AlienSplice_Click(object sender, RoutedEventArgs e)
        {
            if (alienSpliceWin == null || !alienSpliceWin.Activate())
            {
                alienSpliceWin = new Windows.AlienSplice.AlienSpliceWin();
                alienSpliceWin.Show();
            }
        }

        Windows.CloudControl.CloudControlWin cloudControlWin;
        private void CloudControlBtn_Click(object sender, RoutedEventArgs e)
        {
            if (cloudControlWin == null)
            {
                cloudControlWin = new Windows.CloudControl.CloudControlWin();
                cloudControlWin.Show();
                return;
            }
            cloudControlWin.Visibility = System.Windows.Visibility.Visible;
            cloudControlWin.Activate();
        }

        private void WheelPanel_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            openMenuPoint = e.GetPosition(this.WheelPanel);
        }

        Windows.SerialPortSetting.SerialPortSettingWin serialPortSettingWin;
        private void SerialPortSetting_Click(object sender, RoutedEventArgs e)
        {
            if (serialPortSettingWin == null || !serialPortSettingWin.Activate())
            {
                serialPortSettingWin = new Windows.SerialPortSetting.SerialPortSettingWin();
                serialPortSettingWin.Show();
            }
        }

        Windows.AudioControl.AudioControlWin audioControlWin;
        private void AudioControl_Click(object sender, RoutedEventArgs e)
        {
            if (audioControlWin == null)
            {
                audioControlWin = new Windows.AudioControl.AudioControlWin();
                audioControlWin.Show();
                return;
            }
            audioControlWin.Visibility = System.Windows.Visibility.Visible;
            audioControlWin.Activate();
        }
        #endregion

        #region 排序窗口
        /// <summary>
        /// 排序窗口
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        private void SortWin(int row, int col)
        {
            Square square = null;
            foreach (var item in this.Squares)
            {
                if (item.StartPoint.X <= openMenuPoint.X && item.StartPoint.Y <= openMenuPoint.Y && item.EndPoint.X >= openMenuPoint.X && item.EndPoint.Y >= openMenuPoint.Y)
                {
                    square = item;
                    break;
                }
            }
            if (square == null) return;
            List<CustomWindow> sortWin = new List<CustomWindow>();
            foreach (var item in this.wins)
            {
                Point winStartPoint = new Point(Canvas.GetLeft(item) + WinPanel.Margin.Left, Canvas.GetTop(item) + WinPanel.Margin.Top);
                Point winEndPoint = new Point(winStartPoint.X + item.ActualWidth + WinPanel.Margin.Left, winStartPoint.Y + item.ActualHeight + WinPanel.Margin.Top);
                if (square.IsEndosphereRange(winStartPoint, winEndPoint))
                {
                    sortWin.Add(item);
                }
            }
            int index = 0;
            for (int i = 0; i < row; i++)
            {
                for (int l = 0; l < col; l++)
                {
                    if (index < sortWin.Count)
                    {
                        CustomWindow win = sortWin[index++];
                        double x = square.StartPoint.X + square.ActualWidth / col * l - this.WinPanel.Margin.Left;
                        double y = square.StartPoint.Y + square.ActualHeight / row * i - this.WinPanel.Margin.Top;
                        win.InitProportion(square.ActualWidth / col, square.ActualHeight / row, new Point(x, y));
                    }
                }
            }
        } 
        #endregion

        #region 获取窗口的默认大小
        /// <summary>
        /// 获取窗口的默认大小
        /// </summary>
        /// <param name="isWidth">是否是宽度</param>
        /// <returns></returns>
        private double GetWinSize(bool isWidth)
        {
            double nw = this._SVDom.ActualWidth * 0.9;
            double nh = this._SVDom.ActualHeight * 0.9;

            //return Math.Min(nw / DataGrid.Columns, nh / DataGrid.Rows);
            if (isWidth)
            {
                return nw / DataGrid.Columns;
            }
            return nh / DataGrid.Rows;
        }
        #endregion

        #region WinPanel 尺寸
        public double WinPanelWidth
        {
            get { return this.WinPanel.ActualWidth + this.WinPanel.Margin.Left + this.WinPanel.Margin.Right; }
        }

        public double WinPanelHeight
        {
            get { return this.WinPanel.ActualHeight + this.WinPanel.Margin.Top + this.WinPanel.Margin.Bottom; }
        }

        public Point WinPanelStartPoint
        {
            get { return new Point(0 - this.WinPanel.Margin.Left, 0 - this.WinPanel.Margin.Top); }
        }

        public Point GetWinPanelEndPoint
        {
            get { return new Point(this.WinPanel.ActualWidth + this.WinPanel.Margin.Right, this.WinPanel.ActualHeight + this.WinPanel.Margin.Bottom); }
        } 
        #endregion

        #region 删除指定窗口
        /// <summary>
        /// 删除指定窗口
        /// </summary>
        /// <param name="win"></param>
        public void ReleaseWin(CustomWindow win)
        {
            foreach (CustomWindow item in wins)
            {
                if (win == item)
                {
                    this.WinPanel.Children.Remove(win);
                }
            }
            if (ActionWin == win) ActionWin = null;
            this.wins.Remove(win);
        }
        #endregion

        #region 拖拽状态变量
        private DateTime dropTime = DateTime.Now;
        #endregion

    }
}
