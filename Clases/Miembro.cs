using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GT_AdminDB.Clases
{
    public class Miembro : INotifyPropertyChanged
    {
        private int id;
        private string nombre;
        private DateOnly fecha_ingreso;
        private DateOnly ultimo_login;

        public int Id
        {
            get => id;
            set
            {
                id = value;
            }
        }
        public string Nombre
        {
            get => nombre;
            set
            {
                nombre = value;
                OnPropertyChanged();
            }
        }
        public DateOnly Fecha_Ingreso
        {
            get => fecha_ingreso;
            set
            {
                fecha_ingreso = value;
                OnPropertyChanged();
            }
        }
        public DateOnly Ultimo_Login
        {
            get => ultimo_login;
            set
            {
                ultimo_login = value;
                OnPropertyChanged();
            }
        }

        public void ReadAll()
        {
            Debug.WriteLine($"{this.id}\n{this.nombre}\n{this.fecha_ingreso}\n{this.ultimo_login}");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
