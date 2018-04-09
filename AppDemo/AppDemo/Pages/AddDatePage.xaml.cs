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
    public partial class AddDatePage : PopupPage
    {
        AddDateViewModel viewModel;
        public AddDatePage(DateTime? Fecha)
        {

            InitializeComponent();

            viewModel = new AddDateViewModel(Fecha);
            BindingContext = viewModel;
            if (Fecha != null)
            { 
            datePick.Date = new DateTime(Fecha.Value.Year, Fecha.Value.Month, Fecha.Value.Day);
            }
            else
            {
                datePick.Date = DateTime.Now.Date;

            }
        }
    }
}