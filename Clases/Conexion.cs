using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace GT_AdminDB.Clases
{
    public class Conexion
    {
        //la conexion string es statica, por lo tanto al cambiar la ruta hay que riniciar la aplicacion
        private static string connectionString = @$"Data Source={(App.Current as App).ConfiguracionApp.PathDB};Version=3;FailIfMissing=True;Foreign Keys=True;";
        private SQLiteConnection connection = new SQLiteConnection(connectionString);

        public SQLiteConnection Connection { get { return connection; } }
    }
}
