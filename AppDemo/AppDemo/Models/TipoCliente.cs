using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Models
{
    public partial class TipoCliente
    {
      
        public int idTipoCliente { get; set; }
        public string Tipo { get; set; }
        public int? IdEmpresa { get; set; }
        
    }

}
