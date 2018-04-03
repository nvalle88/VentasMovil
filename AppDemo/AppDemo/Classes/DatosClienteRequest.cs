using AppDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Classes
{
    public class DatosClienteRequest
    {
        public Cliente cliente { get; set; }
        public List<Compromiso> compromisos { get; set; }
    }
}
