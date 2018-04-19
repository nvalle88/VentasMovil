using AppDemo.Classes;
using AppDemo.Models;
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
    public class PasswordViewModel: INotifyPropertyChanged
    {
        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public RecuperarContrasenaRequest datos { get; set; }
        public RecuperarContrasenaRequest Datos
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

        #region constructor
        public PasswordViewModel()
        {
            datos = new RecuperarContrasenaRequest();
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();           
        }
        #endregion

        #region Command
        public ICommand VerifyCommand { get { return new RelayCommand(Verify); } }

        private async void Verify()
        {
            var coderesult = await apiService.VerificarCodigo(Datos);
            if (coderesult.IsSuccess)
            { App.Current.MainPage = new NavigationPage(new PasswordPage()); }
            else
            {
                dialogService.ShowMessage("Error", "El Código o el correo son erroneos");
            }
        }
        public ICommand ChangeCommand { get { return new RelayCommand(Change); } }

        private async void Change()
        {
            var coderesult = await apiService.PasswordChange(Datos);
            if (coderesult.IsSuccess)
            {
                dialogService.ShowMessage("Cambio ", coderesult.Message);

                App.Current.MainPage = new NavigationPage(new LoginPage()); }
            else
            {
                dialogService.ShowMessage("Error", coderesult.Message);
            }
        }
        #endregion

    }
}
