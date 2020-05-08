using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesB3
{
    //http://bvmf.bmfbovespa.com.br/pt-br/cotacoes-historicas/FormTutorial.asp
    public class Infors
    {        
        public override string ToString()
        {
            return $"{this.Ativo} - {Data}: Open {this.Abertura} High: {this.Maxima} Low: {this.Minima} Close: {this.Fechamento}";
        }

        /// <summary>
        /// CODNEG - CÓDIGO DE NEGOCIAÇÃO DO PAPEL
        /// X(12) 13 24
        /// </summary>
        public string Ativo { get; set; }
        /// <summary>
        /// DATA DO PREGÃO
        /// N(08) 03 10
        /// </summary>
        public DateTime Data { get; set; }
        /// <summary>
        /// PREABE - PREÇO DE ABERTURA DO PAPELMERCADO NO PREGÃO
        /// (11)V99 57 69
        /// </summary>
        public decimal Abertura { get; set; }
        /// <summary>
        /// PREMAX - PREÇO MÁXIMO DO PAPELMERCADO NO PREGÃO
        /// (11)V99 70 82
        /// </summary>
        public decimal Maxima { get; set; }
        /// <summary>
        /// PREMIN - PREÇO MÍNIMO DO PAPELMERCADO NO PREGÃO
        /// (11)V99 83 95
        /// </summary>
        public decimal Minima { get; set; }
        /// <summary>
        /// PREULT - PREÇO DO ÚLTIMO NEGÓCIO DO PAPEL-MERCADO NO PREGÃO
        /// </summary>
        public decimal Fechamento { get; set; }
        /// <summary>
        /// NOMRES - NOME RESUMIDO DA EMPRESA EMISSORA DO PAPEL
        /// X(12) 28 39
        /// </summary>
        public string Empresa { get; set; }
    }
}