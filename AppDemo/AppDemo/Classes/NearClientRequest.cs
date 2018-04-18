using AppDemo.Helpers.GeoUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Classes
{
    public class NearClientRequest
    {
        public Position Position { get; set; }
        public int myId { get; set; }
        public double radio { get; set; }
    }
}
