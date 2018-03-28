
using Xamarin.Forms;

using AppDemo.Controls;
using AppDemo.Droid.ControlRenderer;

using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(RoundEntry), typeof(RoundEntryRenderer))]
namespace AppDemo.Droid.ControlRenderer
{
    
        public class RoundEntryRenderer : EntryRenderer
        {
            protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
            {
                base.OnElementChanged(e);

                if (Control != null)
                {
                    GradientDrawable gd = new GradientDrawable();
                    // increase or decrease to changes the corner 
                    gd.SetCornerRadius(10);
                //Control.Background = gd;
#pragma warning disable CS0618 // Type or member is obsolete
                Control.Background = Resources.GetDrawable(Resource.Drawable.RoundedEntry);
#pragma warning restore CS0618 // Type or member is obsolete
                              //Control.SetBackgroundDrawable(gd);
            }
            }
        }
    
}