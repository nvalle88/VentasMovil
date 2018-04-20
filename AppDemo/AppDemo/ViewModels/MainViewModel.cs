using GalaSoft.MvvmLight.Command;
using AppDemo.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms.Maps;
using System.ComponentModel;
using AppDemo.Models;
using AppDemo.Classes;
using Xamarin.Forms;
using System.Diagnostics;
using TK.CustomMap;
using Plugin.Geolocator;
using AppDemo.Pages;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using Rg.Plugins.Popup.Pages;
using AppDemo.Helpers;
using AppDemo.Constants;
using Plugin.Geolocator.Abstractions;
using TK.CustomMap.Overlays;
using Plugin.ExternalMaps;
/// <summary>
/// Este es el View model principal desde aquí se inicializa la mayoria de las cosas
/// 
/// </summary>

namespace AppDemo.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public bool isRunning;
        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get { return isRunning; }
        }       
        #region Singleton

        static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }

        #endregion   
        #region Propeties
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public MenuItemViewModel EncabezadoMenu { get; set; }
        public LoginViewModel NewLogin { get; set; }
        public AgendaViewModel Agenda { get; set; }
        public PasswordViewModel PasswordVM { get; set; }
        public ObservableCollection<Pin> Pins { get; set; }
        public ObservableCollection<PinRequest> LocationsRequest { get; set; }
        public ObservableCollection<TKCustomMapPin> locations;
        public TKCustomMapPin MyPin { get; set; }
        public TKRoute MyRoute { get; set; }
        public ObservableCollection<ListRequest> listlocation;
        public TKCustomMapPin myPosition;

        public ICommand PinCommand;

        private Command<object> tapCommand;


        public Command<object> TapCommand
        {
            get { return tapCommand; }
            set { tapCommand = value; }
        }


        public bool IsRefreshing
        {
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshing"));
                }
            }
            get
            {
                return isRefreshing;
            }
        }

        public bool hayRuta;

        public bool HayRuta
        {
            set
            {
                if (hayRuta != value)
                {
                    hayRuta = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HayRuta"));
                }
            }
            get
            {
                return hayRuta;
            }
        }


        #endregion

        #region Attributes
        private bool isRefreshing = false;
        private NavigationService navigationService;
        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Services

        private ApiService apiService;
        private SignalRService signalRService;

        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            Pins = new ObservableCollection<Pin>();
            Locations = new ObservableCollection<TKCustomMapPin>();
            locations = new ObservableCollection<TKCustomMapPin>();
            listlocation = new ObservableCollection<ListRequest>();

            myPosition = new TKCustomMapPin();

            LocationsRequest = new ObservableCollection<PinRequest>();
            apiService = new ApiService();
            Menu = new ObservableCollection<MenuItemViewModel>();
            EncabezadoMenu = new MenuItemViewModel();
            
           
            navigationService = new NavigationService();
            NewLogin = new LoginViewModel();
            Agenda = new AgendaViewModel();
            PasswordVM = new PasswordViewModel();
            //    CheckinClient = new CheckinViewModel();
            signalRService = new SignalRService();
            MyPin = new TKCustomMapPin();

            tapCommand = new Command<object>(ProfileClient);

            LoadClientes();
            if (Settings.IsLoggedIn)
            {
               // Agenda.init();
                Locator();
            }
        }


        public async void LoadClientes()
        {           
                try
                {
                var clientes = await apiService.GetMyClient();
                Locations.Clear();
                ListLocation.Clear();
                clientes.Count();
                    if (clientes!=null && clientes.Count>0)
                    {
                    Point p = new Point(0.48, 0.96);
                        foreach (var cliente in clientes)
                        {
                        var Pincliente = new TKCustomMapPin
                        {
                            Image = "pin.png",
                            Position = new Xamarin.Forms.Maps.Position(cliente.Latitud, cliente.Longitud),
                            Anchor = p,
                                Title = "Razón Social: "+cliente.RazonSocial,
                                Subtitle = "Dirección: "+cliente.Direccion,
                                
                                ShowCallout = true,
                            };
                            var itemcliente = new ListRequest
                            {
                                Titulo=cliente.RazonSocial,
                                Subtitulo= cliente.Direccion+" "+ cliente.Telefono,           
                                idCliente=cliente.IdCliente,
                            };
                            Locations.Add(Pincliente);
                        ListLocation.Add(itemcliente);
                        }
                     }
                }
                catch
                {

                }
            }
    
        public ObservableCollection<TKCustomMapPin> Locations
    {
            protected set
            {
                locations = Locations;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Locations"));
                
            }
            get { return locations; }
        }
        public ObservableCollection<ListRequest> ListLocation
        {
            protected set
            {
                listlocation = ListLocation;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ListLocation"));

            }
            get { return listlocation; }
        }
        /// <summary>
        /// Desde aquí enviamos los datos de localización de forma periodica hacia la aplicacion web 
        /// </summary>
        /// <returns></returns>
        private async Task Locator()
        {
            await CrossGeolocator.Current.StartListeningAsync(3, 10, true);
            CrossGeolocator.Current.PositionChanged += CrossGeolocator_Current_PositionChanged;        
        }
       
        private async Task findPlace()
        {

        }

        async  void CrossGeolocator_Current_PositionChanged(object sender, PositionEventArgs e)
        {
            
            Device.BeginInvokeOnMainThread(() =>
            {
                var position = e.Position;
                myPosition.Position = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);

                //e.Position.Latitude,0);
                
                // e.Position.Longitude;
            });

           await apiService.LogRuta(new LogRutaVendedor { IdVendedor = Settings.userId, Latitud = e.Position.Latitude, Longitud = e.Position.Longitude, Fecha=DateTime.Now });
           await  signalRService.SendPosition((float)e.Position.Latitude, (float)e.Position.Longitude);

        }


        #endregion

        #region Commands
        /// <summary>
        /// En esta Región se encuantran los commands que estan realcionados con los botones de las vistas
        /// </summary>
        public ICommand VerificarAutoCommand { get { return new RelayCommand(VerificarAutonavigate); } }
        public async void VerificarAutonavigate()
        {
            await navigationService.Navigate("VerificarAutoPage");
            IsRefreshing = false;
        }      
        private async void ProfileClient(object obj)
        {
            ListRequest cliente = (ListRequest)obj;
            await App.Navigator.PushAsync(new ClientePage(cliente));
          //  Debug.WriteLine(cliente.idCliente);
        }


        public ICommand PinSelected { get { return new RelayCommand(pinselected); } }

        public async void pinselected()
        {
            HayRuta = true;                     
        }

        public ICommand PinUnselected { get { return new RelayCommand(pinunselected); } }

        public async void pinunselected()
        {
            HayRuta = false;
        }

        public ICommand RefreshDataCommand { get { return new RelayCommand(RefreshData); } }
        public void RefreshData()
        {
            LoadClientes();
        }

        public ICommand OpenInMap { get { return new RelayCommand(openinMap); } }
        public async void openinMap()
        {
            var success = await CrossExternalMaps.Current.NavigateTo(MyPin.Title, MyPin.Position.Latitude, MyPin.Position.Longitude);
        }

        //   public ICommand RefreshParkingCommand { get { return new RelayCommand(RefreshData); } }


       





        public ICommand AddNewClientCommand { get { return new RelayCommand(AddNewClient); } }
        public async void AddNewClient()
        {
            //    PopupPage page = new AddPage();
            //    await PopupNavigation.PushAsync(page);

            await navigationService.Navigate("AddClientePage");
        }

        public ICommand CalendarCommand { get { return new RelayCommand(Calendar); } }
        public async void Calendar()
        {
            //    PopupPage page = new AddPage();
            //    await PopupNavigation.PushAsync(page);

            await navigationService.Navigate("AgendaPage");
        }

        public ICommand AddCheckinCommand { get { return new RelayCommand(AddCheckin); } }
        public async void AddCheckin()
        {
            //PopupPage page = new CheckinPage();          
            //await PopupNavigation.PushAsync(page);

            await navigationService.Navigate("CheckinClientePage");


        }



        #endregion

        #region Methods
     
        /// <summary>
        /// Se arma el menu y en el encabezado del menú se muestra el nombre del agente logeado en la aplicación
        /// </summary>
        /// <param name="Agente"></param>

        public void LoadMenu(string Agente, string AgenteFoto)
        {
            Menu.Clear();
            Menu.Add(new MenuItemViewModel
            {

                PageName = "ListClientPage",
                Icon = "pcontact.png",
                Title = "Clientes",
                SubTitle = "",

            });

            Menu.Add(new MenuItemViewModel
            {
                
                PageName = "CheckinClientePage",
                Icon = "dir.png",
                Title = "Checkin",
                SubTitle = "",
            });

            Menu.Add(new MenuItemViewModel
            {

                PageName = "AgendaPage",
                Icon = "tipo.png",
                Title = "Agenda",
                SubTitle = "",
            });

            //Menu.Add(new MenuItemViewModel
            //{
            //    PageName = "ConsultarMultas",
            //    Icon = "checkin.png",
            //    Title = "Noticias",
            //    SubTitle = "",
            //});
            EncabezadoMenu.Agente = Agente;
            EncabezadoMenu.AgenteFoto = AgenteFoto.Replace("~", Constants.Constants.VentasWS);
        }
        #endregion
    }
}
