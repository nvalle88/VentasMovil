using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Models
{
    public partial class Cliente
    {
        public int idCliente { get; set; }

        public string Foto { get; set; }

        public string Firma { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }  

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public int idTipoCliente { get; set; }

        public int IdVendedor { get; set; }

        public string TelefonoMovil { get; set; }

        public string Identificacion { get; set; }

        public string Direccion { get; set; }

        public int Estado { get; set; }

        public string RazonSocial { get; set; }
    }

}
