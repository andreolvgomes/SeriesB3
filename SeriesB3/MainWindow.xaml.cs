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
            if (!this.ValidaText())
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

        private void ExportToCsv_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.ValidaText())
                {
                    ReadFile read = new ReadFile();
                    using (StreamWriter outputFile = new StreamWriter(this.provider.FileCsv))
                    {
                        int counter = 0;
                        foreach (Infors inf in read.Series(this.txtFile.Text))
                        {
                            if (counter == 0)
                                outputFile.WriteLine($"Ativo,Data,Abertura,Maxima,Minima,Fechamento");

                            outputFile.WriteLine($@"{inf.Ativo},{inf.Data.ToString("dd/MM/yyyy")},{inf.Abertura.Point()},{inf.Maxima.Point()},{inf.Minima.Point()},{inf.Fechamento.Point()}");
                            counter++;
                        }
                    }

                    MessageBox.Show("CSV gerado com sucesso!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidaText()
        {
            if (string.IsNullOrEmpty(this.txtFile.Text))
            {
                MessageBox.Show("Informe o arquivo!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return this.SetFocus(this.txtFile);
            }
            if (string.IsNullOrEmpty(this.provider.FileCsv))
            {
                MessageBox.Show("Informe o caminho para salvar o Csv!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return this.SetFocus(this.txtCsvTo);
            }
            return true;
        }

        public bool SetFocus(TextBox text)
        {
            text.Dispatcher.BeginInvoke(new Action(() =>
            {
                text.Focus();
                text.SelectAll();
            }));
            return false;
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
    }
}
