using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesB3
{
    /// <summary>
    /// read file txt b3
    /// </summary>
    public class ReadFile : IDisposable
    {
        /// <summary>
        /// Readl file
        /// </summary>
        /// <param name="file"></param>
        public IEnumerable<Infors> Series(string file)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                string ln;
                int counter = 0;

                List<Infors> infors = new List<Infors>();
                while ((ln = reader.ReadLine()) != null)
                {
                    counter++;

                    // it's header, next
                    if (counter == 1)
                        continue;

                    if (!this.Check(ln))
                        continue;

                    Infors inf = new Infors();
                    //infors.Add(inf);

                    inf.Codneg = this.Str(ln, 12, 13);
                    inf.Empresa = this.Str(ln, 12, 28);
                    inf.Data = this.Date(ln, 8, 3);

                    inf.Abertura = this.Value(ln, 11, 57);
                    inf.Maxima = this.Value(ln, 11, 70);
                    inf.Minima = this.Value(ln, 11, 83);
                    inf.Fechamento = this.Value(ln, 11, 109);

                    // return line by line
                    yield return inf;
                }
            }
        }

        private bool Check(string ln)
        {
            // http://www.b3.com.br/data/files/C8/F3/08/B4/297BE410F816C9E492D828A8/SeriesHistoricas_Layout.pdf
            // TABELA DE CODBDI - RELAÇÃO DOS VALORES PARA CÓDIGOS DE BDI
            // 02 LOTE PADRAO
            // 96 MERCADO FRACIONARIO
            // TABELA DE CODBDI - RELAÇÃO DOS VALORES PARA CÓDIGOS DE BDI 
            // 05 SANCIONADAS PELOS REGULAMENTOS BMFBOVESPA
            // 07 RECUPERACAO EXTRAJUDICIAL
            // 08 RECUPERAÇÃO JUDICIAL
            // 12 FUNDOS IMOBILIARIOS   
            // 58 OUTROS
            // .... E OUTRO, TUDO SE ENCAIXA COMO MERCADO A VISTA, ENTÃO NÃO LEVAR EM CONSIDERAÇÃO SOMENTE O "TPMERC"
            if (this.Str(ln, 2, 11) == "02")
            {
                // TABELA DE TPMERC - RELAÇÃO DOS VALORES PARA TIPO DE MERCADO
                // 010 VISTA
                // 020 FRACIONÁRIO
                if (this.Str(ln, 3, 25) == "010")
                    return true;
            }
            return false;
        }

        public IEnumerable<string> CodNeg(string file)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                string ln;
                int counter = 0;

                List<Infors> infors = new List<Infors>();
                while ((ln = reader.ReadLine()) != null)
                {
                    counter++;

                    // it's header, next
                    if (counter == 1)
                        continue;

                    if (!this.Check(ln))
                        continue;

                    string codneg = this.Str(ln, 12, 13);
                    // return line by line
                    yield return codneg;
                }
            }
        }

        /// <summary>
        /// Get value decimal/value
        /// </summary>
        /// <param name="ln"></param>
        /// <param name="lenght"></param>
        /// <param name="pos_start"></param>
        /// <param name="pos_end"></param>
        /// <returns></returns>
        private decimal Value(string ln, int lenght, int pos_start, int pos_end = 0)
        {
            // http://bvmf.bmfbovespa.com.br/pt-br/cotacoes-historicas/FormTutorial.asp
            // N(11)V(99): Campo Numérico com 11 caracteres antes da vírgula e 2 após.
            string value = ln.Substring(pos_start - 1, lenght).Trim();
            if (string.IsNullOrEmpty(value))
                return 0;

            string decimais = ln.Substring((pos_start + lenght) - 1, 2).Trim();
            if (string.IsNullOrEmpty(decimais))
                decimais = "00";

            value += "," + decimais;
            return decimal.Parse(value);
        }

        /// <summary>
        /// Get value str
        /// </summary>
        /// <param name="ln"></param>
        /// <param name="lenght"></param>
        /// <param name="pos_start"></param>
        /// <param name="pos_end"></param>
        /// <returns></returns>
        private string Str(string ln, int lenght, int pos_start, int pos_end = 0)
        {
            return ln.Substring(pos_start - 1, lenght).Trim();
        }

        /// <summary>
        /// Get value date
        /// </summary>
        /// <param name="ln"></param>
        /// <param name="lenght"></param>
        /// <param name="pos_start"></param>
        /// <param name="pos_end"></param>
        /// <returns></returns>
        private DateTime Date(string ln, int lenght, int pos_start, int pos_end = 0)
        {
            string value = Str(ln, lenght, pos_start);

            DateTime parsedDate = DateTime.ParseExact(value, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            string formattedDate = parsedDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            return Convert.ToDateTime(formattedDate);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
