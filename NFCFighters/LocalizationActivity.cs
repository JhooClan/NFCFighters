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
using System.Threading.Tasks;

namespace NFCFighters
{
    [Activity(Label = "Localization")]
    public class LocalizationActivity : Activity, ILocationListener
    {
        Location _currentLocation;
        LocationManager _locationManager;
        string _locationProvider;
        TextView lat, lon, dir;

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

            lat = FindViewById<TextView>(Resource.Id.textLat);
            lon = FindViewById<TextView>(Resource.Id.textLon);
            dir = FindViewById<TextView>(Resource.Id.textDir);
            Button fbt = FindViewById<Button>(Resource.Id.fightBt);
            Button map = FindViewById<Button>(Resource.Id.MapBt);

            fbt.Click += delegate
            {
                var intent = new Intent(this, typeof(FightActivity));
                StartActivity(intent);
            };

            map.Click += delegate
            {
                /*var geoUri = Android.Net.Uri.Parse("geo:38.986242,-1.897228?z=20");
                var mapIntent = new Intent(Intent.ActionView, geoUri);*/
                var mapIntent = new Intent(this, typeof(MapActivity));
                StartActivity(mapIntent);
            };

            InitializeLocationManager();

        }
        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
            //Log.Debug(TAG, "Using " + _locationProvider + ".");
        }

        public async void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            if (_currentLocation == null)
            {
                lat.Text = "Imposible determinar tu ubicacion";
            }
            else
            {
                lat.Text = string.Format("{0:f6}", _currentLocation.Latitude);
                lon.Text = string.Format("{0:f6}", _currentLocation.Longitude);
                try
                {
                    Address address = await ReverseGeocodeCurrentLocation();
                    DisplayAddress(address);
                }
                catch
                {

                }

            }
        }

        public void OnProviderDisabled(string provider)
        {
            Toast.MakeText(this, "GPS DESACTIVADO", ToastLength.Short).Show();
        }

        public void OnProviderEnabled(string provider)
        {
            Toast.MakeText(this, "GPS ACTIVADO", ToastLength.Short).Show();
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras) { }

        protected override void OnResume()
        {
            base.OnResume();
            _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }

        async Task<Address> ReverseGeocodeCurrentLocation()
        {
            Geocoder geocoder = new Geocoder(this);
            IList<Address> addressList =
                await geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);

            Address address = addressList.FirstOrDefault();
            return address;
        }

        void DisplayAddress(Address address)
        {
            if (address != null)
            {
                StringBuilder deviceAddress = new StringBuilder();
                for (int i = 0; i < address.MaxAddressLineIndex; i++)
                {
                    deviceAddress.Append(address.GetAddressLine(i) + "\n");
                }
                // Remove the last comma from the end of the address.
                dir.Text = deviceAddress.ToString();
            }
            else
            {
                dir.Text = "Imposible Determinar tu Dirección";
            }
        }
    }

}