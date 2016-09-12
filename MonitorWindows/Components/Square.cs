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
    /// 颜色改变监视
    /// </summary>
    public class ConverterColor : IValueConverter
    {
        private Square square;
        private string type;

        public ConverterColor(Square square, string type)
        {
            this.square = square;
            this.type = type;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (type == "text")
                square.TextColor = value as SolidColorBrush;
            else if(type == "link")
                square.LinkColor = value as SolidColorBrush;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 状态改变监视
    /// </summary>
    public class ConverterState : IValueConverter
    {
        private Square square;

        public ConverterState(Square square)
        {
            this.square = square;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            square.TextState = (int)value;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Square : Control
    {
        #region 全局监视状态
        private static DependencyProperty textColorProperty = DependencyProperty.Register("TextColor", typeof(SolidColorBrush), typeof(Square),
            new PropertyMetadata());

        private static DependencyProperty textStateProperty = DependencyProperty.Register("TextState", typeof(int), typeof(Square),
            new PropertyMetadata());

        private static DependencyProperty linkColorProperty = DependencyProperty.Register("LinkColor", typeof(SolidColorBrush), typeof(Square),
            new PropertyMetadata());

        private SolidColorBrush textColor = new SolidColorBrush(new System.Windows.Media.Color() { R = 135, G = 206, B = 235, A = 255 });
        public SolidColorBrush TextColor { get { return textColor; } set { textColor = value; this.InvalidateVisual(); } } 

        private int textState = 0;
        public int TextState { get { return textState; } set { textState = value; this.InvalidateVisual(); } }

        private SolidColorBrush linkColor = new SolidColorBrush(new System.Windows.Media.Color() { R = 135, G = 206, B = 235, A = 255 });
        public SolidColorBrush LinkColor { get { return linkColor; } set { linkColor = value; this.InvalidateVisual(); } }

        #endregion

        #region 磁浮距离
        /// <summary>
        /// 磁浮距离
        /// </summary>
        private const double DISTANCE = 15; 
        #endregion

        static Square()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Square), new FrameworkPropertyMetadata(typeof(Square)));
        }

        public Square(int x, int y, int splitNumber)
        {
            Binding binding = new Binding();
            binding.Source = ShareResources.Instance;
            binding.Path = new PropertyPath("CellTextColor");
            binding.Converter = new ConverterColor(this, "text");
            binding.Mode = BindingMode.OneWay;
            this.SetBinding(textColorProperty, binding);
            Binding bindingState = new Binding();
            bindingState.Source = ShareResources.Instance;
            bindingState.Path = new PropertyPath("CellTextState");
            bindingState.Converter = new ConverterState(this);
            bindingState.Mode = BindingMode.OneWay;
            this.SetBinding(textStateProperty, bindingState);
            Binding bindingLink = new Binding();
            bindingLink.Source = ShareResources.Instance;
            bindingLink.Path = new PropertyPath("CellLinkColor");
            bindingLink.Converter = new ConverterColor(this, "link");
            bindingLink.Mode = BindingMode.OneWay;
            this.SetBinding(linkColorProperty, bindingLink);
            this._Num = splitNumber;
            this.X = x;
            this.Y = y;
        }

        #region 事件
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            double wSize = this.ActualWidth / this.Num;
            double hSize = this.ActualHeight / this.Num;
            Brush brush = Brushes.Transparent;
            if (BGImg != null) brush = BGImg;
            else if (BGColor != null) brush = new SolidColorBrush(BGColor.Value);
            drawingContext.DrawRectangle(brush, new Pen(linkColor, 1), new Rect(new Point(0, 0), new Size(this.ActualWidth, this.ActualHeight)));
            for (int i = 0; i < this._Num; i++)
            {
                for (int l = 0; l < this._Num; l++)
                {
                    Pen pen = new Pen(linkColor, 1);
                    pen.DashStyle = new DashStyle(new double[] { 1, 10 }, 0);
                    drawingContext.DrawRectangle(Brushes.Transparent, pen, new Rect(new Point(i * wSize, l * hSize), new Size(wSize, hSize)));
                }
            }
            var idFormatted = new FormattedText("ID:" + ID, new System.Globalization.CultureInfo(1), System.Windows.FlowDirection.LeftToRight, new Typeface("微软雅黑"), 13, textColor);
            drawingContext.DrawText(idFormatted, new Point(10, this.ActualHeight - this.ActualHeight * 0.1 - 10));
            var inputFormatted = new FormattedText("INPUT:" + INPUT, new System.Globalization.CultureInfo(1), System.Windows.FlowDirection.LeftToRight, new Typeface("微软雅黑"), 13, textColor);
            drawingContext.DrawText(inputFormatted, new Point(this.Width - inputFormatted.Width - 10, this.ActualHeight - this.ActualHeight * 0.1 - 10));
            if (TextState == 0)
            {
                var indexFormatted = new FormattedText(Index.ToString(), new System.Globalization.CultureInfo(1), System.Windows.FlowDirection.LeftToRight, new Typeface("微软雅黑"), 50, textColor);
                drawingContext.DrawText(indexFormatted, new Point(this.ActualWidth / 2 - indexFormatted.Width / 2, this.ActualHeight / 2 - indexFormatted.Height / 2));
            }
            else if (TextState == 1)
            {
                var indexFormatted = new FormattedText((X + 1).ToString() + "-" + (Y + 1).ToString(), new System.Globalization.CultureInfo(1), System.Windows.FlowDirection.LeftToRight, new Typeface("微软雅黑"), 50, textColor);
                drawingContext.DrawText(indexFormatted, new Point(this.ActualWidth / 2 - indexFormatted.Width / 2, this.ActualHeight / 2 - indexFormatted.Height / 2));
            }
        } 
        #endregion

        #region 内置网格数量
        private int _Num = 2;
        /// <summary>
        /// 网格数
        /// </summary>
        public int Num { get { return this._Num; } set { this._Num = value; this.InvalidateVisual(); } } 
        #endregion

        #region ID 编号
        private int _ID;
        /// <summary>
        /// ID编号
        /// </summary>
        public int ID { get { return _ID; } set { _ID = value; this.InvalidateVisual(); } } 
        #endregion

        #region INPUT 编号
        /// <summary>
        /// INPUT 编号
        /// </summary>
        private int _INPUT;
        public int INPUT { get { return _INPUT; } set { _INPUT = value; this.InvalidateVisual(); } } 
        #endregion

        #region 背景颜色
        private Color? bgColor;
        public Color? BGColor
        {
            get { return bgColor; }
            set
            {
                if (value == null)
                    bgColor = value;
                else
                {
                    bgColor = new Color() { R = value.Value.R, G = value.Value.G, B = value.Value.B, A = 30 };
                }
                this.InvalidateVisual();
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
                this.InvalidateVisual();
            }
        }
        #endregion


        #region 所属下标
        private int _Index;
        /// <summary>
        /// 所属下标
        /// </summary>
        public int Index { get { return _Index; } set { _Index = value; this.InvalidateVisual(); } } 
        #endregion

        #region X坐标
        /// <summary>
        /// X坐标
        /// </summary>
        public int X { get; set; } 
        #endregion

        #region Y坐标
        /// <summary>
        /// Y坐标
        /// </summary>
        public int Y { get; set; } 
        #endregion

        #region 开始坐标
        /// <summary>
        /// 开始坐标
        /// </summary>
        public Point StartPoint { get { return new Point(Y * this.ActualWidth, X * this.ActualHeight); } } 
        #endregion

        #region 结束坐标
        /// <summary>
        /// 结束坐标
        /// </summary>
        public Point EndPoint { get { return new Point(Y * ActualWidth + ActualWidth, X * ActualHeight + ActualHeight); } }
        #endregion

        #region 获取单屏位置
        /// <summary>
        /// 获取单屏位置
        /// </summary>
        /// <param name="direction">方向</param>
        /// <param name="point">位置</param>
        /// <returns></returns>
        public Point? GetMaximum(Direction direction, Point point)
        {
            if (!(point.X >= StartPoint.X && point.X <= this.EndPoint.X && point.Y >= StartPoint.Y && point.Y <= EndPoint.Y)) return null;
            double wSize = this.ActualWidth / this.Num;
            double hSize = this.ActualHeight / this.Num;
            switch (direction)
            {
                case Direction.TopLeft:
                    {
                        double x = point.X - (point.X % wSize);
                        double y = point.Y - (point.Y % hSize);
                        return new Point(x, y);
                    }
                case Direction.TopRight:
                    {
                        double x = point.X - point.X % wSize + wSize;
                        double y = point.Y - point.Y % hSize;
                        return new Point(x, y);
                    }
                case Direction.BottomLeft:
                    {
                        double x = point.X - point.X % wSize;
                        double y = point.Y - point.Y % hSize + hSize;
                        return new Point(x, y);
                    }
                case Direction.BottomRight:
                    {
                        double x = point.X - point.X % wSize + wSize;
                        double y = point.Y - point.Y % hSize + hSize;
                        return new Point(x, y);
                    }
            }
            return null;
        } 
        #endregion

        #region 返回坐标的位置
        /// <summary>
        /// 返回单元格坐标的位置
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Point? IsDargPoint(Point point)
        {
            if (point.X >= StartPoint.X && point.X <= this.EndPoint.X && point.Y >= StartPoint.Y && point.Y <= EndPoint.Y)
            {
                return new Point(point.X - point.X % (this.ActualWidth / this.Num), point.Y - point.Y % (this.ActualHeight / this.Num));
            }
            return null;
        } 
        #endregion

        #region 获取磁浮坐标偏移
        /// <summary>
        /// 获取磁浮坐标偏移
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Point? GetRimDirection(Point point)
        {
            double wSize = this.ActualWidth / this.Num;
            double hSize = this.ActualHeight / this.Num;
            if (point.X < this.StartPoint.X || point.X > this.EndPoint.X || point.Y < this.StartPoint.Y || point.Y > this.EndPoint.Y)
            {
                return null;
            }
            double x = (point.X - StartPoint.X) % wSize;
            double y = (point.Y - StartPoint.Y) % hSize;
            if (y <= DISTANCE && y > 0)
            {
                y *= -1;
            }
            else if (hSize - y <= DISTANCE && y > 0)
            {
                y = hSize - y;
            }
            else
            {
                y = 0;
            }
            if (x <= DISTANCE && x > 0)
            {
                x *= -1;
            }
            else if (wSize - x <= DISTANCE && x > 0)
            {
                x = wSize - x;
            }
            else
            {
                x = 0;
            }
            if (x > 0 || y > 0)
                return new Point(x, y);
            return null;
        } 
        #endregion

        #region 坐标是否在单元格内
        /// <summary>
        /// 坐标是否在单元格内
        /// </summary>
        /// <param name="startPoint">开始坐标</param>
        /// <param name="endPoint">结束坐标</param>
        /// <returns></returns>
        public bool IsEndosphereRange(Point startPoint, Point endPoint)
        {
            Point topLeft = new Point(this.StartPoint.X + 1, this.StartPoint.Y + 1);
            Point topRight = new Point(EndPoint.X - 2, StartPoint.Y + 1);
            Point bottomLeft = new Point(EndPoint.Y + 1, StartPoint.X - 2);
            Point bottomRight = new Point(this.EndPoint.X - 2, this.EndPoint.Y - 2);
            Point winTopLeft = new Point(startPoint.X, startPoint.Y);
            Point winTopRight = new Point(endPoint.X, startPoint.Y);
            Point winBottomLeft = new Point(endPoint.Y, startPoint.X);
            Point winBottomRight = new Point(endPoint.X, endPoint.Y);
            if (startPoint.X <= topLeft.X && startPoint.Y <= topLeft.Y && endPoint.X >= topLeft.X && endPoint.Y >= topLeft.Y)
            {
                return true;
            }
            else if (startPoint.X <= topRight.X && startPoint.Y <= topRight.Y && endPoint.X >= topRight.X && endPoint.Y >= topRight.Y)
            {
                return true;
            }
            else if (startPoint.X <= bottomLeft.X && startPoint.Y <= bottomLeft.Y && endPoint.X >= bottomLeft.X && endPoint.Y >= bottomLeft.Y)
            {
                return true;
            }
            else if (startPoint.X <= bottomRight.X && startPoint.Y <= bottomRight.Y && endPoint.X >= bottomRight.X && endPoint.Y >= bottomRight.Y)
            {
                return true;
            }
            else if (topLeft.X <= winTopLeft.X && topLeft.Y <= winTopLeft.Y && bottomRight.X >= winTopLeft.X && bottomRight.Y >= winTopLeft.Y)
            {
                return true;
            }
            else if (topLeft.X <= winTopRight.X && topLeft.Y <= winTopRight.Y && bottomRight.X >= winTopRight.X && bottomRight.Y >= winTopRight.Y)
            {
                return true;
            }
            else if (topLeft.X <= winBottomLeft.X && topLeft.Y <= winBottomLeft.Y && bottomRight.X >= winBottomLeft.X && bottomRight.Y >= winBottomLeft.Y)
            {
                return true;
            }
            else if (topLeft.X <= winBottomRight.X && topLeft.Y <= winBottomRight.Y && bottomRight.X >= winBottomRight.X && bottomRight.Y >= winBottomRight.Y)
            {
                return true;
            }
            return false;
        } 
        #endregion
    }

    public enum Direction
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}
