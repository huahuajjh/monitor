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
    /// <summary>
    /// 拖出窗口事件
    /// </summary>
    /// <param name="win">拖出的窗口</param>
    /// <param name="startPoint">开始的坐标</param>
    /// <param name="endPoint">结束的坐标</param>
    /// <returns>返回false:不增加改窗口到面板.返回true:于false相反</returns>
    public delegate bool DragPutWindowEventHandler(CustomWindow win, Point startPoint, Point endPoint);

    /// <summary>
    /// CustomWindows.xaml 的交互逻辑
    /// </summary>
    public partial class CustomWindows : UserControl
    {
        private double _scaleValue = 1;
        private CustomWindow _ActionWin = null;

        private List<Square> _Squares = new List<Square>();
        public List<Square> Squares { get { return _Squares; } }

        private List<CustomWindow> wins = new List<CustomWindow>();

        public CustomWindow ActionWin
        {
            get { return _ActionWin; }
            set
            {
                foreach (var item in wins)
                {
                    item.Effect = null;
                    Canvas.SetZIndex(item, item.ZIndex);
                }
                if(value != null)
                {
                    value.Effect = new DropShadowEffect() { ShadowDepth = 0 };
                }
                _ActionWin = value;
            }
        }

        public CustomWindows()
        {
            InitializeComponent();
            this.Loaded += CustomWindows_Loaded;
            this.SizeChanged += CustomWindows_SizeChanged;
        }

        void CustomWindows_SizeChanged(object sender, SizeChangedEventArgs e)
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
        }

        void CustomWindows_Loaded(object sender, RoutedEventArgs e)
        {
            InitDataGrid(3, 5);
        }

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
                    Square btn = new Square(l, i, int.Parse(((ComboBoxItem)splitWin.SelectedItem).DataContext.ToString()));
                    btn.Index = index++;
                    btn.Width = this.GetWinSize(true);
                    btn.Height = this.GetWinSize(false);
                    btn.UpdateLayout();
                    DataGrid.Children.Add(btn);
                    _Squares.Add(btn);
                }
            }
        }

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

        public void CalculateWinPoint(CustomWindow win, Point point)
        {
            double x = point.X;
            double y = point.Y;
            Point start = new Point(x, y);
            Point end = new Point(x + win.ActualWidth, y + win.ActualHeight);
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
            x = x + win.ActualWidth > this.WinPanel.ActualWidth ? this.WinPanel.ActualWidth - win.ActualWidth : x < 0 ? 0 : x;
            y = y + win.ActualHeight > this.WinPanel.ActualHeight ? this.WinPanel.ActualHeight - win.ActualHeight : y < 0 ? 0 : y;
            win.InitProportion(new Point(x, y));
        }

        private Rectangle rectangle = null;
        private Point? rectangleStartPoint = null;

        public DragPutWindowEventHandler OnDragPutWindow { get; set; }

        private void WinPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (rectangle != null)
            {
                WinPanel.Children.Remove(rectangle);
                rectangle = null;
            }
            rectangle = new Rectangle();
            rectangle.StrokeDashArray = new DoubleCollection(new double[] {3});
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
            }
            else if (rectangle != null)
            {
                WinPanel.Children.Remove(rectangle);
                rectangle = null;
            }
            else if (rectangleStartPoint != null)
            {
                rectangleStartPoint = null;
            }
        }

        // 测试变量(可删除)
        private int tempState = 0;
        private void WinPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
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
            if(endPoint.X - startPoint.X > 50 || endPoint.Y - startPoint.Y > 50)
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

        public void ReleaseWin(CustomWindow win)
        {
            foreach (CustomWindow item in wins)
            {
                if(win == item)
                {
                    this.WinPanel.Children.Remove(win);
                }
            }
            if (ActionWin == win) ActionWin = null;
            this.wins.Remove(win);
        }

        private DateTime dropTime = DateTime.Now;

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
            var w = new CustomWindow(this, this.GetWinSize(true), this.GetWinSize(false), inPoint.Value, "窗口", CustomWindowStateType.HDSource);
            WinPanel.Children.Add(w);
            wins.Add(w);
            this.ActionWin = w;
        }

        public double GetWinSize(bool isWidth)
        {
            double nw = this._SVDom.ActualWidth * 0.9;
            double nh = this._SVDom.ActualHeight * 0.9;

            //return Math.Min(nw / DataGrid.Columns, nh / DataGrid.Rows);
            if(isWidth)
            {
                return nw / DataGrid.Columns;
            }
            return nh / DataGrid.Rows;
        }

        private void PartialFullScreenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (null == this.ActionWin) return;
            this.ActionWin.SingleScreen();
        }

        private void FullScreenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (null == ActionWin) return;
            ActionWin.WinState = CustomWindowState.FullScreen;
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
            this.InitDataGrid(2,2);
        }

        private void MenuItem_NineItem_Click(object sender, RoutedEventArgs e)
        {
            this.InitDataGrid(3, 3);
        }

        private void MenuItem_SixteenItem_Click(object sender, RoutedEventArgs e)
        {
            this.InitDataGrid(4, 4);
        }

        private void MenuItem_DIYSplit_Click(object sender, RoutedEventArgs e)
        {
            new Windows.SplitSquare.SplitSquareWin((int row, int col) => {
                this.InitDataGrid(row, col);
            }).ShowDialog();
        }

        private void MenuItem_AutoSort_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < wins.Count; i++)
            {
                if (_Squares.Count <= i) break;
                CustomWindow win = wins[i];
                Square square = _Squares[i];
                win.InitProportion(square.ActualWidth - 2, square.ActualHeight - 2, new Point(square.StartPoint.X + 1, square.StartPoint.Y + 1));
            }
            for (int i = _Squares.Count; i < wins.Count; i++)
            {
                CustomWindow win = wins[i];
                ReleaseWin(win);
            }
        }

        private void SignalAttribute_Click(object sender, RoutedEventArgs e)
        {
            if (null == ActionWin) return;
            new Windows.SignalAttribute.SignalAttributeWin().ShowDialog();
        }

        private void WinAttribute_Click(object sender, RoutedEventArgs e)
        {
            if (null == ActionWin) return;
            new Windows.WinAttribute.WinAttributeWin(
                ActionWin.Title,
                Canvas.GetLeft(ActionWin),
                Canvas.GetTop(ActionWin),
                ActionWin.ActualWidth,
                ActionWin.ActualHeight,
                (string title, double x, double y, double w, double h) => {
                    ActionWin.Title = title;
                    ActionWin.InitProportion(w, h, new Point(x, y));
                }).ShowDialog();
        }

        private void OSDSetting_Click(object sender, RoutedEventArgs e)
        {
            if (null == ActionWin) return;
            new Windows.OSDSetting.OSDSettingWin().ShowDialog();
        }

        private void AlienSplice_Click(object sender, RoutedEventArgs e)
        {
            if (null == ActionWin) return;
            new Windows.AlienSplice.AlienSpliceWin().ShowDialog();
        }

        private void CloudControlBtn_Click(object sender, RoutedEventArgs e)
        {
            new Windows.CloudControl.CloudControlWin().ShowDialog();
        }
    }
}
