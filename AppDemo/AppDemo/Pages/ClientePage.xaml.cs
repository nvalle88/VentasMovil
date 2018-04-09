using AppDemo.Classes;
using AppDemo.Models;
using AppDemo.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XLabs.Forms.Controls;

namespace AppDemo.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientePage : TabbedPage
    {
        ClienteViewModel viewmodel;

        //List<Microcharts.Entry> entries = new List<Microcharts.Entry>
        //{
        //    new Microcharts.Entry(200)
        //    {
        //        Color=SKColor.Parse("#FF1493"),
        //        Label="Vistas",
        //        ValueLabel="200",

        //    },
        //     new Microcharts.Entry(100)
        //    {
        //        Color=SKColor.Parse("#00BFFF"),
        //        Label="Compras",
        //        ValueLabel="100",

        //    }
        //};
        public ClientePage(ListRequest cliente)
        {

            InitializeComponent();
            viewmodel = new ClienteViewModel( cliente);
            BindingContext = viewmodel;

           // chart1.Chart = new Microcharts.PointChart { Entries=entries};
           

        }

        private async void CheckBox_CheckedChanged(object sender, XLabs.EventArgs<bool> e)
        {
            CheckBox item = sender as CheckBox;

            if (e.Value)
            {
                int idCompromiso=int.Parse(item.ClassId);
                PopupPage page = new SolucionesPage(idCompromiso);
                await PopupNavigation.PushAsync(page);
            }

        }

      
    }
}