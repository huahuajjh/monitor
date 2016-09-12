using MonitorWindows.Components;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MonitorWindows.Controls
{
    public class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            CustomWindow designerItem = this.DataContext as CustomWindow;

            if (designerItem != null && designerItem.WinState == CustomWindowState.NotFullScreen)
            {
                double deltaVertical, deltaHorizontal;

                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange, designerItem.ActualHeight - designerItem.MinHeight);
                        designerItem.Height -= deltaVertical;
                        break;
                    case VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange, designerItem.ActualHeight - designerItem.MinHeight);
                        Canvas.SetTop(designerItem, Canvas.GetTop(designerItem) + deltaVertical);
                        designerItem.Height -= deltaVertical;
                        break;
                    default:
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
                        Canvas.SetLeft(designerItem, Canvas.GetLeft(designerItem) + deltaHorizontal);
                        designerItem.Width -= deltaHorizontal;
                        break;
                    case HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange, designerItem.ActualWidth - designerItem.MinWidth);
                        designerItem.Width -= deltaHorizontal;
                        break;
                    default:
                        break;
                }
                designerItem.WidthProportion = designerItem.Width / designerItem.windows.WinPanelWidth;
                designerItem.HeightProportion = designerItem.Height / designerItem.windows.WinPanelHeight;
                designerItem.XProportion = Canvas.GetLeft(designerItem) / designerItem.windows.WinPanelWidth;
                designerItem.YProportion = Canvas.GetTop(designerItem) / designerItem.windows.WinPanelHeight;
                designerItem.InitProportion(); 
            }
            e.Handled = true;
        }
    }
}
