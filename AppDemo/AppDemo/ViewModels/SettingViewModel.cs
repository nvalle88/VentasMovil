using AppDemo.Classes;
using AppDemo.Helpers;
using AppDemo.Pages;
using AppDemo.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppDemo.ViewModels
{

    public class SettingViewModel: INotifyPropertyChanged
    {
        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public Double valor { get; set; }
        public Double Valor
        {
            set
            {
                if (valor != value)
                {
                    valor = value;
                    sliderValue = String.Empty;
                    SliderValue = valor+" km";
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Valor"));
                }
            }
            get
            {
                return valor;
            }
        }
        public string sliderValue { get; set; }
        public string SliderValue
        {
            set
            {
                if (sliderValue != value)
                {
                    sliderValue = value;
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("SliderValue"));
                }
            }
            get
            {
                return sliderValue;
            }
        }

        #endregion

        #region constructor
        public SettingViewModel()
        {
            valor = new Double();
            valor =  Double.Parse(Settings.RadioValue);
            sliderValue = string.Empty;
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();
            SliderValue = string.Empty;
        }


        #endregion

        #region Command
        public ICommand SaveCommand { get { return new RelayCommand(Save); } }
        private async void Save()
        {
            Settings.RadioValue= Valor.ToString();
            await dialogService.ShowMessage("Configuración", "Cambio guardado correctamente");

            navigationService.NavigateBack();
        }

        
        public ICommand CancelCommand { get { return new RelayCommand(Cancel); } }
        private async void Cancel()
        {
           navigationService.NavigateBack();
        }
        #endregion
    }
}
