using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Windows.Media.AppBroadcasting;
using System.Diagnostics;

namespace GT_AdminDB.Clases
{
    public class RaidCollection
    {
        private ObservableCollection<Raid> raids =  new ObservableCollection<Raid>();
        public ObservableCollection<Raid> Raids { get => raids; set => raids = value; }

        public void GetRaids()
        {
            Conexion conexion = new Conexion();
            using (conexion.Connection)
            {
                conexion.Connection.Open();
                string consulta = "SELECT id, fecha_inicio FROM RAID";
                using (SQLiteCommand comando = new SQLiteCommand(consulta, conexion.Connection))
                {
                    var reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Raid raid = new Raid
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Fecha_Inicio = DateOnly.Parse(reader["fecha_inicio"].ToString())
                        };
                        this.Raids.Add(raid);
                    }
                }
                conexion.Connection.Close();
            }
        }

        public bool Add(Raid raidAdd)
        {
            bool error = false;
            Conexion conexion = new Conexion();
            using (conexion.Connection)
            {
                conexion.Connection.Open();
                using (var transanction = conexion.Connection.BeginTransaction())
                {
                    try
                    {
                        string consulta = $"INSERT INTO RAID (fecha_inicio) VALUES ('{raidAdd.Fecha_Inicio}')";
                        using (SQLiteCommand cmd = new SQLiteCommand(consulta, conexion.Connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        transanction.Commit();
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        Debug.WriteLine(ex.ToString());
                    }
                }
                conexion.Connection.Close();
            }
            return error;
        }

        public bool AddRaidParticipacion(Raid raid)
        {
            bool error = false;
            Conexion conexion = new Conexion();
            using (conexion.Connection)
            {
                try
                {
                    conexion.Connection.Open();
                    using (var transaction = conexion.Connection.BeginTransaction())
                    {
                        string consulta = $"INSERT INTO RAID (fecha_inicio) VALUES ('{raid.Fecha_Inicio}')";
                        using (SQLiteCommand cmd = new SQLiteCommand(consulta, conexion.Connection))
                        {
                            cmd.Transaction = transaction;
                            cmd.ExecuteNonQuery();
                        }
                        string consultaId = @"SELECT last_insert_rowid()";
                        using (SQLiteCommand commandQuery = new SQLiteCommand(consultaId, conexion.Connection))
                        {
                            commandQuery.Transaction = transaction;
                            raid.Id = Convert.ToInt32(commandQuery.ExecuteScalar());
                        }

                        string consultaIdMiembros = @"SELECT id FROM MIEMBRO";
                        List<int> idMiembros = new List<int>();
                        using (SQLiteCommand cmd = new SQLiteCommand(consultaIdMiembros, conexion.Connection))
                        {
                            cmd.Transaction = transaction;
                            using (SQLiteDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    idMiembros.Add(reader.GetInt32(0)); // Leer cada id de miembro
                                }
                            }
                        }

                        foreach (int idMiembro in idMiembros)
                        {
                            string consultaCrearParticipacion = $"INSERT INTO PARTICIPACION (total_damage,intentos_totales,fk_id_raid,fk_id_miembro) VALUES ({0},{0},{raid.Id},{idMiembro})";
                            using (SQLiteCommand cmd = new SQLiteCommand(consultaCrearParticipacion, conexion.Connection))
                            {
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    error = true;
                    Debug.WriteLine(ex.ToString());
                }
                conexion.Connection.Close();
            }
            this.Raids.Add(raid);
            return error;
        }

        public bool DeleteRaidById(int idRaid)
        {
            bool error = false;
            Conexion conexion = new Conexion();

            using (conexion.Connection)
            {
                conexion.Connection.Open();

                try
                {
                    using (var transaction = conexion.Connection.BeginTransaction())
                    {
                        string consultaEliminarParticipacion = $"DELETE FROM PARTICIPACION WHERE fk_id_raid = {idRaid}";
                        using (SQLiteCommand cmd = new SQLiteCommand(consultaEliminarParticipacion,conexion.Connection))
                        {
                            cmd.Transaction = transaction;
                            cmd.ExecuteNonQuery();
                        }
                        string consultaELiminarRaid = $"DELETE FROM RAID WHERE id = {idRaid}";
                        using (SQLiteCommand cmd = new SQLiteCommand(consultaELiminarRaid, conexion.Connection))
                        {
                            cmd.Transaction= transaction;
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    error = true;
                    Debug.WriteLine(ex.ToString());
                }
                conexion.Connection.Close();
            }
            List<Raid> listaTemp = Raids.ToList();
            foreach (Raid raidTemp in listaTemp)
            {
                if (raidTemp.Id == idRaid)
                {
                    Raids.Remove(raidTemp);
                    break;
                }
            }
            return error;
        }
    }
}
