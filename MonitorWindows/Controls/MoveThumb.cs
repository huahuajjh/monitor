using MonitorWindows.Components;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MonitorWindows.Controls
{
    public class MoveThumb : Thumb
    {
        public MoveThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
            MouseMove += new MouseEventHandler((object sender, MouseEventArgs e) =>
            {
                if (e.LeftButton == MouseButtonState.Released) return;
                
            });
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            CustomWindow designerItem = this.DataContext as CustomWindow;

            if (designerItem != null)
            {
                double left = Canvas.GetLeft(designerItem);
                double top = Canvas.GetTop(designerItem);
                Point point = new Point();
                point.X = left + e.HorizontalChange;
                point.Y = top + e.VerticalChange;
                designerItem.windows.CalculateWinPoint(designerItem, point);
            }
        }
    }
}
