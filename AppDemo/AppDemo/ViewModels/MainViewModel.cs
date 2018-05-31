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

        public Helpers.GeoUtils.Position positionAux = new Helpers.GeoUtils.Position();

        public double distancia=0.02;

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
        public ObservableCollection<TKCustomMapPin> locationsSearch;

        public TKCustomMapPin myPin = new TKCustomMapPin();
        public TKCustomMapPin MyPin
        {
            get { return myPin; }
            set
            {
                if (this.myPin != value)
                {

                    this.myPin = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MyPin"));
                }
            }
        }

        public TKRoute MyRoute { get; set; }
        public ObservableCollection<ListRequest> listlocation;
        public TKCustomMapPin myPosition;

        public ICommand PinCommand;

        public ICommand SearchCommand { get; private set; }


        private Command<object> tapCommand;


        public Command<object> TapCommand
        {
            get { return tapCommand; }
            set { tapCommand = value; }
        }

        private Command<object> tapCommand2;


        public Command<object> TapCommand2
        {
            get { return tapCommand2; }
            set { tapCommand2 = value;}

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

        public string mySearch="";
        public string MySearch
        {
            get { return mySearch; }
            set
            {
                if (this.mySearch != value)
                {

                    this.mySearch = value;
                    SearchClient(value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MySearch"));
                }
            }
        }

        public MapSpan centerSearch = null;
        public MapSpan CenterSearch
        {
            get { return centerSearch; }
            set
            {
                if (this.centerSearch != value)
                {

                    this.centerSearch = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CenterSearch"));
                }
            }
        }

        public bool isSearch = false;
        public bool IsSearch
        {
            set
            {
                if (isSearch != value)
                {
                    isSearch = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSearch"));
                }
            }
            get { return isSearch; }
        }

        public bool isList = false;
        public bool IsList
        {
            set
            {
                if (isList != value)
                {
                    isList = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsList"));
                }
            }
            get { return isList; }
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

            LocationsSearch = new ObservableCollection<TKCustomMapPin>();
            locationsSearch = new ObservableCollection<TKCustomMapPin>();

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
            tapCommand2 = new Command<object>(PinClient);

            SearchCommand = new Command<string>(async (text) => SearchClient(text));


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
                LocationsSearch.Clear();
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

        public ObservableCollection<TKCustomMapPin> LocationsSearch
        {
            protected set
            {
                locationsSearch= LocationsSearch;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LocationsSearch"));

            }
            get { return locationsSearch; }
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
            });


            var miposicion = new Helpers.GeoUtils.Position
            {
                latitude = e.Position.Latitude,
                longitude = e.Position.Longitude,
            };


            if (positionAux == null)
            {
                positionAux = miposicion;
                await apiService.LogRuta(new LogRutaVendedor
                {
                    IdVendedor = Settings.userId,
                    Latitud = e.Position.Latitude,
                    Longitud = e.Position.Longitude,
                    Fecha = DateTime.Now
                });
            }
            if (!(Helpers.GeoUtils.GeoUtils.EstaCercaDeMi(positionAux, miposicion, distancia)))
            {
                await apiService.LogRuta(new LogRutaVendedor
                {
                    IdVendedor = Settings.userId,
                    Latitud = miposicion.latitude,
                    Longitud = miposicion.longitude,
                    Fecha = DateTime.Now
                });
                positionAux = miposicion;
            }
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

        private async void PinClient(object obj)
        {
            TKCustomMapPin cliente = (TKCustomMapPin)obj;
            IsSearch = false;
            
            MoveTo(cliente.Position);

            MyPin=locations.Where(x => x.Title == cliente.Title).FirstOrDefault();
           
            //MyPin, location 

            // await App.Navigator.PushAsync(new ClientePage(cliente));
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


        private async Task MoveTo(Xamarin.Forms.Maps.Position posicion)
        {
            //await Task.Delay(3000);
            CenterSearch = (MapSpan.FromCenterAndRadius((posicion), Distance.FromMiles(.3)));
            // await Task.Delay(3000);
        }






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






        public async void SearchClient(string text)
        {
            IsSearch = false;

            LocationsSearch.Clear();
            locationsSearch.Clear();
            if(text==""|| text==null)
            {
                IsSearch = false;
            }
            else
            {
                var a = Locations.Where(p => p.Title.ToLower().Contains(text.ToLower()));
                if (a != null)
                {


                    foreach (var item in a)
                    {
                        LocationsSearch.Add(item);
                        //  await MoveTo(item.Position);
                    }
                    IsSearch = true;
                }
            }
            
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
            EncabezadoMenu.Agente = Agente;
            EncabezadoMenu.AgenteFoto = AgenteFoto.Replace("~", Constants.Constants.VentasWS);
        }
        #endregion
    }
}
