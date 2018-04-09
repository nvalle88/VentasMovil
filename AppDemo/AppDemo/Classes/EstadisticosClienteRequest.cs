using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Classes
{
    public class EstadisticosClienteRequest
    {
        // Campos de la tabla vendedor
        public int IdCliente { get; set; }

        public int? CompromisosCumplidos { get; set; }

        public int? CompromisosIncumplidos { get; set; }

        public List<TipoCompromisoRequest> ListaTipoCompromiso { get; set; }
    }
}
