using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GT_AdminDB.Clases
{
    public class Raid : INotifyPropertyChanged
    {
        private int id;
        private DateOnly fecha_inicio;

        public int Id
        {
            get => this.id;
            set
            {
                this.id = value;
            }
        }
        public DateOnly Fecha_Inicio
        {
            get => this.fecha_inicio;
            set
            {
                this.fecha_inicio = value;
                OnPropertyChanged();
            }
        }

        public void ReadAll()
        {
            Debug.WriteLine($"{this.id}\n{this.fecha_inicio}");
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
