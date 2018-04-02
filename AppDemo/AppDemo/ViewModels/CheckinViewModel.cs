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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public Compromiso compromiso { get; set; }

        public List<Compromiso> listaCompromisos;
        public List<Compromiso> ListaCompromiso
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

        #endregion

        #region constructor
        public CheckinViewModel()
        {
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
            compromiso = new Compromiso();
        }
        #endregion
        private async Task init()
        {           
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 25;
            var location = await locator.GetPositionAsync();
            position.latitude = location.Latitude;
            position.longitude = location.Longitude;
            Cliente = await apiService.GetNearClients(position);
            ListaCompromiso = new List<Compromiso>();
            TipoCompromiso = await apiService.GetTipoCompromiso();            
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
            if (compromiso != null)
            {
                 ListaCompromiso.Add(compromiso);
            }
        }



      //  public ICommand CheckCommand { get { return new RelayCommand(Checkin); } }

        public async void submit(Stream image, int calificacion)
        {
           string  idfoto = DateTime.Now.ToString().Replace(" ", "").Replace(".", "").Replace("/", "").Replace(":", "");
            var responseImagen = await apiService.SetFileAsync(idfoto, 4, image);
            if(responseImagen.IsSuccess)
            {
                visita.idCliente = clienteSelectItem.idCliente;
                visita.IdVendedor = App.VendedorActual.IdVendedor;
                visita.Fecha = DateTime.Now;
                visita.idTipoVisita = 2;
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
                    return;
                }
                await dialogService.ShowMessage("Error", "Cliente no registrado");
            }

           

            //if(visita!=null)
            //{
            //   var result= await apiService.Checkin(visita);
            //    if (result.IsSuccess)
            //    {
            //        await dialogService.ShowMessage("Checkin", "Se agrego su visita correctamente");
            //        var resultado = result.Result.ToString();
            //        var visitadata = JsonConvert.DeserializeObject<Visita>(resultado);

            //        await App.Navigator.PushAsync(new FormPage(visitadata));

            //        await PopupNavigation.PopAllAsync();

            //    }
            //}
            
        }


        public ICommand CloseCommand { get { return new RelayCommand(Close); } }
        public async void Close()
        {
            //    PopupPage page = new CheckinPage();

            navigationService.NavigateBack();
            
           // await PopupNavigation.PopAllAsync();
        }




    }
}
