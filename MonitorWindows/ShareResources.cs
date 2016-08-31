using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MonitorWindows
{
    public class ShareResources : DependencyObject
    {
        private static readonly DependencyProperty indexBG = DependencyProperty.Register("IndexBG", typeof(ImageBrush), typeof(ShareResources), new PropertyMetadata(Application.Current.Resources["Index_BG"] as ImageBrush));

        private static readonly DependencyProperty _LOGO = DependencyProperty.Register("LOGO", typeof(ImageBrush), typeof(ShareResources), new PropertyMetadata(Application.Current.Resources["LOGO"] as ImageBrush));

        private static readonly DependencyProperty cellTextColor = DependencyProperty.Register("CellTextColor", typeof(SolidColorBrush), typeof(ShareResources),
            new PropertyMetadata(new SolidColorBrush(new System.Windows.Media.Color() { R = 135, G = 206, B = 235, A = 255 })));

        private static readonly DependencyProperty cellLinkColor = DependencyProperty.Register("CellLinkColor", typeof(SolidColorBrush), typeof(ShareResources),
            new PropertyMetadata(new SolidColorBrush(new System.Windows.Media.Color() { R = 135, G = 206, B = 235, A = 255 })));

        private static readonly DependencyProperty cellBGColor = DependencyProperty.Register("CellBGColor", typeof(SolidColorBrush), typeof(ShareResources),
            new PropertyMetadata(new SolidColorBrush(new System.Windows.Media.Color() { R = 135, G = 206, B = 235, A = 255 })));

        private static readonly DependencyProperty hdSourceWinColor = DependencyProperty.Register("HDSourceWinColor", typeof(SolidColorBrush), typeof(ShareResources),
            new PropertyMetadata(new SolidColorBrush(new System.Windows.Media.Color() { R = 255, G = 255, B = 255, A = 185 })));

        private static readonly DependencyProperty ipCSourceWinColor = DependencyProperty.Register("IPCSourceWinColor", typeof(SolidColorBrush), typeof(ShareResources),
            new PropertyMetadata(new SolidColorBrush(new System.Windows.Media.Color() { R = 255, G = 255, B = 255, A = 185 })));

        private static readonly DependencyProperty ipDesktopSourceWinColor = DependencyProperty.Register("IPDesktopSourceWinColor", typeof(SolidColorBrush), typeof(ShareResources),
            new PropertyMetadata(new SolidColorBrush(new System.Windows.Media.Color() { R = 255, G = 255, B = 255, A = 185 })));

        private static readonly DependencyProperty ipStreamSourceWinColor = DependencyProperty.Register("IPStreamSourceWinColor", typeof(SolidColorBrush), typeof(ShareResources),
            new PropertyMetadata(new SolidColorBrush(new System.Windows.Media.Color() { R = 255, G = 255, B = 255, A = 185 })));

        private static readonly DependencyProperty otherSourceWinColor = DependencyProperty.Register("OtherSourceWinColor", typeof(SolidColorBrush), typeof(ShareResources),
            new PropertyMetadata(new SolidColorBrush(new System.Windows.Media.Color() { R = 255, G = 255, B = 255, A = 185 })));

        public ImageBrush IndexBG
        {
            get { return (ImageBrush)GetValue(indexBG); }
            set { SetValue(indexBG, value); }
        }

        public ImageBrush LOGO
        {
            get { return (ImageBrush)GetValue(_LOGO); }
            set { SetValue(_LOGO, value); }
        }

        public SolidColorBrush CellTextColor
        {
            get { return (SolidColorBrush)GetValue(cellTextColor); }
            set { SetValue(cellTextColor, value); }
        }

        public SolidColorBrush CellLinkColor
        {
            get { return (SolidColorBrush)GetValue(cellLinkColor); }
            set { SetValue(cellLinkColor, value); }
        }

        public SolidColorBrush CellBGColor
        {
            get { return (SolidColorBrush)GetValue(cellBGColor); }
            set { SetValue(cellBGColor, value); }
        }

        public SolidColorBrush HDSourceWinColor
        {
            get { return (SolidColorBrush)GetValue(hdSourceWinColor); }
            set { SetValue(hdSourceWinColor, value); }
        }

        public SolidColorBrush IPCSourceWinColor
        {
            get { return (SolidColorBrush)GetValue(ipCSourceWinColor); }
            set { SetValue(ipCSourceWinColor, value); }
        }

        public SolidColorBrush IPDesktopSourceWinColor
        {
            get { return (SolidColorBrush)GetValue(ipDesktopSourceWinColor); }
            set { SetValue(ipDesktopSourceWinColor, value); }
        }

        public SolidColorBrush IPStreamSourceWinColor
        {
            get { return (SolidColorBrush)GetValue(ipStreamSourceWinColor); }
            set { SetValue(ipStreamSourceWinColor, value); }
        }

        public SolidColorBrush OtherSourceWinColor
        {
            get { return (SolidColorBrush)GetValue(otherSourceWinColor); }
            set { SetValue(otherSourceWinColor, value); }
        }

        public static ShareResources Instance { get; private set; }

        static ShareResources()
        {
            Instance = new ShareResources();
        }
    }
}
