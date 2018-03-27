﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamForms.Controls;

namespace AppDemo.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AgendaPage : ContentPage
	{
		public AgendaPage ()
		{
			InitializeComponent ();
            calendar.StartDay = DayOfWeek.Monday;
            calendar.TitleLabel.Text = "Agenda";
            SpecialDate item = new SpecialDate(DateTime.Now.AddDays(3))
            {
                BackgroundColor = Color.Green,
                TextColor = Color.Blue,
                Selectable = true,
                BackgroundPattern = new BackgroundPattern(1)
                {
                    Pattern = new List<Pattern>
                            {
                              
                                new Pattern{ WidthPercent = 1f, HightPercent = 0.25f, Color = Color.Yellow,Text = "Reunión", TextColor=Color.DarkBlue, TextSize=11, TextAlign=TextAlign.Middle}
                            }
                }
            };

            calendar.SpecialDates.Add(item);

           

        }
	}
}