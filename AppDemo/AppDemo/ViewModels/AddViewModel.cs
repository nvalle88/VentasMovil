using AppDemo.Classes;
using AppDemo.Helpers;
using AppDemo.Models;
using AppDemo.Services;
using GalaSoft.MvvmLight.Command;
using Plugin.Geolocator;
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

namespace AppDemo.ViewModels
{
    public class AddViewModel : INotifyPropertyChanged
    {
        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public ClienteRequest cliente { get; set; }
        private List<TipoCliente> tipoClientes { get; set; }
        public List<TipoCliente> TipoClientes
        {
            set
            {
                if (tipoClientes != value)
                {
                    tipoClientes = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("TipoClientes"));
                }
            }
            get
            {
                return tipoClientes;
            }
        }
        private TipoCliente tipoSelect;


        #endregion
        #region constructor
        public AddViewModel()
        {
            cliente = new ClienteRequest();
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();
            tipoClientes = new List<TipoCliente>();
            tipoSelect = new TipoCliente();
        }
        #endregion

        public async Task tiposdecliente()
        {
            TipoClientes = await apiService.GetClientTypes();
        }

        public TipoCliente TipoSelectItem 
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

        private async Task Locator()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var location = await locator.GetPositionAsync();
            cliente.Latitud = location.Latitude;
            cliente.Longitud = location.Longitude;
        }



        public async void submit(Stream image)
        {
            var responseImagen = await apiService.SetFileAsync(cliente.RazonSocial, 2,image);         
          
            if (responseImagen.IsSuccess)
            {
                cliente.Firma = responseImagen.Resultado.ToString();
                Add();


                //await dialogService.ShowMessage("Informe", "Se guardo correctamente");
                //await navigationService.Navigate("MainPage");
            }
        }

        public ICommand AddCommand { get { return new RelayCommand(Add); } }
        private async void Add()
        {
            await Locator();

            if (string.IsNullOrEmpty(cliente.Nombre))
            {
                await dialogService.ShowMessage("Error", "Debe ingresar el nombre del cliente");
                return;
            }

            if (string.IsNullOrEmpty(cliente.Telefono))
            {
                await dialogService.ShowMessage("Error", "Debe ingresar el telefono del cliente");
                return;
            }

            cliente.IdVendedor = Settings.userId;
            cliente.IdEmpresa = Settings.companyId;
            cliente.IdTipoCliente = tipoSelect.idTipoCliente;




            var response = await apiService.postNewClient(cliente);
            if (response.IsSuccess)
            {
                var cliente = (Cliente)response.Result;
                await dialogService.ShowMessage("Ok", "Cliente registrado correctamente");
                //  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Locations"));
                cliente = new Cliente();
                await PopupNavigation.PopAllAsync();
                return;
            }

            await dialogService.ShowMessage("Error", "Cliente no registrado");
        }



        public ICommand CloseCommand { get { return new RelayCommand(Close); } }
        public async void Close()
        {
            //    PopupPage page = new CheckinPage();
            await navigationService.Navigate("");
        }


    }
}
