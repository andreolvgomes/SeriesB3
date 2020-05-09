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
                using (ReadFile read = new ReadFile())
                {
                    List<string> list = new List<string>();
                    foreach (string ativo in read.Ativos(this.FileB3))
                    {
                        if (list.Contains(ativo))
                            continue;
                        list.Add(ativo);
                    }
                    this.FilterAtivos = string.Join(";", list);
                }
            }));
        }

        private List<string> Header = new List<string>() { "Ativo", "Data", "Abertura", "Maxima", "Minima", "Fechamento", "Empresa" };

        internal bool SaveCsvSapareted()
        {
            try
            {
                using (ReadFile read = new ReadFile())
                {
                    string folder = System.IO.Path.Combine(this.FileCsv, "CSVb3_" + DateTime.Now.ToString("HHmmss"));
                    if (System.IO.Directory.Exists(folder) == false)
                        System.IO.Directory.CreateDirectory(folder);

                    List<string> ativos = new List<string>();

                    foreach (Infors inf in read.Series(this.FileB3))
                    {
                        System.Windows.Forms.Application.DoEvents();
                        if (!this.CheckFiltersAtivs(inf.Ativo))
                            continue;

                        string file = System.IO.Path.Combine(folder, inf.Ativo + ".csv");

                        using (StreamWriter outputFile = new StreamWriter(file, true))
                        {
                            if (ativos.Contains(inf.Ativo) == false)
                            {
                                outputFile.WriteLine(string.Join(",", this.Header));
                                ativos.Add(inf.Ativo);
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
                            if (!this.CheckFiltersAtivs(inf.Ativo))
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
            data.Add(inf.Ativo);
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
                            if (!this.CheckFiltersAtivs(inf.Ativo))
                                continue;

                            if (this.IsTableSeparated == true)
                            {
                                sql = Sql(inf.Ativo);

                                if (this.DropTable && tables.Contains(inf.Ativo) == false)
                                {
                                    this.CreateTable(inf.Ativo, connection);
                                    tables.Add(inf.Ativo);
                                }
                            }

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
        /// Check if ativ to be in the filter
        /// </summary>
        /// <param name="ativo"></param>
        /// <returns></returns>
        private bool CheckFiltersAtivs(string ativo)
        {
            if (this.ByAtivo == false) return true;
            // se não tiver no filtro, então parte p/ próximo
            //List<string> filters = new List<string>();

            if (this.FilterAtivos.NullOrEmpty()) return true;

            //filters = this.FilterAtivos.ToUpper().Split(';').ToList();
            //return filters.Contains(ativo);
            return this.FilterAtivos.Contains(ativo);
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
if not exists (select Ativo from dbo.{table} where Ativo = @Ativo and [Data] = @Data)
    insert into dbo.[{table}] (Ativo, [Data]) values (@Ativo, @Data)

update dbo.{table} set
Abertura = @Abertura,
Maxima = @Maxima,
Minima = @Minima,
Fechamento = @Fechamento,
Empresa = @Empresa
where Ativo = @Ativo and [Data] = @Data";

            // somente o insert o processo se torna bem mais rápido
            if (this.DropTable)
                sql = $@"insert into dbo.[{table}] (Ativo, [Data], Abertura, Maxima, Minima, Fechamento, Empresa) values (@Ativo, @Data, @Abertura, @Maxima, @Minima, @Fechamento, @Empresa)";

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
	    Ativo varchar(50) default('') not null,
	    [Data] datetime not null,
	    Abertura decimal(10, 2) default(0) not null,
	    Maxima decimal(10, 2) default(0) not null,
	    Minima decimal(10, 2) default(0) not null,
	    Fechamento decimal(10, 2) default(0) not null,
	    Empresa varchar(50) default('') not null,

	    primary key (Ativo, [Data])
    )
end
";
            connection.Query(sql);
        }
    }
}