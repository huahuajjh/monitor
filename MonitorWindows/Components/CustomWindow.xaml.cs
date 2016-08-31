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
        public CustomWindows windows;

        private Point? point = null;

        private string _Title;
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this._Title = value;
                this.TitleVaule.Content = value;
            }
        }

        private int zIndex = 5;
        public int ZIndex
        {
            get { return zIndex; }
            set
            {
                if (value > 10) zIndex = 10;
                else if (value < 0) zIndex = 0;
                else zIndex = value;
                Canvas.SetZIndex(this, zIndex);
            }
        }

        private CustomWindowState winState = CustomWindowState.NotFullScreen;

        

        public CustomWindowState WinState
        {
            get { return winState; }
            set
            {
                winState = value;
                switch (value)
                {
                    case CustomWindowState.FullScreen:
                        this.FullScreen();
                        Canvas.SetZIndex(this, 20);
                        maxMinWinBtn.Style = this.FindResource("Narrow_Btn") as Style;
                        break;
                    case CustomWindowState.NotFullScreen:
                        this.NotFullScreen();
                        Canvas.SetZIndex(this, ZIndex);
                        maxMinWinBtn.Style = this.FindResource("Maximize_Btn") as Style;
                        break;
                }
            }
        }

        private CustomWindowStateType winType;
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

        private double xProportion;
        private double xProportionTemp;
        private double yProportion;
        private double yProportionTemp;
        private double widthProportion;
        private double widthProportionTemp;
        private double heightProportion;
        private double heightProportionTemp;

        public CustomWindow(CustomWindows windows, double width, double height, Point point, string title, CustomWindowStateType winType)
        {
            this.windows = windows;
            InitializeComponent();
            this.Title = title;
            this.SizeChanged += CustomWindow_SizeChanged;
            this.InitProportion(width, height, point);
            this.WinType = winType;
        }

        void CustomWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.widthProportion = this.Width / this.windows.WinPanel.ActualWidth;
            this.heightProportion = this.Height / this.windows.WinPanel.ActualHeight;
            this.xProportion = Canvas.GetLeft(this) / this.windows.WinPanel.ActualWidth;
            this.yProportion = Canvas.GetTop(this) / this.windows.WinPanel.ActualHeight;
        }

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
            if (point == null || WinState == CustomWindowState.FullScreen) return;
            if (e.LeftButton == MouseButtonState.Released) return;
            windows.ActionWin = this;
            Point winPoint = e.GetPosition(windows.WinPanel);
            Point inPoint = new Point();
            inPoint.X = winPoint.X - point.Value.X;
            inPoint.Y = winPoint.Y - point.Value.Y;
            //Canvas.SetTop(this, inPoint.Y);
            //Canvas.SetLeft(this, inPoint.X);
            windows.CalculateWinPoint(this, inPoint);
        }

        private void Win_MouseUp(object sender, MouseButtonEventArgs e)
        {
            point = null;
        }

        public void InitProportion(Point point)
        {
            this.xProportion = point.X / this.windows.WinPanel.ActualWidth;
            this.yProportion = point.Y / this.windows.WinPanel.ActualHeight;
            this.InitProportion();
        }

        public void InitProportion(double width, double height, Point point)
        {
            this.xProportion = point.X / this.windows.WinPanel.ActualWidth;
            this.yProportion = point.Y / this.windows.WinPanel.ActualHeight;
            this.widthProportion = width / this.windows.WinPanel.ActualWidth;
            this.heightProportion = height / this.windows.WinPanel.ActualHeight;
            this.InitProportion();
        }

        public void InitProportion()
        {
            double x = this.xProportion * this.windows.WinPanel.ActualWidth;
            double y = this.yProportion * this.windows.WinPanel.ActualHeight;
            double width = this.widthProportion * this.windows.WinPanel.ActualWidth;
            double height = this.heightProportion * this.windows.WinPanel.ActualHeight;
            Canvas.SetLeft(this, x);
            Canvas.SetTop(this, y);
            this.Width = width;
            this.Height = height;
        }

        public void FullScreen()
        {
            winState = CustomWindowState.FullScreen;
            this.xProportionTemp = this.xProportion;
            this.yProportionTemp = this.yProportion;
            this.widthProportionTemp = this.widthProportion;
            this.heightProportionTemp = this.heightProportion;
            this.InitProportion(windows.WinPanel.ActualWidth, windows.WinPanel.ActualHeight, new Point(0, 0));
        }

        public void SingleScreen()
        {
            Point topLeftPoint = new Point(Canvas.GetLeft(this), Canvas.GetTop(this));
            Point topRightPoint = new Point(topLeftPoint.X + this.ActualWidth, topLeftPoint.Y);
            Point bottomLeftPoint = new Point(topLeftPoint.X, topLeftPoint.Y + this.ActualHeight);
            Point bottomRightPoint = new Point(topLeftPoint.X + this.ActualWidth, topLeftPoint.Y + this.ActualHeight);
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
            if (width > this.windows.WinPanel.ActualWidth) width = this.windows.WinPanel.ActualWidth;
            double height = inBottomLeftPoint.Value.Y - inTopLeftPoint.Value.Y;
            if (height > this.windows.WinPanel.ActualHeight) height = this.windows.WinPanel.ActualHeight;
            this.InitProportion(width - 2, height - 2, new Point(inTopLeftPoint.Value.X + 1, inTopLeftPoint.Value.Y + 1));
        }

        public void NotFullScreen()
        {
            winState = CustomWindowState.NotFullScreen;
            this.xProportion = this.xProportionTemp;
            this.yProportion = this.yProportionTemp;
            this.widthProportion = this.widthProportionTemp;
            this.heightProportion = this.heightProportionTemp;
            this.InitProportion();
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
                this.SingleScreen();
            }
        }

        private DispatcherTimer timer;
        private int i = 0;

        private void Win_DblClickDown(object sender, MouseButtonEventArgs e)
        {
            i += 1;
            if (timer != null) timer.IsEnabled = false;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer.Tick += (s, e1) => { timer.IsEnabled = false; i = 0; };
            timer.IsEnabled = true;
        }
    }

    public enum CustomWindowState
    {
        FullScreen,
        NotFullScreen
    }

    public enum CustomWindowStateType
    {
        HDSource,
        IPCSource,
        IPDesktopSource,
        IPStreamSource,
        OtherSource
    }

}
