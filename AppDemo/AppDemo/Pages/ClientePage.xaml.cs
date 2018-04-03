using AppDemo.Classes;
using AppDemo.ViewModels;
using SkiaSharp;
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
    }
}