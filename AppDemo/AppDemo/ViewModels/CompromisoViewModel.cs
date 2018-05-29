using AppDemo.Models;
using AppDemo.Services;
using GalaSoft.MvvmLight.Command;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppDemo.ViewModels
{
    public class CompromisoViewModel : INotifyPropertyChanged
    {
        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public int id;
        public string solucion;
        public string Solucion
        {
            set
            {
                if (solucion != value)
                {
                    solucion = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Solucion"));
                }
            }
            get
            {
                return solucion;
            }
        }
        #endregion

        public CompromisoViewModel(int id)
        {
            solucion = "";
            this.id = id;
            apiService = new ApiService();
            navigationService = new NavigationService();
            dialogService = new DialogService();
        }

        public ICommand UpdateCommand { get { return new RelayCommand(Update); } }
        public async void Update()
        {
            var compromiso = new Compromiso
            {
                IdCompromiso = id,
                Solucion = Solucion,
            };
           var response=  await apiService.ActualizarCompromiso(compromiso);
            if (response.IsSuccess)
            {
                await dialogService.ShowMessage("Actualización", "Actualizado correctamente");

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
