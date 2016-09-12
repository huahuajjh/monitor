using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// <summary>
    /// CustomWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CustomWindow : UserControl
    {

        #region 鼠标移动窗口状态变量
        private Point? point = null; 
        #endregion

        #region 双击状态变量
        private DispatcherTimer timer;
        private int i = 0;
        private bool isDBClick = false;
        #endregion

        #region 窗口标题
        private string _Title;
        /// <summary>
        /// 窗口标题
        /// </summary>
        public string Title { get { return this._Title; } set { this._Title = value; this.TitleVaule.Content = value; } } 
        #endregion

        #region 窗口顶部位置
        private int zIndex = 5;
        /// <summary>
        /// 窗口顶部位置
        /// </summary>
        public int ZIndex { get { return zIndex; } set { if (value > 10) zIndex = 10; else if (value < 0) zIndex = 0; else zIndex = value; Canvas.SetZIndex(this, zIndex); } } 
        #endregion

        #region 窗口状态
        private CustomWindowState winState = CustomWindowState.NotFullScreen;
        /// <summary>
        /// 窗口状态
        /// </summary>
        public CustomWindowState WinState
        {
            get { return winState; }
            set
            {
                winState = value;
                switch (value)
                {
                    case CustomWindowState.FullScreen:
                        maxMinWinBtn.Style = this.FindResource("Narrow_Btn") as Style;
                        break;
                    case CustomWindowState.NotFullScreen:
                        maxMinWinBtn.Style = this.FindResource("Maximize_Btn") as Style;
                        break;
                }
                this.InitProportion();
            }
        } 
        #endregion

        #region 窗口类型
        private CustomWindowStateType winType;
        /// <summary>
        /// 窗口类型
        /// </summary>
        public CustomWindowStateType WinType
        {
            get { return winType; }
            set
            {
                winType = value;
                Binding binding = new Binding();
                binding.Source = ShareResources.Instance;
                switch (winType)
                {
                    case CustomWindowStateType.HDSource:
                        binding.Path = new PropertyPath("HDSourceWinColor");
                        break;
                    case CustomWindowStateType.IPCSource:
                        binding.Path = new PropertyPath("IPCSourceWinColor");
                        break;
                    case CustomWindowStateType.IPDesktopSource:
                        binding.Path = new PropertyPath("IPDesktopSourceWinColor");
                        break;
                    case CustomWindowStateType.IPStreamSource:
                        binding.Path = new PropertyPath("IPStreamSourceWinColor");
                        break;
                    case CustomWindowStateType.OtherSource:
                        binding.Path = new PropertyPath("OtherSourceWinColor");
                        break;
                }
                this.bgDom.SetBinding(DockPanel.BackgroundProperty, binding);
            }
        } 
        #endregion

        #region 窗口位置
        private double xProportion;
        public double XProportion
        {
            get { return xProportion; }
            set { xProportion = value; }
        }
        private double xProportionDBClickTemp;
        private double yProportion;
        public double YProportion
        {
            get { return yProportion; }
            set { yProportion = value; }
        }
        private double yProportionDBClickTemp;
        private double widthProportion;
        public double WidthProportion
        {
            get { return widthProportion; }
            set { widthProportion = value; }
        }
        private double widthProportionDBClickTemp;
        private double heightProportion;
        public double HeightProportion
        {
            get { return heightProportion; }
            set { heightProportion = value; }
        }
        private double heightProportionDBClickTemp; 
        #endregion

        #region 构造函数
        public CustomWindows windows;
        public CustomWindow(CustomWindows windows, double width, double height, Point point, string title, CustomWindowStateType winType)
        {
            this.windows = windows;
            InitializeComponent();
            this.Title = title;
            this.InitProportion(width, height, point);
            this.WinType = winType;
        } 
        #endregion

        #region 触发事件
        // 关闭窗口方法
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.windows.ReleaseWin(this);
        }

        private void Win_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            windows.ActionWin = this;
            this.point = e.GetPosition(this);
        }

        private void Win_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && this.point != null && WinState == CustomWindowState.NotFullScreen)
            {
                isDBClick = false;
                Point thisPoint = e.GetPosition(this);
                if (this.point.Value.X == thisPoint.X && this.point.Value.Y == thisPoint.Y) return;
                this.windows.MoveWin(this, point.Value);
                point = null;
            }
        }

        private void Win_MouseUp(object sender, MouseButtonEventArgs e)
        {
            point = null;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            switch (this.WinState)
            {
                case CustomWindowState.FullScreen:
                    this.WinState = CustomWindowState.NotFullScreen;
                    break;
                case CustomWindowState.NotFullScreen:
                    this.WinState = CustomWindowState.FullScreen;
                    break;
            }
        }

        private void Win_DblClickUp(object sender, MouseButtonEventArgs e)
        {
            if (i > 0 && i % 2 == 0)
            {
                timer.IsEnabled = false;
                i = 0;
                if (this.WinState == CustomWindowState.FullScreen) return;
                if (!isDBClick)
                {
                    isDBClick = true;
                    this.widthProportionDBClickTemp = this.widthProportion;
                    this.heightProportionDBClickTemp = this.heightProportion;
                    this.xProportionDBClickTemp = this.xProportion;
                    this.yProportionDBClickTemp = this.yProportion;
                    this.SingleScreen();
                }
                else
                {
                    isDBClick = false;
                    this.widthProportion = this.widthProportionDBClickTemp;
                    this.heightProportion = this.heightProportionDBClickTemp;
                    this.xProportion = this.xProportionDBClickTemp;
                    this.yProportion = this.yProportionDBClickTemp;
                    this.InitProportion();
                }
            }
        }
        private void Win_DblClickDown(object sender, MouseButtonEventArgs e)
        {
            i += 1;
            if (timer != null) timer.IsEnabled = false;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer.Tick += (s, e1) => { timer.IsEnabled = false; i = 0; };
            timer.IsEnabled = true;
        }

        private void Four_Click(object sender, RoutedEventArgs e)
        {
            if (this.windows.Squares.Count <= 0) return;
            Square square = this.windows.Squares[0];
            double width = square.ActualWidth / 2;
            double height = square.ActualHeight / 2;
            this.InitProportion(width, height);
        }

        private void Nine_Click(object sender, RoutedEventArgs e)
        {
            if (this.windows.Squares.Count <= 0) return;
            Square square = this.windows.Squares[0];
            double width = square.ActualWidth / 3;
            double height = square.ActualHeight / 3;
            this.InitProportion(width, height);
        }

        private void Sixteen_Click(object sender, RoutedEventArgs e)
        {
            if (this.windows.Squares.Count <= 0) return;
            Square square = this.windows.Squares[0];
            double width = square.ActualWidth / 4;
            double height = square.ActualHeight / 4;
            this.InitProportion(width, height);
        }

        private void Max_Click(object sender, RoutedEventArgs e)
        {
            if (this.windows.Squares.Count <= 0) return;
            Square square = this.windows.Squares[0];
            double width = square.ActualWidth;
            double height = square.ActualHeight;
            this.InitProportion(width, height);
        }
        #endregion

        #region 设置窗口位置尺寸/刷新窗口位置尺寸
        public void InitProportion(Point point)
        {
            this.xProportion = point.X / this.windows.WinPanelWidth;
            this.yProportion = point.Y / this.windows.WinPanelHeight;
            this.InitProportion();
        }

        public void InitProportion(double width, double height)
        {
            this.widthProportion = width / this.windows.WinPanelWidth;
            this.heightProportion = height / this.windows.WinPanelHeight;
            this.InitProportion();
        }

        public void InitProportion(double width, double height, Point point)
        {
            this.xProportion = point.X / this.windows.WinPanelWidth;
            this.yProportion = point.Y / this.windows.WinPanelHeight;
            this.widthProportion = width / this.windows.WinPanelWidth;
            this.heightProportion = height / this.windows.WinPanelHeight;
            this.InitProportion();
        }

        public void InitProportion()
        {
            if (winState == CustomWindowState.FullScreen)
            {
                Canvas.SetLeft(this, windows.WinPanelStartPoint.X);
                Canvas.SetTop(this, windows.WinPanelStartPoint.Y);
                this.Width = windows.WinPanelWidth;
                this.Height = windows.WinPanelHeight;
                Canvas.SetZIndex(this, 100);
            }
            else if (winState == CustomWindowState.NotFullScreen)
            {
                double x = this.xProportion * this.windows.WinPanelWidth;
                double y = this.yProportion * this.windows.WinPanelHeight;
                double width = this.widthProportion * this.windows.WinPanelWidth;
                double height = this.heightProportion * this.windows.WinPanelHeight;
                Canvas.SetLeft(this, x);
                Canvas.SetTop(this, y);
                this.Width = width;
                this.Height = height;
                Canvas.SetZIndex(this, this.zIndex);
            }
        } 
        #endregion

        #region 单屏
        /// <summary>
        /// 单屏
        /// </summary>
        public void SingleScreen()
        {
            if (this.WinState == CustomWindowState.FullScreen) return;
            Point topLeftPoint = new Point(Canvas.GetLeft(this) + windows.WinPanel.Margin.Left + 1, Canvas.GetTop(this) + windows.WinPanel.Margin.Top + 1);
            Point topRightPoint = new Point(topLeftPoint.X + this.ActualWidth - 2, topLeftPoint.Y);
            Point bottomLeftPoint = new Point(topLeftPoint.X, topLeftPoint.Y + this.ActualHeight - 2);
            Point bottomRightPoint = new Point(topLeftPoint.X + this.ActualWidth - 2, topLeftPoint.Y + this.ActualHeight - 2);
            Point? inTopLeftPoint = null;
            Point? inTopRightPoint = null;
            Point? inBottomLeftPoint = null;
            Point? inBottomRightPoint = null;
            foreach (var item in this.windows.Squares)
            {
                if (inTopLeftPoint == null) inTopLeftPoint = item.GetMaximum(Direction.TopLeft, topLeftPoint);
                if (inTopRightPoint == null) inTopRightPoint = item.GetMaximum(Direction.TopRight, topRightPoint);
                if (inBottomLeftPoint == null) inBottomLeftPoint = item.GetMaximum(Direction.BottomLeft, bottomLeftPoint);
                if (inBottomRightPoint == null) inBottomRightPoint = item.GetMaximum(Direction.BottomRight, bottomRightPoint);
            }
            if (inTopLeftPoint == null || inTopRightPoint == null || inBottomLeftPoint == null || inBottomRightPoint == null) return;
            double width = inTopRightPoint.Value.X - inTopLeftPoint.Value.X;
            if (width > this.windows.WinPanelWidth) width = this.windows.WinPanelWidth;
            double height = inBottomLeftPoint.Value.Y - inTopLeftPoint.Value.Y;
            if (height > this.windows.WinPanelHeight) height = this.windows.WinPanelHeight;
            this.InitProportion(width, height, new Point(inTopLeftPoint.Value.X - windows.WinPanel.Margin.Left, inTopLeftPoint.Value.Y - windows.WinPanel.Margin.Top));
        } 
        #endregion
    }

    /// <summary>
    /// 窗口状态
    /// </summary>
    public enum CustomWindowState
    {
        /// <summary>
        /// 最大化
        /// </summary>
        FullScreen,
        /// <summary>
        /// 还原
        /// </summary>
        NotFullScreen
    }

    /// <summary>
    /// 窗口类型
    /// </summary>
    public enum CustomWindowStateType
    {
        /// <summary>
        /// 高清数据源
        /// </summary>
        HDSource,
        /// <summary>
        /// IPC信号源
        /// </summary>
        IPCSource,
        /// <summary>
        /// IP桌面信号源
        /// </summary>
        IPDesktopSource,
        /// <summary>
        /// IP流媒体信号源
        /// </summary>
        IPStreamSource,
        /// <summary>
        /// 其他信号源
        /// </summary>
        OtherSource
    }

}
