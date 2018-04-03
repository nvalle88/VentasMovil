using AppDemo.Classes;
using AppDemo.Services;
using Newtonsoft.Json;
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
             if (Response.IsSuccess)
            {
                var result =  Response.Resultado.ToString();
                Datos = JsonConvert.DeserializeObject<DatosClienteRequest>(result);

            }
            Debug.WriteLine(Datos.cliente.Apellido);
        }
    }
}
