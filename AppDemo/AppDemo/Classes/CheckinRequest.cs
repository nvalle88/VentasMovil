using AppDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.Classes
{
    public class CheckinRequest
    {
        public ObservableCollection<Compromiso> compromisos { get; set; }
        public Visita visita { get; set; }
    }
}
