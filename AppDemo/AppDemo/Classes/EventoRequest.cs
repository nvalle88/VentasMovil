using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Classes
{
    public class EventoRequest
    {
        // Campos de la tabla agenda
        public int idAgenda { get; set; }

        public int? Prioridad { get; set; }

        public DateTime? FechaFin { get; set; }

        public DateTime? FechaVista { get; set; }

        public string Notas { get; set; }

        public int idCliente { get; set; }

        public int IdVendedor { get; set; }


        // Campos de la tabla compromiso
        public int IdCompromiso { get; set; }

        public int? IdTipoCompromiso { get; set; }

        public int? idVisita { get; set; }

        public string Descripcion { get; set; }

        public string Solucion { get; set; }

    }
}
