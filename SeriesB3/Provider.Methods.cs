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

        internal void LoadCodigosNeg()
        {
            Task.Run(new Action(() =>
            {
                using (ReadFile read = new ReadFile())
                {
                    List<string> list = new List<string>();
                    foreach (string codneg in read.CodNeg(this.FileB3))
                    {
                        if (list.Contains(codneg))
                            continue;
                        list.Add(codneg);
                    }
                    this.FilterCodneg = string.Join(";", list);
                }
            }));
        }

        private List<string> Header = new List<string>() { "Codneg", "Data", "Abertura", "Maxima", "Minima", "Fechamento", "Empresa" };

        internal bool SaveCsvSapareted()
        {
            try
            {
                using (ReadFile read = new ReadFile())
                {
                    string folder = System.IO.Path.Combine(this.FileCsv, "CSVb3_" + DateTime.Now.ToString("HHmmss"));
                    if (System.IO.Directory.Exists(folder) == false)
                        System.IO.Directory.CreateDirectory(folder);

                    List<string> exists = new List<string>();

                    foreach (Infors inf in read.Series(this.FileB3))
                    {
                        System.Windows.Forms.Application.DoEvents();
                        if (!this.CheckCodneg(inf.Codneg))
                            continue;

                        string file = System.IO.Path.Combine(folder, inf.Codneg + ".csv");

                        using (StreamWriter outputFile = new StreamWriter(file, true))
                        {
                            if (exists.Contains(inf.Codneg) == false)
                            {
                                outputFile.WriteLine(string.Join(",", this.Header));
                                exists.Add(inf.Codneg);
                            }

                            List<string> data = this.GetLine(inf);
                            outputFile.WriteLine(string.Join(",", data));
                        }
                    }
                    // return true if file exists
                    return System.IO.Directory.Exists(folder);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
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
                        outputFile.WriteLine(string.Join(",", this.Header));

                        foreach (Infors inf in read.Series(this.FileB3))
                        {
                            System.Windows.Forms.Application.DoEvents();
                            if (!this.CheckCodneg(inf.Codneg))
                                continue;

                            List<string> data = this.GetLine(inf);
                            outputFile.WriteLine(string.Join(",", data));
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

        private List<string> GetLine(Infors inf)
        {
            List<string> data = new List<string>();
            data.Add(inf.Codneg);
            data.Add(inf.Data.ToString("dd/MM/yyyy"));
            data.Add(inf.Abertura.Point());
            data.Add(inf.Maxima.Point());
            data.Add(inf.Minima.Point());
            data.Add(inf.Fechamento.Point());
            data.Add(inf.Empresa);
            return data;
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
                        List<string> tables = new List<string>();
                        if (this.IsTableSeparated == false)
                            this.CreateTable(this.Table, connection);

                        string sql = Sql(this.Table);
                        foreach (Infors inf in read.Series(this.FileB3))
                        {
                            System.Windows.Forms.Application.DoEvents();
                            if (!this.CheckCodneg(inf.Codneg))
                                continue;

                            if (this.IsTableSeparated == true)
                            {
                                sql = Sql(inf.Codneg);

                                if (this.DropTable && tables.Contains(inf.Codneg) == false)
                                {
                                    this.CreateTable(inf.Codneg, connection);
                                    tables.Add(inf.Codneg);
                                }
                            }

                            using (SqlCommand cmd = new SqlCommand())
                            {
                                cmd.CommandText = sql;
                                cmd.Parameters.AddWithValue("@Codneg", inf.Codneg);
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
        /// Check if ativ to be in the filter
        /// </summary>
        /// <param name="codneg"></param>
        /// <returns></returns>
        private bool CheckCodneg(string codneg)
        {
            if (this.ByCodneg == false) return true;
            // se não tiver no filtro, então parte p/ próximo
            if (this.FilterCodneg.NullOrEmpty()) return true;
            return this.FilterCodneg.Contains(codneg);
        }

        /// <summary>
        /// Drop all tables
        /// </summary>
        internal void ClearDatabase()
        {
            using (Connection connection = new Connection(this.ConnectionString))
            {
                string sql = @"declare @sql_trigger varchar(MAX) = '', @crlf_trigger varchar(2) = char(13) + char(10);
select @sql_trigger = @sql_trigger + 'drop table dbo.' + quotename(v.name) +';' + @crlf_trigger from sys.tables v
exec(@sql_trigger);";
                connection.Query(sql);
            }
        }

        private string Sql(string table)
        {
            string sql = $@"
if not exists (select Codneg from dbo.{table} where Codneg = @Codneg and [Data] = @Data)
    insert into dbo.[{table}] (Codneg, [Data]) values (@Codneg, @Data)

update dbo.{table} set
Abertura = @Abertura,
Maxima = @Maxima,
Minima = @Minima,
Fechamento = @Fechamento,
Empresa = @Empresa
where Codneg = @Codneg and [Data] = @Data";

            // somente o insert o processo se torna bem mais rápido
            if (this.DropTable)
                sql = $@"insert into dbo.[{table}] (Codneg, [Data], Abertura, Maxima, Minima, Fechamento, Empresa) values (@Codneg, @Data, @Abertura, @Maxima, @Minima, @Fechamento, @Empresa)";

            return sql;
        }

        /// <summary>
        /// Create table if not exists
        /// </summary>
        /// <param name="table"></param>
        /// <param name="connection"></param>
        private void CreateTable(string table, Connection connection)
        {
            if (this.DropTable)
                connection.Query($"if object_id('{table}') is not null drop table dbo.[{table}]");

            string sql = $@"
if object_id('{table}') is null
begin
    create table {table} (
	    Codneg varchar(50) default('') not null,
	    [Data] datetime not null,
	    Abertura decimal(10, 2) default(0) not null,
	    Maxima decimal(10, 2) default(0) not null,
	    Minima decimal(10, 2) default(0) not null,
	    Fechamento decimal(10, 2) default(0) not null,
	    Empresa varchar(50) default('') not null,

	    primary key (Codneg, [Data])
    )
end
";
            connection.Query(sql);
        }
    }
}