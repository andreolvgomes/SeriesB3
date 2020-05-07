using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesB3
{
    public class Provider : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ToTypes _toTypes;

        public ToTypes ToType
        {
            get { return _toTypes; }
            set
            {
                if (_toTypes != value)
                {
                    _toTypes = value;
                    this.OnPropertyChanged("ToType");
                }
            }
        }

        public Provider()
        {
        }
    }
}
