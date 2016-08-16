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

        private List<CustomWindow> wins = new List<CustomWindow>();

        public CustomWindow ActionWin
        {
            get { return _ActionWin; }
            set
            {
                foreach (var item in wins)
                {
                    item.Style = this.FindResource("Win_NotAction") as Style;
                }
                if(value != null)
                {
                    value.Style = this.FindResource("Win_Action") as Style;
                }
                _ActionWin = value;
            }
        }

        public CustomWindows()
        {
            InitializeComponent();
            InitDataGrid();
        }

        private void InitDataGrid()
        {
            DataGrid.Rows = 3;
            DataGrid.Columns = 3;
            for (int i = 0; i < DataGrid.Rows; i++)
            {
                for (int l = 0; l < DataGrid.Columns; l++)
                {
                    Square btn = new Square(i,l);
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
                Point? topLeft = item.GetRimDirection(Direction.TopLeft, start);
                Point? topRight = item.GetRimDirection(Direction.TopRight, new Point(end.X, start.Y));
                Point? bottomLeft = item.GetRimDirection(Direction.BottomLeft, new Point(start.X, end.Y));
                Point? bottomRight = item.GetRimDirection(Direction.BottomRight, end);
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
            Canvas.SetTop(win, y);
            Canvas.SetLeft(win, x);
        }

        private Rectangle rectangle = null;
        private Point point = new Point();

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
            rectangle.StrokeThickness = 1;
            rectangle.RadiusX = 3;
            rectangle.RadiusY = 3;
            WinPanel.Children.Add(rectangle);
            this.point = e.GetPosition(WinPanel);
            Canvas.SetLeft(rectangle, point.X);
            Canvas.SetTop(rectangle, point.Y);
        }

        private void WinPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && rectangle != null)
            {
                Point endPoint = e.GetPosition(WinPanel);
                Point startPoint = point;
                if (startPoint.X > endPoint.X && startPoint.Y > endPoint.Y)
                {
                    startPoint = endPoint;
                    endPoint = point;
                }
                else if (startPoint.X > endPoint.X && startPoint.Y < endPoint.Y)
                {
                    startPoint = new Point(endPoint.X, startPoint.Y);
                    endPoint = new Point(point.X, endPoint.Y);
                }
                else if (startPoint.X < endPoint.X && startPoint.Y > endPoint.Y)
                {
                    startPoint = new Point(startPoint.X, endPoint.Y);
                    endPoint = new Point(endPoint.X, point.Y);
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
        }

        private void WinPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WinPanel.Children.Remove(rectangle);
            rectangle = null;
            Point endPoint = e.GetPosition(WinPanel);
            Point startPoint = point;
            if (startPoint.X > endPoint.X && startPoint.Y > endPoint.Y)
            {
                startPoint = endPoint;
                endPoint = point;
            }
            else if (startPoint.X > endPoint.X && startPoint.Y < endPoint.Y)
            {
                startPoint = new Point(endPoint.X, startPoint.Y);
                endPoint = new Point(point.X, endPoint.Y);
            }
            else if (startPoint.X < endPoint.X && startPoint.Y > endPoint.Y)
            {
                startPoint = new Point(startPoint.X, endPoint.Y);
                endPoint = new Point(endPoint.X, point.Y);
            }
            if(endPoint.X - startPoint.X > 50 || endPoint.Y - startPoint.Y > 50)
            {
                var w = new CustomWindow(this);
                w.Width = endPoint.X - startPoint.X;
                w.Height = endPoint.Y - startPoint.Y;
                WinPanel.Children.Add(w);
                wins.Add(w);
                this.ActionWin = w;
                Canvas.SetLeft(w, startPoint.X);
                Canvas.SetTop(w, startPoint.Y);
            }
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
            this.wins.Remove(win);
        }
    }
}
