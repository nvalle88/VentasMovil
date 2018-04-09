using AppDemo.Classes;
using AppDemo.Pages;
using AppDemo.Services;
using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamForms.Controls;

namespace AppDemo.ViewModels
{
    public class AgendaViewModel : INotifyPropertyChanged
    {
        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        private List<EventoRequest> agendaData;

        public ObservableCollection<AgendaList> listaAgenda;
        public ObservableCollection<AgendaList> ListaAgenda
        {
            set
            {
                if (listaAgenda != value)
                {
                    listaAgenda = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("ListaAgenda"));
                }
            }
            get
            {
                return listaAgenda;
            }

        }


        public ObservableCollection<SpecialDate> fechas;
        public ObservableCollection<SpecialDate> Fechas
        {
            set
            {
                if (fechas != value)
                {
                    fechas = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Fechas"));
                }
            }
            get
            {
                return fechas;
            }

        }
       
        private DateTime? _date;
        public DateTime? Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Date"));
                //
                ListaAgenda.Clear();
                var a = (_date as DateTime?);
                var d = agendaData.Where(x => x.FechaVista.Value.Date == a.Value.Date).ToList();
                foreach (var item in d)
                {
                    if (item.Notas != null && item.Notas != "")
                    {
                        string color = "";
                        switch (item.Prioridad)
                        {
                            case 0:
                                color = "#00b300";
                                break;
                            case 1:
                                color = "#ff471a";
                                break;
                            case 2:
                                color = "#cc0000";
                                break;
                        }
                        ListaAgenda.Add(new AgendaList

                        {
                            Color = color,

                            Titulo = item.Notas
                        });

                    }
                    else
                    {
                        ListaAgenda.Add(new AgendaList

                        {
                            Color="#FFFFFF",
                            Titulo = item.Descripcion,
                            Subtitulo = item.Solucion,

                        });
                    }




                }

            }
        }



        public  AgendaViewModel()
        {           
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();
            fechas = new ObservableCollection<SpecialDate>();
            agendaData = new List<EventoRequest>();
            listaAgenda = new ObservableCollection<AgendaList>();
            init();
        }

        public async Task init()
        {
            try
            {
                agendaData = await apiService.AgendaPorVendedor();
            Fechas.Clear();
            foreach (var item in agendaData)
            {               
                    SpecialDate fec = new SpecialDate(item.FechaVista.Value)
                    {
                        BackgroundColor = Color.Orange,
                        TextColor = Color.White,
                        Selectable = true,                       
                    };
                    Fechas.Add(fec);
            }

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public ICommand DateChosen
        {
            get
            {
                return new Command((obj) => {
                 
                });
            }
        }

        public ICommand AddCommand { get { return new RelayCommand(Add); } }
        public async void Add()
        {
            Debug.WriteLine(Date);
            PopupPage page = new AddDatePage(Date);
            await PopupNavigation.PushAsync(page);
        }


    }
}
