using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesB3
{
    public partial class Provider
    {
        private string _fileB3;
        /// <summary>
        /// Path file b3
        /// </summary>
        public string FileB3
        {
            get { return _fileB3; }
            set
            {
                if (_fileB3 != value)
                {
                    _fileB3 = value;
                    this.OnPropertyChanged("FileB3");
                }
            }
        }

        private ToTypes _toTypes;
        /// <summary>
        /// Export to
        /// </summary>
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
    }
}
