using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeriesB3
{
    public partial class Provider
    {
        /// <summary>
        /// Valid table
        /// </summary>
        /// <returns></returns>
        internal bool ValidTable()
        {
            if (this.IsTableSeparated) return true;
            if (string.IsNullOrEmpty(this.Table))
                return MessageBox.Show("Informe a Tabela onde salvar os dados!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning) != MessageBoxResult.OK;
            return true;
        }

        /// <summary>
        /// Check filters
        /// </summary>
        /// <returns></returns>
        internal bool ValidFilterAtiv()
        {
            if (this.ByCodneg == false) return true;
            if (this.FilterCodneg.NullOrEmpty())
                return MessageBox.Show("Informe os Códigos de Negociação que deseja filtrar!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning) != MessageBoxResult.OK;
            return true;
        }

        /// <summary>
        /// Check the file path B3
        /// </summary>
        /// <returns></returns>
        internal bool ValidFileB3()
        {
            if (string.IsNullOrEmpty(this.FileB3))
                return MessageBox.Show("Informe o arquivo!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning) != MessageBoxResult.OK;
            return true;
        }

        /// <summary>
        /// Check the file path Csv
        /// </summary>
        /// <returns></returns>
        internal bool ValidFileCsv()
        {
            if (string.IsNullOrEmpty(this.FileCsv))
                return MessageBox.Show("Informe o caminho onde deseja salvar o Csv!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning) != MessageBoxResult.OK;
            return true;
        }

        /// <summary>
        /// Valid the ConnectionString
        /// </summary>
        /// <returns></returns>
        internal bool ValidConnectionString()
        {
            if (string.IsNullOrEmpty(this.ConnectionString))
                return MessageBox.Show("Informe o ConnectionString!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning) != MessageBoxResult.OK;
            return true;
        }
    }
}