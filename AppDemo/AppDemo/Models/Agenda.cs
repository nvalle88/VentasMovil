using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Models
{
    public partial class Agenda
    {
        public int idAgenda { get; set; }

        public int? Prioridad { get; set; }

        public DateTime? FechaFin { get; set; }

        public DateTime? FechaVista { get; set; }

        public string Notas { get; set; }

        public int idCliente { get; set; }

        public int IdVendedor { get; set; }

     }

}
