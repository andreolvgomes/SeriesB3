using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesB3
{
    public partial class Provider
    {
        private string _FileCsv;
        /// <summary>
        /// File save csv
        /// </summary>
        public string FileCsv
        {
            get { return _FileCsv; }
            set
            {
                if (_FileCsv != value)
                {
                    _FileCsv = value;
                    this.OnPropertyChanged("_FileCsv");
                }
            }
        }

        private ObservableCollection<string> _ativos =  new ObservableCollection<string>();

        public ObservableCollection<string> Ativos
        {
            get { return _ativos; }
            set
            {
                if (_ativos != value)
                {
                    _ativos = value;
                    this.OnPropertyChanged("Ativos");
                }
            }
        }

        private string _delimiter = ",";
        /// <summary>
        /// Delimiter Csv
        /// </summary>
        public string Delimiter
        {
            get { return _delimiter; }
            set
            {
                if (_delimiter != value)
                {
                    _delimiter = value;
                    this.OnPropertyChanged("Delimiter");
                }
            }
        }

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
