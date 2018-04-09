using AppDemo.Classes;
using AppDemo.Services;
using Microcharts;
using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDemo.ViewModels
{
    public class ClienteViewModel: INotifyPropertyChanged
    {
        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Properties

        public DatosClienteRequest datos;
        public DatosClienteRequest Datos
        {
            set
            {
                if (datos != value)
                {
                    datos = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Datos"));
                }
            }
            get
            {
                return datos;
            }

        }

        public Chart Chart { get; private set; }

        #endregion

        int idCliente;
        public ClienteViewModel(ListRequest cliente)
        {
            this.idCliente = cliente.idCliente;
            apiService = new ApiService();
            datos = new DatosClienteRequest();
          //  Datos = new DatosClienteRequest();
            Init();
        }
        public async void Init()
        {
            ClienteRequest cr = new ClienteRequest
            {
                IdCliente = this.idCliente
            };
           var Response = await apiService.ClienteData(cr);
            var estadistico = await apiService.DatosEstadisticos(cr);
             if (Response.IsSuccess)
            {
                var result =  Response.Resultado.ToString();
                Datos = JsonConvert.DeserializeObject<DatosClienteRequest>(result);
            }
            if (estadistico!=null)
            {

                List<Microcharts.Entry> entries = new List<Microcharts.Entry>
                {
                 new Microcharts.Entry(estadistico.CompromisosCumplidos.Value)
                 {
                      Color=SKColor.Parse("#FF5722"),
                      Label="Compromiso cumplidos",
                      
                      ValueLabel=estadistico.CompromisosCumplidos.Value.ToString(),

                    },
                     new Microcharts.Entry(estadistico.CompromisosIncumplidos.Value)
                    {
                        Color=SKColor.Parse("#FF9800"),
                        Label="Compromiso incumplidos",
                        
                        ValueLabel=estadistico.CompromisosIncumplidos.Value.ToString(),

                    }
                };

                this.Chart = new DonutChart()
                {
                    Entries = entries,
                };
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Chart)));
            }
            Debug.WriteLine(Datos.cliente.Apellido);
        }
    }
}
