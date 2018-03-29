using AppDemo.ViewModels;
using SignaturePad.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppDemo.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddClientPage : ContentPage
    {
        AddViewModel viewmodel;

        public AddClientPage()
        {
            InitializeComponent();

            viewmodel = new AddViewModel();
            BindingContext = viewmodel;
            viewmodel.tiposdecliente();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {

            Stream image = await PadView.GetImageStreamAsync(SignatureImageFormat.Png);

            viewmodel.submit(image);


            // you can add code here to save the Stream image.
        }
    }
}