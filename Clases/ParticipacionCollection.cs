using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
using System.Transactions;

namespace GT_AdminDB.Clases
{
    class ParticipacionCollection
    {
        private ObservableCollection<Participacion> participaciones = new ObservableCollection<Participacion>();
        public ObservableCollection<Participacion> Participaciones { get => participaciones; set => participaciones = value; }

        public void GetParticipaciones()
        {
            //Falta implementar el get all ya que no tiene uso por el momento, ahora mismo solo de puede buscar por id de miembro
            Conexion conexion = new Conexion();
            using (conexion.Connection)
            {
                conexion.Connection.Open();
                using (var transaction = conexion.Connection.BeginTransaction())
                {
                    try
                    {
                        string consulta = "SELECT id, total_damage, intentos_totales, fk_id_raid, fk_id_miembro FROM PARTICIPACION";
                        using (SQLiteCommand cmd = new SQLiteCommand(consulta, conexion.Connection))
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                Participacion participacionTemp = new Participacion(){
                                    Id = reader.GetInt32(0),
                                    Total_Damage = reader.GetInt32(1),
                                    Intentos_Totales = reader.GetInt32(2),
                                    Fk_Id_Raid = reader.GetInt32(3),
                                    Fk_Id_Miembro = reader.GetInt32(4),
                                };
                                participaciones.Add(participacionTemp);
                            }
                        }
                        transaction.Commit();
                    } 
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine(ex.ToString());
                    }
                }
                conexion.Connection.Close();
            }
        }

        public void GetById(int idMiembro, bool orderByFechaInicio = false)
        {
            string test = "";
            try
            {
                Conexion conexion = new Conexion();
                
                string consulta = $"SELECT PARTICIPACION.id as p_id, intentos_totales, total_damage, fk_id_miembro, fk_id_raid, fecha_inicio FROM PARTICIPACION JOIN RAID ON PARTICIPACION.fk_id_raid = RAID.id WHERE fk_id_miembro = {idMiembro.ToString()}";
                if (orderByFechaInicio)
                {
                    consulta = consulta + " " + @"ORDER BY SUBSTR(fecha_inicio,7,4)||'-'||SUBSTR(fecha_inicio,4,2)||'-'||SUBSTR(fecha_inicio,1,2) DESC";
                }
                using (conexion.Connection)
                {
                    conexion.Connection.Open();
                    using (SQLiteCommand comando = new SQLiteCommand(consulta, conexion.Connection))
                    {
                        var reader = comando.ExecuteReader();
                        while (reader.Read())
                        {
                            Participacion participacion = new Participacion
                            {
                                Id = Convert.ToInt32(reader["p_id"]),
                                Intentos_Totales = Convert.ToInt32(reader["intentos_totales"]),
                                Total_Damage = Convert.ToInt32(reader["total_damage"]),
                                Fk_Id_Miembro = Convert.ToInt32(reader["fk_id_miembro"]),
                                Fk_Id_Raid = Convert.ToInt32(reader["fk_id_raid"]),
                                Raid = new Raid()
                                {
                                    Id = Convert.ToInt32(reader["fk_id_raid"]),
                                    Fecha_Inicio = DateOnly.Parse(reader["fecha_inicio"].ToString())
                                }
                            };
                            this.Participaciones.Add(participacion);
                        }
                    }
                    conexion.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Debug.WriteLine(test);
            }
        }

        public void Add(Participacion participacionAdd)
        {
            Conexion conexion = new Conexion();
            using (conexion.Connection)
            {
                conexion.Connection.Open();
                using (var transaction = conexion.Connection.BeginTransaction())
                {
                    try
                    {
                        string consulta = $"INSERT INTO PARTICIPACION (intentos_totales, total_damage,fk_id_raid,fk_id_miembro) VALUES ({participacionAdd.Intentos_Totales},{participacionAdd.Total_Damage},{participacionAdd.Fk_Id_Raid},{participacionAdd.Fk_Id_Miembro})";
                        using (SQLiteCommand cmd = new SQLiteCommand(consulta,conexion.Connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Debug.WriteLine(ex.ToString());
                    }
                }
                conexion.Connection.Close();
            }
        }

        public void UpdateParticipacion(Participacion participacionEdit)
        {
            try
            {
                Conexion conexion = new Conexion();
                using (conexion.Connection)
                {
                    conexion.Connection.Open();
                    using (var transaction = conexion.Connection.BeginTransaction())
                    {
                        try
                        {
                            string consulta = $"UPDATE PARTICIPACION SET total_damage = {participacionEdit.Total_Damage},intentos_totales = {participacionEdit.Intentos_Totales},fk_id_raid = {participacionEdit.Fk_Id_Raid} WHERE id = {participacionEdit.Id}";
                            using (SQLiteCommand cmd = new SQLiteCommand(consulta, conexion.Connection))
                            {
                                cmd.ExecuteNonQuery();
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Debug.WriteLine(ex.ToString());
                        }
                    }
                    conexion.Connection.Close();
                }
            }
            catch
            {
                Debug.WriteLine("Error en la base de datos");
            }
        }

        public void DeleteParticipacionById(int idParticipacion)
        {
            Conexion conexion = new Conexion();
            using (conexion.Connection)
            {
                conexion.Connection.Open();
                try
                {
                    using (var transaction = conexion.Connection.BeginTransaction())
                    {
                        string consulta = $"DELETE FROM PARTICIPACION WHERE id = {idParticipacion}";
                        using (SQLiteCommand cmd = new SQLiteCommand(consulta, conexion.Connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                conexion.Connection.Close();
            }
            List<Participacion> listTemp = Participaciones.ToList();
            foreach (Participacion partTemp in listTemp)
            {
                if (partTemp.Id == idParticipacion)
                {
                    Participaciones.Remove(partTemp);
                }
            }
        }
    }
}
