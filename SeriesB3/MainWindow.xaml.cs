using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeriesB3
{
    ///https://www.filehelpers.net/download/
    ///
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Provider provider = null;

        public MainWindow()
        {
            InitializeComponent();
            this.provider = new Provider();
            this.DataContext = provider;
        }

        private void ReadTxt_Click(object sender, RoutedEventArgs e)
        {
            if (!this.CheckValidations())
                return;
        }

        private void ChooseFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.txtFile.Text = openFileDialog.FileName;
                this.provider.LoadAtivos();
            }
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.CheckValidations())
                {
                    this.provider.InProcessing = true;
                    bool success = false;
                    if (this.provider.ToType == ToTypes.ToCsv)
                        success = this.provider.SaveCsv();
                    else
                        success = this.provider.SaveSql();

                    if (success)
                        MessageBox.Show("Executa com sucesso!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.provider.InProcessing = false;
            }
        }

        /// <summary>
        /// Check validations
        /// </summary>
        /// <returns></returns>
        private bool CheckValidations()
        {
            if (!this.ValidDefault()) return false;
            if (this.provider.ToType == ToTypes.ToCsv)
                return this.ValidCsv();
            return this.ValidSql();
        }

        /// <summary>
        /// Check default settings
        /// </summary>
        /// <returns></returns>
        private bool ValidDefault()
        {
            if (!this.provider.ValidFileB3()) return this.txtFile.SetFocus();
            return true;
        }

        /// <summary>
        /// Check sql settings
        /// </summary>
        /// <returns></returns>
        private bool ValidSql()
        {
            if (!this.provider.ValidConnectionString()) return this.txtCsvTo.SetFocus();
            if (!this.provider.ValidTable()) return this.txtTable.SetFocus();
            return true;
        }

        /// <summary>
        /// Check Csv settings
        /// </summary>
        /// <returns></returns>
        private bool ValidCsv()
        {
            if (!this.provider.ValidFileCsv()) return this.txtCsvTo.SetFocus();
            return true;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.b3.com.br/pt_br/market-data-e-indices/servicos-de-dados/market-data/historico/mercado-a-vista/series-historicas/");
        }

        private void SaveCsv_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "CSV file (*.csv)|*.csv";
            saveFileDialog.FileName = $"Series {DateTime.Now.ToString("ddMMyyyy HHmmss")}.csv";
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.provider.FileCsv = saveFileDialog.FileName;
        }

        private void ClearDatabase_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!this.provider.ValidConnectionString()) return;
                this.provider.ClearDatabase();
                MessageBox.Show("Processo concluído com sucesso!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
