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

namespace MonitorWindows.Controls
{
    /// <summary>
    /// IPTextBoxCtrl.xaml 的交互逻辑
    /// </summary>
    public partial class IPTextBoxCtrl : UserControl
    {
        public IPTextBoxCtrl()
        {
            InitializeComponent();
            SetUndoLimit();
            this.Text = defaultIP;
        }
        #region Field and Property
        private string backIP = string.Empty;
        private string ip = string.Empty;
        private string defaultIP = "127.0.0.1";
        public string Text
        {
            get
            {
                return string.Format("{0}.{1}.{2}.{3}",
                    tbFirstIP.Text.Length != 0 ? tbFirstIP.Text : "0",
                    tbSecondIP.Text.Length != 0 ? tbSecondIP.Text : "0",
                    tbThirdIP.Text.Length != 0 ? tbThirdIP.Text : "0",
                    tbForthIP.Text.Length != 0 ? tbForthIP.Text : "0");
            }
            set
            {
                if (IsIPString(value))
                {
                    SetIP(value);
                }
                else
                {
                    SetIP(defaultIP);
                }
            }
        }

        #endregion

        #region Base Method
        private bool CheckIntegerNumber(string strNumber, int length)
        {
            if (string.IsNullOrWhiteSpace(strNumber) || strNumber.Length > length)
                return false;
            foreach (var c in strNumber)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
        private string RemoveFirstZero(string strNumber)
        {
            if (string.IsNullOrWhiteSpace(strNumber))
                return string.Empty;
            strNumber = strNumber.Trim();
            while (true)
            {
                if (strNumber.Length > 1 && strNumber[0] == '0')
                {
                    strNumber = strNumber.Substring(1, strNumber.Length - 1);
                }
                else
                {
                    if (strNumber.Length == 1 && strNumber == "0")
                    {
                        return "0";
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return strNumber;
        }
        private bool IsIPString(string iPString)
        {
            if (string.IsNullOrWhiteSpace(iPString))
            {
                return false;
            }
            var split = iPString.Split('.');
            if (split.Length != 4)
                return false;
            foreach (var strNumber in split)
            {
                if (CheckIntegerNumber(strNumber, 3) == true)
                {
                    if (int.Parse(strNumber) > 255)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Event
        private void TextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            InputMethod.Current.ImeState = InputMethodState.Off;
            TextBox tb = sender as TextBox;
            if (tb.Text.Length != 0)
            {
                tb.SelectAll();
            }
        }
        private void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Tab)
            {
                TextBox tb = sender as TextBox;
                if (e.Key == Key.OemPeriod)
                {
                    e.Handled = true;
                    SetNextFocus(tb);
                    return;
                }
                if (e.Key < Key.D0 || e.Key > Key.D9 && e.Key < Key.NumPad0 || e.Key > Key.NumPad9)
                {
                    e.Handled = true;
                    return;
                }
                if (tb.Text.Length == 3 && tb.SelectedText.Length == 0)
                {
                    e.Handled = true;
                    return;
                }
            }
        }
        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Length == 0)
                return;
            string strNumber = RemoveFirstZero(tb.Text);
            if (CheckIntegerNumber(strNumber, 3) == true)
            {
                if (int.Parse(strNumber) <= 255)
                {
                    backIP = strNumber;
                }
            }
            tb.Text = backIP;
            SetNextFocus(tb, false);
        }
        private void TextBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.V) && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                string clipboardString = string.Empty;
                if (Clipboard.ContainsText())
                {
                    clipboardString = Clipboard.GetText();
                    if (IsIPString(clipboardString))
                    {
                        SetIP(clipboardString);
                        e.Handled = true;
                    }
                }
            }
        }
        #endregion

        #region Other Logic Method
        private void MoveNextTextBox(string tbName)
        {
            switch (tbName)
            {
                case "tbFirstIP":
                    tbSecondIP.Focus();
                    tbSecondIP.SelectAll();
                    break;
                case "tbSecondIP":
                    tbThirdIP.Focus();
                    tbThirdIP.SelectAll();
                    break;
                case "tbThirdIP":
                    tbForthIP.Focus();
                    tbForthIP.SelectAll();
                    break;
                case "tbForthIP":
                    tbFirstIP.Focus();
                    tbFirstIP.SelectAll();
                    break;
                default:
                    break;
            }
        }
        private void SetNextFocus(TextBox tb, bool IsForce = true)
        {
            if (IsForce)
            {
                MoveNextTextBox(tb.Name);
            }
            else
            {
                if (tb.Text.Length == 3 || (tb.Text.Length == 2 && int.Parse(tb.Text.Substring(0, 2)) > 25))
                {
                    MoveNextTextBox(tb.Name);
                }
            }
        }
        private void SetIP(string iPString)
        {
            if (string.IsNullOrWhiteSpace(iPString))
            {
                return;
            }
            var split = iPString.Split('.');
            if (split.Length != 4)
                return;
            tbFirstIP.Text = split[0];
            tbSecondIP.Text = split[1];
            tbThirdIP.Text = split[2];
            tbForthIP.Text = split[3];
        }
        private void SetUndoLimit()
        {
            tbFirstIP.UndoLimit = 1;
            tbSecondIP.UndoLimit = 1;
            tbThirdIP.UndoLimit = 1;
            tbForthIP.UndoLimit = 1;
        }
        #endregion
    }
}
