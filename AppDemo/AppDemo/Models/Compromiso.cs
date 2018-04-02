using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Models
{
    public partial class Compromiso
    {
        public int IdCompromiso { get; set; }

        public int IdTipoCompromiso { get; set; }

        public int idVisita { get; set; }

        public string Descripcion { get; set; }

        public string Solucion { get; set; }

    }

}
