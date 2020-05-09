using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesB3.Helpers
{
    public class ScriptDb
    {
        /// <summary>
        /// Drop all tables
        /// </summary>
        public static void ClearDatabase(string connectioinSTring)
        {
            using (Connection connection = new Connection(connectioinSTring))
            {
                string sql = @"declare @sql_trigger varchar(MAX) = '', @crlf_trigger varchar(2) = char(13) + char(10);
select @sql_trigger = @sql_trigger + 'drop table dbo.' + quotename(v.name) +';' + @crlf_trigger from sys.tables v
exec(@sql_trigger);";
                connection.Query(sql);
            }
        }

        /// <summary>
        /// Create table if not exists
        /// </summary>
        /// <param name="table"></param>
        /// <param name="connection"></param>
        public static void CreateTable(string table, bool dropTable, Connection connection)
        {
            if (dropTable)
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

        public static string Sql(string table, bool dropTable)
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
            if (dropTable)
                sql = $@"insert into dbo.[{table}] (Codneg, [Data], Abertura, Maxima, Minima, Fechamento, Empresa) values (@Codneg, @Data, @Abertura, @Maxima, @Minima, @Fechamento, @Empresa)";

            return sql;
        }
    }
}
