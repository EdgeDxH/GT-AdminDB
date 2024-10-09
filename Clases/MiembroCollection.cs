using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GT_AdminDB.Clases
{
    public class MiembroCollection
    {
        private ObservableCollection<Miembro> miembros = new ObservableCollection<Miembro>();

        public ObservableCollection<Miembro> Miembros { get => miembros; set => miembros = value; }

        public bool Add(Miembro miembroTemp)
        {
            bool error = false;
            try
            {
                Conexion conexion = new Conexion();
                using (conexion.Connection)
                {
                    conexion.Connection.Open();
                    Debug.WriteLine("Conexión a la base de datos SQLite establecida.");

                    using (var transaction = conexion.Connection.BeginTransaction())
                    {
                        try
                        {
                            string insertQuery = $"INSERT INTO MIEMBRO (nombre,fecha_ingreso,ultimo_login) VALUES ('{miembroTemp.Nombre}','{miembroTemp.Fecha_Ingreso}','{miembroTemp.Ultimo_Login}')";
                            using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, conexion.Connection))
                            {
                                cmd.ExecuteNonQuery();
                            }
                            string consulta = @"SELECT last_insert_rowid()";
                            using (SQLiteCommand cmd = new SQLiteCommand(consulta, conexion.Connection))
                            {
                                SQLiteCommand commandQuery = new SQLiteCommand(consulta, conexion.Connection);
                                miembroTemp.Id = Convert.ToInt32(commandQuery.ExecuteScalar());
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Debug.WriteLine(ex);
                            error = true;
                        }
                    }
                    conexion.Connection.Close();
                    Debug.WriteLine("Conexión cerrada.");
                }
                this.Miembros.Add(miembroTemp);
                return error;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return true;
            }
        }

        public void GetMiembros(bool oraderByLogin = false)
        {
            Miembros.Clear();
            Conexion conexion = new Conexion();
            using (conexion.Connection)
            {
                conexion.Connection.Open();
                string consulta = "";
                if (oraderByLogin)
                {
                    consulta = "SELECT id, nombre, fecha_ingreso, ultimo_login FROM MIEMBRO ORDER BY ultimo_login ASC";
                }
                else
                {
                    consulta = "SELECT id, nombre, fecha_ingreso, ultimo_login FROM MIEMBRO";
                }
                
                using (SQLiteCommand comando = new SQLiteCommand(consulta, conexion.Connection))
                {
                    SQLiteDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Miembro miembro = new Miembro
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Nombre = reader["nombre"].ToString(),
                            Fecha_Ingreso = DateOnly.Parse(reader["fecha_ingreso"].ToString()),
                            Ultimo_Login = DateOnly.Parse(reader["ultimo_login"].ToString())
                        };
                        this.Miembros.Add(miembro);
                    }
                }
                conexion.Connection.Close();
            }
        }

        public void Update(Miembro miembroTemp)
        {
            Conexion conexion = new Conexion();
            using (conexion.Connection)
            {
                conexion.Connection.Open();
                Debug.WriteLine("Conexión a la base de datos SQLite establecida.");

                string insertQuery = $"UPDATE MIEMBRO SET nombre = '{miembroTemp.Nombre}',fecha_ingreso = '{miembroTemp.Fecha_Ingreso}',ultimo_login = '{miembroTemp.Ultimo_Login}' WHERE id = {miembroTemp.Id}";
                SQLiteCommand command = new SQLiteCommand(insertQuery, conexion.Connection);
                command.ExecuteNonQuery();

                conexion.Connection.Close();
                Debug.WriteLine("Conexión cerrada.");
            }
        }

        public void DeleteById(int idMiembro)
        {
            Conexion conexion = new Conexion();
            using (conexion.Connection)
            {
                conexion.Connection.Open();
                Debug.WriteLine("Conexión a la base de datos SQLite establecida.");
                //ELIMINAR PARTICIPACIONES
                string delParticipacion = $"DELETE FROM PARTICIPACION WHERE fk_id_miembro = {idMiembro}";
                using (var cmd = new SQLiteCommand(delParticipacion, conexion.Connection))
                {
                    cmd.ExecuteNonQuery();
                }
                //ELIMINAR MIEMBRO
                string delMiembro = $"DELETE FROM MIEMBRO WHERE id = {idMiembro}";
                using (var cmd = new SQLiteCommand(delMiembro, conexion.Connection))
                {
                    cmd.ExecuteNonQuery();
                }

                conexion.Connection.Close();
                Debug.WriteLine("Conexión cerrada.");
            }
            List<Miembro> listTemp = miembros.ToList<Miembro>();
            foreach (Miembro miembroWeon in listTemp)
            {
                if (miembroWeon.Id == idMiembro)
                {
                    miembros.Remove(miembroWeon);
                    break;
                }
            }
        }

        public void ActualizarLogin(List<Miembro> listaLogin)
        {
            Conexion conexion = new Conexion();
            using (conexion.Connection)
            {
                conexion.Connection.Open();
                using (var transaction = conexion.Connection.BeginTransaction())
                {
                    try
                    {
                        foreach (Miembro miembro in listaLogin)
                        {
                            string consulta = $"UPDATE MIEMBRO SET ultimo_login = '{miembro.Ultimo_Login}' WHERE id = {miembro.Id}";
                            using (var command = new SQLiteCommand(consulta, conexion.Connection, transaction))
                            {
                                command.ExecuteNonQuery(); // Ejecutar la actualización
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Debug.WriteLine("Error al actualizar login"+"\n"+ex.ToString());
                    }
                }
                conexion.Connection.Close();
                }
        }
    }
}
