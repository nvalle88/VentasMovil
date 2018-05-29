using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Models
{
    public class DistanciaRequest
    {
        public int IdVendedor { get; set; }
        public double? DistanciaSeguimiento { get; set; }
        public bool isSet { get; set; }
    }
}
