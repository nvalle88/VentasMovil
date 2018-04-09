using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Classes
{
    public class TipoCompromisoRequest
    {
        public int IdTipoCompromiso { get; set; }

        public string Descripcion { get; set; }

        // Este campo se usa para realizar el estadístico (No es parte de la tabla)
        public int? CantidadCompromiso { get; set; }
    }
}
