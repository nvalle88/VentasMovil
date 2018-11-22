using AppDemo.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppDemo.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PagoPage : ContentPage
    {
		public PagoPage()
		{

            var model = new PagoViewModel();
            model.CargarTiposPago();
            BindingContext = model;
            InitializeComponent ();
            picker.SelectedIndex = 0;
            pickerFactura.SelectedIndex = 0;
            pickerFactura.SelectedIndexChanged += this.OnPickerSelectedIndexChanged;

            Deuda.Text = "225.66";

            Deuda.TextChanged += Deuda_TextChanged;
            MyLabel.Text = "225.66";
            Valor.Text = "0.00";
            Valor.TextChanged += Valor_TextChanged;

        }

        private void Deuda_TextChanged(object sender, TextChangedEventArgs e)
        {
            MyLabel.Text = Convert.ToString(Convert.ToDecimal(Deuda.Text));
        }

        private void Valor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Valor.Text) || !string.IsNullOrEmpty(Valor.Text))
            {
                MyLabel.Text = Convert.ToString(Convert.ToDecimal(Deuda.Text) - Convert.ToDecimal(Valor.Text));
            }
            else
            {
                MyLabel.Text = Convert.ToString(Convert.ToDecimal(Deuda.Text));
            }
           
        }

        public void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            switch (selectedIndex)
            {
                case 0:
                    Deuda.Text = "658.66";
                    Valor.Text = "0.00";
                    break;
                case 1:
                    Deuda.Text = "76.44";
                    Valor.Text = "0.00";
                    break;
                case 2:
                    Deuda.Text = "55.49";
                    Valor.Text = "0.00";
                    break;
                case 3:
                    Deuda.Text = "1225.12";
                    Valor.Text = "0.00";
                    break;
                default:
                    break;
            }
           

            if (selectedIndex == -1)
            {
               App.Current.MainPage.DisplayAlert("Mensaje", "No Hay facturas para mostrar", "Aceptar");
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Valor.Text) || string.IsNullOrWhiteSpace(Valor.Text) || Convert.ToDecimal(Valor.Text)==0)
            {
                await App.Current.MainPage.DisplayAlert("Error", "El pago no se ha podido realizar, El valor a cobrar debe se mayor que 0 ", "Aceptar");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Información", "El pago se ha realizado satisfactoriamente,   Valor Cobrado:    " + Valor.Text + "   Saldo Actual:   " + MyLabel.Text, "Aceptar");
                await App.Navigator.PopAsync();
            }
          
            // you can add code here to save the Stream image.
        }
    }
}