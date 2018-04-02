using AppDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Classes
{
    public class CheckinRequest
    {
        public List<Compromiso> compromisos { get; set; }
        public Visita visita { get; set; }
    }
}
