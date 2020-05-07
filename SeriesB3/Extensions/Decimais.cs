using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace System
{
    public static class Decimais
    {
        public static string Point(this decimal value)
        {
            return value.ToString().Replace(",", ".");
        }
    }
}
