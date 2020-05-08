using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesB3
{
    public partial class Provider : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public Provider()
        {
        }

        internal void LoadAtivos()
        {
            Task.Run(new Action(() =>
            {
                ReadFile read = new ReadFile();
                foreach (string ativo in read.Ativos(this.FileB3))
                {
                    if (Ativos.Contains(ativo))
                        continue;

                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                    {
                        Ativos.Add(ativo);
                    });
                }
            }));
        }

        /// <summary>
        /// Save file csv
        /// </summary>
        /// <returns></returns>
        internal bool SaveCsv()
        {
            try
            {
                using (ReadFile read = new ReadFile())
                {
                    using (StreamWriter outputFile = new StreamWriter(this.FileCsv))
                    {
                        int counter = 0;
                        foreach (Infors inf in read.Series(this.FileB3))
                        {
                            if (counter == 0)
                                outputFile.WriteLine($"Ativo,Data,Abertura,Maxima,Minima,Fechamento");

                            outputFile.WriteLine($@"{inf.Ativo},{inf.Data.ToString("dd/MM/yyyy")},{inf.Abertura.Point()},{inf.Maxima.Point()},{inf.Minima.Point()},{inf.Fechamento.Point()}");
                            counter++;
                        }
                    }
                    // return true if file exists
                    return System.IO.File.Exists(this.FileCsv);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Save data into sql
        /// </summary>
        /// <returns></returns>
        internal bool SaveSql()
        {
            try
            {
                using (ReadFile read = new ReadFile())
                {
                    using (Connection connection = new Connection(this.ConnectionString))
                    {                        
                        this.CreateTableIfNotExist(connection);
                        this.TruncateTable(connection);
                        string sql = $"insert into dbo.[{this.Table}] (Ativo, [Data], Abertura, Maxima, Minima, Fechamento, Empresa) values (@Ativo, @Data, @Abertura, @Maxima, @Minima, @Fechamento, @Empresa)";

                        int counter = 0;
                        foreach (Infors inf in read.Series(this.FileB3))
                        {
                            counter++;

                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandText = sql;
                                cmd.Parameters.AddWithValue("@Ativo", inf.Ativo);
                                cmd.Parameters.AddWithValue("@Data", inf.Data);
                                cmd.Parameters.AddWithValue("@Abertura", inf.Abertura);
                                cmd.Parameters.AddWithValue("@Maxima", inf.Maxima);
                                cmd.Parameters.AddWithValue("@Minima", inf.Minima);
                                cmd.Parameters.AddWithValue("@Fechamento", inf.Fechamento);
                                cmd.Parameters.AddWithValue("@Empresa", inf.Empresa);

                                connection.Query(cmd);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            return false;
        }

        /// <summary>
        /// Truncate table, delete all records
        /// </summary>
        /// <param name="connection"></param>
        private void TruncateTable(Connection connection)
        {
            connection.Query($"truncate table dbo.[{this.Table}]");
        }

        /// <summary>
        /// Create table if not exists
        /// </summary>
        /// <param name="connection"></param>
        private void CreateTableIfNotExist(Connection connection)
        {
            string sql = $@"
if object_id('{this.Table}') is null
begin
	create table {this.Table} (
		Ativo varchar(50) default('') not null,	
		[Data] datetime null,
		Abertura decimal(10, 2) default(0) not null,
		Maxima decimal(10, 2) default(0) not null,
		Minima decimal(10, 2) default(0) not null,
		Fechamento decimal(10, 2) default(0) not null,
		Empresa varchar(50) default('') not null,	
	)
end";
            connection.Query(sql);
        }
    }
}