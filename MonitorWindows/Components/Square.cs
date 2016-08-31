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
            else if (type == "bg")
            {
                Color color = (value as SolidColorBrush).Color;
                square.BGColor = new SolidColorBrush(new Color(){ R = color.R, G = color.G, B = color.B, A = 30});
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Square : Control
    {
        private static DependencyProperty textColorProperty = DependencyProperty.Register("TextColor", typeof(SolidColorBrush), typeof(Square),
            new PropertyMetadata());
        private static DependencyProperty linkColorProperty = DependencyProperty.Register("LinkColor", typeof(SolidColorBrush), typeof(Square),
            new PropertyMetadata());

        private static DependencyProperty bgColorProperty = DependencyProperty.Register("BGColor", typeof(SolidColorBrush), typeof(Square),
            new PropertyMetadata());

        private SolidColorBrush textColor = new SolidColorBrush(new System.Windows.Media.Color() { R = 135, G = 206, B = 235, A = 255 });
        public SolidColorBrush TextColor { get { return textColor; } set { textColor = value; this.InvalidateVisual(); } }

        private SolidColorBrush linkColor = new SolidColorBrush(new System.Windows.Media.Color() { R = 135, G = 206, B = 235, A = 255 });
        public SolidColorBrush LinkColor { get { return linkColor; } set { linkColor = value; this.InvalidateVisual(); } }

        private SolidColorBrush bgColor = new SolidColorBrush(new System.Windows.Media.Color() { R = 135, G = 206, B = 235, A = 30 });
        public SolidColorBrush BGColor { get { return bgColor; } set { bgColor = value; this.InvalidateVisual(); } }

        private const double DISTANCE = 15;

        private int _Num = 2;

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
            Binding bindingLink = new Binding();
            bindingLink.Source = ShareResources.Instance;
            bindingLink.Path = new PropertyPath("CellLinkColor");
            bindingLink.Converter = new ConverterColor(this, "link");
            bindingLink.Mode = BindingMode.OneWay;
            this.SetBinding(linkColorProperty, bindingLink);
            Binding bindingBG = new Binding();
            bindingBG.Source = ShareResources.Instance;
            bindingBG.Path = new PropertyPath("CellBGColor");
            bindingBG.Converter = new ConverterColor(this, "bg");
            bindingBG.Mode = BindingMode.OneWay;
            this.SetBinding(bgColorProperty, bindingBG);
            this._Num = splitNumber;
            this.X = x;
            this.Y = y;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            double wSize = this.ActualWidth / this.Num;
            double hSize = this.ActualHeight / this.Num;
            drawingContext.DrawRectangle(bgColor, new Pen(linkColor, 1), new Rect(new Point(0, 0), new Size(this.ActualWidth, this.ActualHeight)));
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
            var indexFormatted = new FormattedText(Index.ToString(), new System.Globalization.CultureInfo(1), System.Windows.FlowDirection.LeftToRight, new Typeface("微软雅黑"), 50, textColor);
            drawingContext.DrawText(indexFormatted, new Point(this.ActualWidth / 2 - indexFormatted.Width / 2, this.ActualHeight / 2 - indexFormatted.Height / 2));
        }

        public int Num { get { return this._Num; } set { this._Num = value; this.InvalidateVisual(); } }

        private int _ID;
        public int ID { get { return _ID; } set { _ID = value; this.InvalidateVisual(); } }
        private int _INPUT;
        public int INPUT { get { return _INPUT; } set { _INPUT = value; this.InvalidateVisual(); } }

        private int _Index;
        public int Index { get { return _Index; } set { _Index = value; this.InvalidateVisual(); } }

        public int X { get; set; }
        public int Y { get; set; }

        public Point StartPoint { get { return new Point(X * this.ActualWidth, Y * this.ActualHeight); } }
        public Point EndPoint { get { return new Point(X * ActualWidth + ActualWidth, Y * ActualHeight + ActualHeight); } }

        public Point? GetMaximum(Direction direction, Point point)
        {
            double wSize = this.ActualWidth / this.Num;
            double hSize = this.ActualHeight / this.Num;
            if (!(point.X >= StartPoint.X && point.X <= this.EndPoint.X && point.Y >= StartPoint.Y && point.Y <= EndPoint.Y)) return null;
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

        public Point? IsDargPoint(Point point)
        {
            if (point.X >= StartPoint.X && point.X <= this.EndPoint.X && point.Y >= StartPoint.Y && point.Y <= EndPoint.Y)
            {
                return StartPoint;
            }
            return null;
        }

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
    }

    public enum Direction
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}
