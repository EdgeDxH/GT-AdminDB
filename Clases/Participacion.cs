using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GT_AdminDB.Clases
{
    public class Participacion : INotifyPropertyChanged
    {
        private int id;
        private int total_damage;
        private int intentos_totales;
        private int fk_id_miembro;
        private int fk_id_raid;
        private Raid raid;

        public int Id
        {
            get => id;
            set
            {
                id = value;
            }
        }
        public int Total_Damage 
        {
            get => this.total_damage;
            set
            {
                this.total_damage = value;
                OnPropertyChanged();
            }
        }
        public int Intentos_Totales
        {
            get => this.intentos_totales;
            set
            {
                this.intentos_totales = value;
                OnPropertyChanged();
            }
        }
        public int Fk_Id_Miembro
        {
            get => this.fk_id_miembro;
            set
            {
                this.fk_id_miembro = value;
            }
        }
        public int Fk_Id_Raid
        {
            get => this.fk_id_raid;
            set
            {
                this.fk_id_raid = value;
            }
        }
        public Raid Raid
        {
            get => this.raid;
            set
            {
                this.raid = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
