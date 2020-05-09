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
        private bool _CsvSeparated;
        /// <summary>
        /// Csv separated
        /// </summary>
        public bool CsvSeparated
        {
            get { return _CsvSeparated; }
            set
            {
                if (_CsvSeparated != value)
                {
                    _CsvSeparated = value;
                    this.OnPropertyChanged("CsvSeparated");
                }
            }
        }

        private string _filterCodneg;
        /// <summary>
        /// 
        /// </summary>
        public string FilterCodneg
        {
            get { return _filterCodneg; }
            set
            {
                if (_filterCodneg != value)
                {
                    _filterCodneg = value;
                    this.OnPropertyChanged("FilterCodneg");
                }
            }
        }

        private bool _byCodneg;
        /// <summary>
        /// Extract only by codneg 
        /// </summary>
        public bool ByCodneg
        {
            get { return _byCodneg; }
            set
            {
                if (_byCodneg != value)
                {
                    _byCodneg = value;
                    this.OnPropertyChanged("ByCodneg");
                }
            }
        }

        private bool _InProcessing;

        public bool InProcessing
        {
            get { return _InProcessing; }
            set
            {
                if (_InProcessing != value)
                {
                    _InProcessing = value;
                    this.OnPropertyChanged("InProcessing");
                }
            }
        }

        private bool _DropTable;
        /// <summary>
        /// Clear all datas
        /// </summary>
        public bool DropTable
        {
            get { return _DropTable; }
            set
            {
                if (_DropTable != value)
                {
                    _DropTable = value;
                    this.OnPropertyChanged("DropTable");
                }
            }
        }

        private bool _IsTableSeparated;
        /// <summary>
        /// Save data by table separated
        /// </summary>
        public bool IsTableSeparated
        {
            get { return _IsTableSeparated; }
            set
            {
                if (_IsTableSeparated != value)
                {
                    _IsTableSeparated = value;
                    this.OnPropertyChanged("IsTableSeparated");
                }
            }
        }

        private string _Table = "Series";
        /// <summary>
        /// Table name
        /// </summary>
        public string Table
        {
            get { return _Table; }
            set
            {
                if (_Table != value)
                {
                    _Table = value;
                    this.OnPropertyChanged("Table");
                }
            }
        }

        private string _ConnectionString = @"Server=DESKTOP-S8UOP11\SQLEXPRESS;Database=bmf;Integrated Security=SSPI;";
        /// <summary>
        /// Connection String to connect in sql server
        /// </summary>
        public string ConnectionString
        {
            get { return _ConnectionString; }
            set
            {
                if (_ConnectionString != value)
                {
                    _ConnectionString = value;
                    this.OnPropertyChanged("ConnectionString");
                }
            }
        }

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
                    this.OnPropertyChanged("FileCsv");
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
