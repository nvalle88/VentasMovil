using AppDemo.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDemo.Models;
using AppDemo.Helpers;
using AppDemo.Classes;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace AppDemo.ViewModels
{
   public class AddDateViewModel: INotifyPropertyChanged
    {
        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        #region Properties

        public DateTime fecha;
        public DateTime Fecha
        {
            set
            {
                if (fecha != value)
                {
                    fecha = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Fecha"));
                }
            }
            get
            {
                return fecha;
            }

        }

        public List<PrioridadRequest> prioridad;
        public List<PrioridadRequest> Prioridad
        {
            set
            {
                if (prioridad != value)
                {
                    prioridad = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Prioridad"));
                }
            }
            get
            {
                return prioridad;
            }

        }

        public List<ClienteRequest> cliente;
        public List<ClienteRequest> Cliente
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

        public String nota;
        public String Nota
        {
            set
            {
                if (nota != value)
                {
                    nota = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Nota"));
                }
            }
            get
            {
                return nota;
            }

        }

        ClienteRequest clienteSelect;

        public ClienteRequest clienteSelectItem
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

        PrioridadRequest prioridadSelect;

        public PrioridadRequest prioridadSelectItem
        {
            get
            {
                return prioridadSelect;
            }
            set
            {
                // marcaseleccionada = ;

                prioridadSelect = value;
            }
        }

        #endregion
        #region Constructor
        public AddDateViewModel(DateTime? _fecha)
                {

            clienteSelect = new ClienteRequest();
            prioridadSelect = new PrioridadRequest();
            fecha = new DateTime();
                    prioridad = new List<PrioridadRequest>();
                    cliente = new List<ClienteRequest>();
            
                    Prioridad.Add( new PrioridadRequest { id = 0, Prioridad = "Baja" });
                    Prioridad.Add(new PrioridadRequest { id = 1, Prioridad = "Media" });
                    Prioridad.Add(new PrioridadRequest { id = 2, Prioridad = "Alta" });
                    nota = "";
                    apiService = new ApiService();
                    dialogService = new DialogService();
                    navigationService = new NavigationService();
                    Init();
                }

        #endregion
                private async void Init()
                {

                    Cliente = await apiService.GetMyClient();
                }
        /// <summary>
        /// 0 Bajo
        /// 1 Medio
        /// 2 Alto
        /// </summary>
        /// <returns></returns>
        public ICommand AgregarAgendaCommand { get { return new RelayCommand(AgregarAgenda); } }

        public async void AgregarAgenda()
        {
            var Agenda = new Agenda
            {
                FechaVista=Fecha,
                FechaFin=Fecha,
                idCliente=clienteSelectItem.IdCliente,
                IdVendedor=Settings.userId,
                Notas=Nota,
                Prioridad= prioridadSelectItem.id,
                
            };
            var a = await  apiService.AgregarAgenda(Agenda);
            if (a.IsSuccess)
            {
                MessagingCenter.Send<App>((App)Application.Current, "OnDateCreated");
                await dialogService.ShowMessage("Agenda", "Se agendo correctamente");
                await PopupNavigation.PopAllAsync();

            }
        }

        public ICommand CloseCommand { get { return new RelayCommand(Close); } }
        public async void Close()
        {
        

             await PopupNavigation.PopAllAsync();
        }

    }
}
