using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Classes
{
    public class ArchivoRequest
    {
        public string Id { get; set; }
        public int Tipo { get; set; }
        public byte[] Array { get; set; }
    }
}
