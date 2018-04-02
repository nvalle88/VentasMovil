using AppDemo.Models;
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
	public partial class CheckinClientPage : ContentPage
	{
	
        CheckinViewModel viewmodel;
        public CheckinClientPage()
        {
            InitializeComponent();
            viewmodel = new CheckinViewModel();
            BindingContext = viewmodel;
        }
        private async void SaveButton_Clicked(object sender, EventArgs e)
        {

            var a = starFive.Rating;
            Stream image = await PadView.GetImageStreamAsync(SignatureImageFormat.Png);
            viewmodel.submit(image, a);
            // .
        }

    }
}