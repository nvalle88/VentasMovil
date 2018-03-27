using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Models
{
    public class VendedorRequest
    {

        // Campos de la tabla vendedor
        public int IdVendedor { get; set; }

        public int? TiempoSeguimiento { get; set; }

        public int? IdSupervisor { get; set; }




        //Campos de tabla usuario
        public string IdUsuario { get; set; }

        public string TokenContrasena { get; set; }

        public string Foto { get; set; }

        public int Estado { get; set; }

        public string Contrasena { get; set; }

        public string Correo { get; set; }

        public string Direccion { get; set; }

        public string Identificacion { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Telefono { get; set; }

        public int? idEmpresa { get; set; }
    }

}
