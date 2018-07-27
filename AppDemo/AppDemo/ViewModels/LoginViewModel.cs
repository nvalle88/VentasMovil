using GalaSoft.MvvmLight.Command;
using AppDemo.Models;
using AppDemo.Pages;
using AppDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using AppDemo.Helpers;
using AppDemo.Classes;
using Xamarin.Forms;
/// <summary>
/// View Model para para la vista de login 
/// Guarda los datos del usuario en las variables almacenadas en el local del telefono 
/// </summary>
namespace AppDemo.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Attributes
        public bool isRunning;
        #endregion
        #region Properties


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



        public string Usuario { get; set; }

        public string Contrasena { get; set; }

        public bool Recuerdame { get; set; }
        #endregion
        #region Services
        private NavigationService navigationService;
        private DialogService dialogService;
        private ApiService apiService;


        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        #region Constructors
        public LoginViewModel()
        {
            navigationService = new NavigationService();
            dialogService = new DialogService();
            apiService = new ApiService();

            Recuerdame = true;
        }

        #endregion
        #region Commands

        public ICommand NavigateRegisterCommand { get { return new RelayCommand(Navigate); } }

        private void Navigate()
        {
           
        }
        public ICommand RegisterCommand { get { return new RelayCommand(Register); } }

        private async void Register()
        {
            App.Current.MainPage = new NavigationPage(new CodePage());
        }

        public ICommand LoginCommand { get { return new RelayCommand(Login); } }
        private async void Login()
        {
            IsRunning = true;
            if (string.IsNullOrEmpty(Usuario))
            {
                await dialogService.ShowMessage("Error", "Debe ingresar el nombre de Usuario");
                return;
            }

            if (string.IsNullOrEmpty(Contrasena))
            {
                await dialogService.ShowMessage("Error", "Debe ingresar la Contraseña");
                return;
            }

            var user = new LoginRequest()
            {
                userid = Usuario,
                password = Contrasena,
            };
            var response = await apiService.Login(user);
            if (response.IsSuccess)
            {
                var vendedor = (VendedorRequest)response.Result;

                var vendedorView = new VendedorViewModel
                {
                    Nombres = vendedor.Nombres,
                    IdVendedor = vendedor.IdVendedor,
                    TiempoSeguimiento=vendedor.TiempoSeguimiento,
                    IdUsuario= vendedor.IdUsuario,
                    Foto= vendedor.Foto                    
                };

                Settings.userId = vendedor.IdVendedor;
                Settings.UserName = vendedor.Nombres;
                Settings.UserPhoto = vendedor.Foto;
                Settings.companyId = (int)vendedor.idEmpresa;
                Settings.IsLoggedIn = true;

                var main = MainViewModel.GetInstance();
                main.LoadMenu(vendedorView.Nombres, vendedor.Foto);
              //  main.LoadClientes();
              //  main.Agenda = new AgendaViewModel();
             //   main.Agenda.init();



                navigationService.SetMainPage(vendedorView);

                IsRunning = false;
                return;
            }

            await dialogService.ShowMessage("Error", "Usuario o contraseña incorrectos");
            IsRunning = false;
        }

        #endregion
    }
}
