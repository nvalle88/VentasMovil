using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Classes
{
    public class ClienteRequest
    {
        public int IdCliente { get; set; }

        public int? IdEmpresa { get; set; }

        public string Identificacion { get; set; }

        public string Foto { get; set; }

        public string Firma { get; set; }

        public double Latitud { get; set; }

        public double Longitud { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Telefono { get; set; }

        public string TelefonoMovil { get; set; }

        public string Email { get; set; }

        public int IdTipoCliente { get; set; }

        public string TipoCliente { get; set; }

        public int IdVendedor { get; set; }

        public string NombresVendedor { get; set; }

        public string ApellidosVendedor { get; set; }

        public string Direccion { get; set; }
    }
}
