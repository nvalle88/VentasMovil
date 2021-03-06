﻿using AppDemo.Classes;
using AppDemo.Helpers;
using AppDemo.Models;
using AppDemo.Pages;
using AppDemo.Services;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppDemo.ViewModels
{
   public  class CheckinViewModel: INotifyPropertyChanged
    {
        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
        public event PropertyChangedEventHandler PropertyChanged;
     
        #endregion

        #region Properties
        public Helpers.GeoUtils.Position position { get; set; }
        public Visita visita { get; set; }
        public List<Cliente> cliente;
        public List<Cliente> Cliente
        {
            set
            {
                if (cliente != value)
                {
                    cliente = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Cliente"));
                }
            }
            get
            {
                return cliente;
            }

        }

        public List<Tipos> Tipos { get; set; }
        public string valor { get; set; }

        public List<TipoCompromiso> tipoCompromisos;
        public List<TipoCompromiso> TipoCompromiso
        {
            set
            {
                if (tipoCompromisos != value)
                {
                    tipoCompromisos = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("TipoCompromiso"));
                }
            }
            get
            {
                return tipoCompromisos;
            }

        }

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

        private Compromiso compromiso = new Compromiso();
        public Compromiso Compromiso
        {
            set
            {
                if (compromiso != value)
                {
                    compromiso = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Compromiso"));
                }
            }
            get
            {
                return compromiso;
            }
        }


        public ObservableCollection<Compromiso> listaCompromisos;
        public ObservableCollection<Compromiso> ListaCompromiso
        {
            set
            {
                if (listaCompromisos != value)
                {
                    listaCompromisos = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ListaCompromiso"));
                }
            }
            get
            {
                return listaCompromisos;
            }
        }

        private ListRequest clienteseleccionado = new ListRequest();
        #endregion

        #region constructor
        public CheckinViewModel(ListRequest clienteseleccionado)
        {
            this.clienteseleccionado = clienteseleccionado;
            valor ="";
            position = new Helpers.GeoUtils.Position();
            visita = new Visita();
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();
      
            if (Settings.IsLoggedIn)
            {
                init();
            }
            
        }
        #endregion
        private async Task init()
        {           
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 25;
            var location = await locator.GetPositionAsync();
            position.latitude = location.Latitude;
            position.longitude = location.Longitude;
            Cliente = await apiService.GetNearClients(position, Double.Parse(Settings.RadioValue));
            try
            {
            
                if(clienteseleccionado!= null)
                {
                var selectclient = Cliente.Where(x => x.idCliente == clienteseleccionado.idCliente).FirstOrDefault();
                if (selectclient != null)
                {
                    clienteSelectItem = selectclient;
                }
                else
                {
                    await dialogService.ShowMessage("Error", "Se encuentra fuera del rango para realizar la visita");
                }
                }
            }
            catch (Exception ex)
            {
                await dialogService.ShowMessage("Error", "La acción no pudo ser ejecutada correctamente");

                Debug.WriteLine(ex.Message);
               
            }           
            ListaCompromiso = new ObservableCollection<Compromiso>();
            TipoCompromiso = await apiService.GetTipoCompromiso();            
        }

        public async void InitCompromisos(Cliente clienteSelect)
        {
            Datos = new DatosClienteRequest();

            ClienteRequest cr = new ClienteRequest
            {
                IdCliente = clienteSelect.idCliente
            };
            var Response = await apiService.ClienteData(cr);
            var estadistico = await apiService.DatosEstadisticos(cr);
            if (Response.IsSuccess)
            {
                var result = Response.Resultado.ToString();
                Datos = JsonConvert.DeserializeObject<DatosClienteRequest>(result);
            }
        }

        Cliente clienteSelect;
        public Cliente clienteSelectItem
        {
            get
            {
                return clienteSelect;
            }
            set
            {
                // marcaseleccionada = ;
                clienteSelect = value;
                InitCompromisos(clienteSelect);
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("clienteSelectItem"));

            }
        }

        TipoCompromiso tipoSelect;
        public TipoCompromiso TipoSelectItem
        {
            get
            {
                return tipoSelect;
            }
            set
            {
                // marcaseleccionada = ;

                tipoSelect = value;
            }
        }
        public ICommand AddCompromisoCommand { get { return new RelayCommand(addCompromiso); } }
        private async void addCompromiso()
        {
            compromiso.IdTipoCompromiso = tipoSelect.IdTipoCompromiso;
            if (Compromiso != null)
            {
                 ListaCompromiso.Add(Compromiso);
            }
            Compromiso = new Compromiso();

        }

        public async void submit(Stream image, int calificacion)
        {
           string  idfoto = DateTime.Now.ToString().Replace(" ", "").Replace(".", "").Replace("/", "").Replace(":", "");
            var responseImagen = await apiService.SetFileAsync(idfoto, 4, image);
            if(responseImagen.IsSuccess)
            {
                visita.idCliente = clienteSelectItem.idCliente;
                visita.IdVendedor = App.VendedorActual.IdVendedor;
                visita.Fecha = DateTime.Now;

                //Se debe modificar el id tipo visita para cuando se tenga mas de un tipo de visita
                visita.idTipoVisita = 0;
                visita.Calificacion = calificacion;
                visita.Firma = responseImagen.Resultado.ToString();

                CheckinRequest checkData = new CheckinRequest
                {
                    visita = visita,
                    compromisos = ListaCompromiso
                };

                var response = await apiService.Checkin(checkData);
                if (response.IsSuccess)
                {
                    await dialogService.ShowMessage("Ok", "Visita registrada correctamente");
                    navigationService.NavigateBack();
                    return;
                }
                await dialogService.ShowMessage("Error", "Visita no registrada");
            }
        }


        public ICommand CloseCommand { get { return new RelayCommand(Close); } }
        public async void Close()
        {
          navigationService.NavigateBack();
        }




    }
}
