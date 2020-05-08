using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace System
{
    public static class TextBoxs
    {
        public static bool SetFocus(this TextBox text)
        {
            text.Dispatcher.BeginInvoke(new Action(() =>
            {
                text.Focus();
                text.SelectAll();
            }));
            return false;
        }
    }
}
