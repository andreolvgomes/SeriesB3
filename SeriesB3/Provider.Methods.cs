using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    }
}
