using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDemo.Classes;

namespace AppDemo.ViewModels
{
    class PagoViewModel
    {

        public ObservableCollection<Pago> TiposPagos { get; set; }
        public string Deuda { get; set; }
        public string Valor { get; set; }


        public PagoViewModel()
        {
            Deuda = "2222";
            Valor = string.Empty;
        }

        public void CargarTiposPago()
        {
            var pago1 = new Pago { Id = 1, Nombre = "Cheque" };
            var pago2 = new Pago { Id = 1, Nombre = "Débito" };
            var pago3 = new Pago { Id = 1, Nombre = "Efectivo" };
            TiposPagos = new ObservableCollection<Pago>();
            TiposPagos.Add(pago1);
            TiposPagos.Add(pago2);
            TiposPagos.Add(pago3);
        }
      

        private Pago tipoSelect;

        public Pago TipoSelectItem
        {
            get
            {
                return tipoSelect;
            }
            set
            {
                tipoSelect = value;
            }
        }



    }
}
