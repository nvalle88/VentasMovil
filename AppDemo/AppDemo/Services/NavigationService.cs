using AppDemo.Pages;
using AppDemo.ViewModels;
using System.Threading.Tasks;
/// <summary>
/// En esta clase se encuentran los metodos para navegar dentro de la aplicación de una pagina a otra
/// </summary>
namespace AppDemo.Services
{
    public class NavigationService
    {
        public void NavigateBack() => App.Navigator.PopToRootAsync();

        public async Task Navigate(string pageName)
        {
            App.Master.IsPresented = false;
            switch (pageName)
            {
                //case "VerificarAutoPage":
                //    await App.Navigator.PushAsync(new VerificarAutoPage(), true);
                //    break;

                //case "ConsultarMultas":
                //    await App.Navigator.PushAsync(new ConsultarAutoPage());
                //    break;
                //case "PonerMulta":
                //    await App.Navigator.PushAsync(new PonerMultaPage(), true);
                //    break;

                //case "PasswordPage":
                //    await App.Navigator.PushAsync(new PasswordPage());
                //    break;

                case "SettingPage":
                    
                    await App.Navigator.PushAsync(new SettingPage());
                    break;

                case "PasswordPage":
                    
                    await App.Navigator.PushAsync(new PasswordPage());
                    break;

                case "CodePage":
                   await App.Navigator.PushAsync(new CodePage());
                    break;

                case "CheckinClientePage":
                    await App.Navigator.PushAsync(new CheckinClientPage(null));
                    break;

                case "AddClientePage":
                    await App.Navigator.PushAsync(new AddClientPage());
                    break;

                case "AgendaPage":
                    await App.Navigator.PushAsync(new AgendaPage());
                    break;

                case "ListClientPage":
                    await App.Navigator.PushAsync(new ListClientPage());
                    break;

                case "MainPage":
                    await App.Navigator.PopToRootAsync();
                    break;

                default: break;
            }
        }

        internal void SetMainPage(VendedorViewModel vendedorActual)
        {

            var main = MainViewModel.GetInstance();
            App.VendedorActual = vendedorActual;
         //   main.LoadClientes();
            
            App.Current.MainPage = new MasterPage();


        }

        public VendedorViewModel GetAgenteActual() => App.VendedorActual;
    }
}