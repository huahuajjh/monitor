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
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:MonitorWindows.Components"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:MonitorWindows.Components;assembly=MonitorWindows.Components"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:Square/>
    ///
    /// </summary>
    public class Square : Control
    {
        private const double SIZE = 120;
        private const double DISTANCE = 10;

        private int _Num = 2;

        private double size = 0;

        private List<Point> points = new List<Point>();

        static Square()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Square), new FrameworkPropertyMetadata(typeof(Square)));
        }

        public Square(int x,int y)
        {
            this.X = x;
            this.Y = y;
            this.Width = SIZE;
            this.Height = SIZE;
            this.StartPoint = new Point(x * SIZE, y * SIZE);
            this.EndPoint = new Point(x * SIZE + SIZE, y * SIZE + SIZE);
            this.size = SIZE / this._Num;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Gray, 1), new Rect(new Point(0, 0), new Size(SIZE, SIZE)));
            points.Clear();
            for (int i = 0; i < this._Num; i++)
            {
                for (int l = 0; l < this._Num; l++)
                {
                    points.Add(new Point(i * size, l * size));
                    Pen pen = new Pen(Brushes.Gray, 1);
                    pen.DashStyle = new DashStyle(new double[] { 1, 10 }, 0);
                    drawingContext.DrawRectangle(Brushes.Transparent, pen, new Rect(new Point(i * size, l * size), new Size(size, size)));
                }
            }
        }

        public int Num
        {
            get
            {
                return this._Num;
            }
            set
            {
                this._Num = value;
                this.size = SIZE / this._Num;
                this.UpdateLayout();
            }
        }

        public double X { get; set; }
        public double Y { get; set; }

        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }

        public Point? GetRimDirection(Direction direction, Point point)
        {
            if(point.X < this.StartPoint.X || point.X > this.EndPoint.X || point.Y < this.StartPoint.Y || point.Y > this.EndPoint.Y)
            {
                return null;
            }
            double y = (point.Y - StartPoint.Y) % size;
            double x = (point.X - StartPoint.X) % size;
            if (y <= DISTANCE && y > 0)
            {
                y *= -1;
            }
            else if (size - y <= DISTANCE && y > 0)
            {
                y = size - y;
            }
            else
            {
                y = 0;
            }
            if (x <= DISTANCE && x > 0)
            {
                x *= -1;
            }
            else if (size - x <= DISTANCE && x > 0)
            {
                x = size - x;
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
