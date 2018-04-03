using AppDemo.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AppDemo.Pages
{
    public class ClientProfilePage : ContentPage
    {
        public DotButtonsLayout dotLayout;
        public CarouselView carousel;

        public class Details
        {
            public int position { get; set; }
            public string Header { get; set; }
            public string Content { get; set; }
            public ObservableCollection<values> obscollection { get; set; }
        }
        public class values
        {
            public string value { get; set; }
        }
        public ClientProfilePage(ListRequest cliente)
        {
            //la primera lista 
            ObservableCollection<values> Productos = new ObservableCollection<values>
            {
                new values{value="producto 1"},
                new values{value="producto 2"},
                new values{value="producto 3"},
                new values{value="producto 4"},
                new values{value="producto 5"},
                new values{value="producto 6"}
        };

            ObservableCollection<values> Gustos = new ObservableCollection<values>
            {
                new values{value="Gusto 1"},
            new values{value="Gusto 2"},
            new values{value="Gusto 3"},
                new values{value="Gusto 4"},
            new values{value="Gusto 5"},
            new values{value="Gusto 6"}
        };

            ObservableCollection<values> compromiso = new ObservableCollection<values>
            {
                new values{value="compromiso 1"},
            new values{value="compromiso 2"},
            new values{value="compromiso 3"},
                new values{value="compromiso 4"},
            new values{value="compromiso 6"},
            new values{value="compromiso 6"}
        };

            ObservableCollection<Details> collection = new ObservableCollection<Details>{
                new Details{ obscollection = Productos, Header="productos ", Content="" },
                new Details{obscollection = Gustos, Header="Gustos", Content="" },
                new Details{obscollection = compromiso, Header="compromiso", Content=""},
        };

            BackgroundColor = Color.FromHex("#FFFFFF");

            StackLayout body = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            StackLayout header = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start
            };

            Image profilePhoto = new Image
            {
                WidthRequest = 100,
                HeightRequest = 100,
                Source = "ic_Profile.png"
            };
            header.Children.Add(profilePhoto);

            StackLayout dataProfile = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start
            };

            Label NombreLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                TextColor = Color.Black,
                Text = "Nombre"
            };
            Label ApellidoLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                TextColor = Color.Black,
                Text = "Apellido"
            };
            Label CILabel = new Label()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                TextColor = Color.Black,
                Text = "CI"
            };
            Label TelfLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                TextColor = Color.Black,
                Text = "Telf"
            };
            Label MailLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                TextColor = Color.Black,
                Text = "e-mail"
            };
            dataProfile.Children.Add(NombreLabel);
            dataProfile.Children.Add(ApellidoLabel);
            dataProfile.Children.Add(CILabel);
            dataProfile.Children.Add(TelfLabel);
            dataProfile.Children.Add(MailLabel);



            header.Children.Add(dataProfile);


            carousel = new CarouselView()
            {
                BackgroundColor = Color.Transparent,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };


            DataTemplate template = new DataTemplate(() =>
            {

                var stacksample = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.White
                };

                ListView lstview = new ListView()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    SeparatorVisibility = SeparatorVisibility.Default,
                    ItemTemplate = new DataTemplate((typeof(cell))),
                    BackgroundColor = Color.White
                };
                lstview.SetBinding(ListView.ItemsSourceProperty, "obscollection");

                lstview.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
                {
                    if (e.SelectedItem == null)
                    {
                        return;
                    }

                    var item = e.SelectedItem as values;

                    DisplayAlert("Item Selected", item.value, "Ok");

                    ((ListView)sender).SelectedItem = null;

                };


                stacksample.Children.Add(lstview);

                return stacksample;
            });

            carousel.ItemTemplate = template;

            carousel.PositionSelected += pageChanged;

            carousel.ItemsSource = collection;

            dotLayout = new DotButtonsLayout(collection.Count, Color.Red, 15);

            foreach (DotButton dot in dotLayout.dots)

                dot.Clicked += dotClicked;
            body.Children.Add(header);

            body.Children.Add(carousel);

            body.Children.Add(dotLayout);

            StackLayout stack = new StackLayout()
            {
                Children = { body },
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White
            };

            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

            Content = stack;

        }
        private void pageChanged(object sender, SelectedPositionChangedEventArgs e)
        {
            var position = (int)(e.SelectedPosition);
            for (int i = 0; i < dotLayout.dots.Length; i++)
                if (position == i)
                {
                    dotLayout.dots[i].Opacity = 1;
                }
                else
                {
                    dotLayout.dots[i].Opacity = 0.2;
                }
        }

        private void dotClicked(object sender)
        {
            var button = (DotButton)sender;
            int index = button.index;
            carousel.Position = index;
        }
    }

    public class DotButtonsLayout : StackLayout
    {
        public DotButton[] dots;
        public DotButtonsLayout(int dotCount, Color dotColor, int dotSize)
        {

            dots = new DotButton[dotCount];

            Orientation = StackOrientation.Horizontal;
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.Center;
            Spacing = 20;

            for (int i = 0; i < dotCount; i++)
            {
                dots[i] = new DotButton
                {
                    HeightRequest = dotSize,
                    WidthRequest = dotSize,
                    BackgroundColor = dotColor,
                    Opacity = 0.2
                };
                dots[i].index = i;
                dots[i].layout = this;
                Children.Add(dots[i]);
            }
            dots[0].Opacity = 1;
        }
    }


    public class DotButton : BoxView
    {
        public int index;
        public DotButtonsLayout layout;
        public event ClickHandler Clicked;
        public delegate void ClickHandler(DotButton sender);
        public DotButton()
        {
            var clickCheck = new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    if (Clicked != null)
                    {
                        Clicked(this);
                    }
                })
            };
            GestureRecognizers.Add(clickCheck);
        }
    }

    public class cell : ViewCell
    {
        public cell()
        {
            Label TitleLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Start,
                TextColor = Color.Black,
                Text="Titulo"
                
            };

            Label headerLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.Black
            };
            headerLabel.SetBinding(Label.TextProperty, "value");

            StackLayout stack = new StackLayout()
            {
                Children = { headerLabel },
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.White
            };

            View = stack;

        }

    }
}