using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesB3
{
    public class Connection : IDisposable
    {
        private SqlConnection connection = null;

        public Connection(string connectionString)
        {
            this.connection = new SqlConnection(connectionString);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (this.connection.State != System.Data.ConnectionState.Closed)
                this.connection.Close();
        }

        /// <summary>
        /// Execute query
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Query(string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql))
                return this.Query(cmd);
        }

        /// <summary>
        /// Execute query
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public int Query(SqlCommand cmd)
        {
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            cmd.Connection = connection;
            return cmd.ExecuteNonQuery();
        }
    }
}
