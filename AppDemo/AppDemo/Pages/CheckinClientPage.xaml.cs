using AppDemo.Classes;
using AppDemo.Models;
using AppDemo.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SignaturePad.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;

namespace AppDemo.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckinClientPage : ContentPage
	{
	
        CheckinViewModel viewmodel;
        public CheckinClientPage(ListRequest clienteseleccionado)
        {
           // Debug.WriteLine(clienteseleccionado.Titulo);


            InitializeComponent();
            viewmodel = new CheckinViewModel(clienteseleccionado);
            BindingContext = viewmodel;
        }
        private async void SaveButton_Clicked(object sender, EventArgs e)
        {

            var a = starFive.Rating;
            Stream image = await PadView.GetImageStreamAsync(SignatureImageFormat.Png);
            viewmodel.submit(image, a);
            // .
        }

        private async void CheckBox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            CheckBox item = sender as CheckBox;
            if (e.Value)
            {
                int idCompromiso = int.Parse(item.ClassId);
                PopupPage page = new SolucionesPage(idCompromiso);
                await PopupNavigation.PushAsync(page);
            }
        }

    }
}