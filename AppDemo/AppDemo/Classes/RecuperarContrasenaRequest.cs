using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Classes
{
    public class RecuperarContrasenaRequest
    {
        public string Email { get; set; }

        public string Codigo { get; set; }

        public string Contrasena { get; set; }

        public string ConfirmarContrasena { get; set; }
    }
}
