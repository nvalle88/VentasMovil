using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Models
{
    public partial class LogRutaVendedor
    {
        public int IdLogRutaVendedor { get; set; }

        public double? Latitud { get; set; }

        public double? Longitud { get; set; }

        public DateTime? Fecha { get; set; }

        public int IdVendedor { get; set; }

    }

}
