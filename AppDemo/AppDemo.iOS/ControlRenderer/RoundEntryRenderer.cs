using System;
using System.Collections.Generic;
using System.Text;

[assembly: ExportRenderer(typeof(RoundEntry), typeof(RoundEntryRenderer))]
namespace AppDemo.iOS.ControlRenderer
{
    class RoundEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Layer.CornerRadius = 15;
            }
        }
    }
}
