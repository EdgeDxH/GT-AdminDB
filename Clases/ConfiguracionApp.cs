using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT_AdminDB.Clases
{
    public class ConfiguracionApp
    {
        public string PathDB { get; set; }
    }

    public class ConfiguracionRaiz
    {
        public ConfiguracionApp ConfiguracionApp { get; set; }
    }
}
