using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;

using NFCFighters.Utils;
using NFCFighters.Services;
using NFCFighters.Models;

namespace NFCFighters
{
    [Activity(Label = "Localization")]
    public class Localization : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            Settings settings = Settings.LoadSettings();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Localization);
            // Create your application here
            var surfaceOrientation = WindowManager.DefaultDisplay.Rotation;

            if (settings.nightmode)
            {
                LinearLayout main = FindViewById<LinearLayout>(Resource.Id.mainLayout);
                if (surfaceOrientation == SurfaceOrientation.Rotation0 || surfaceOrientation == SurfaceOrientation.Rotation180)
                {
                    main.SetBackgroundResource(Resource.Drawable.backgr_night_port);
                }
                else
                {
                    main.SetBackgroundResource(Resource.Drawable.backgr_night_land);
                }
            }

            LocationManager locationManager = (LocationManager) GetSystemService(Context.LocationService);
            Criteria criteria = new Criteria();

            Location location = locationManager.GetLastKnownLocation(locationManager.GetBestProvider(criteria, false));
            double latitude = location.Latitude;
            double longitud = location.Longitude;

            TextView lat = FindViewById<TextView>(Resource.Id.textLat);
            lat.Text = latitude.ToString();

            TextView lon = FindViewById<TextView>(Resource.Id.textLon);
            lon.Text = longitud.ToString();

            Geocoder geocoder = new Geocoder(this);
            IList<Address> direc = geocoder.GetFromLocation(latitude, longitud, 1);

            
            List<Address> direc2 = direc.ToList<Address>();
            TextView dir = FindViewById<TextView>(Resource.Id.textDir);
            dir.Text = direc[0].ToString(); //address.IndexOf("addressLines").ToString();


            //Toast.MakeText(this, latitude.ToString(), ToastLength.Short).Show();

        }
    }
        
}