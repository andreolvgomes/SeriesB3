using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SeriesB3
{
    public abstract class WindowHelp
    {
        public bool SetFocus(TextBox text)
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
