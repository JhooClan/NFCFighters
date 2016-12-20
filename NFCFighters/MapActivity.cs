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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.Util;

namespace NFCFighters
{
    [Activity(Label = "MapActivity")]
    public class MapActivity : Activity, ILocationListener, IOnMapReadyCallback
    {
        Location _currentLocation;
        LocationManager _locationManager;
        string _locationProvider;
        GoogleMap _map;
        GroundOverlay _myOverlay;
        GroundOverlay _enemyOverlay;
        double lat, lon;
        int bHeight;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Map);

            // Create your application here

            var surfaceOrientation = WindowManager.DefaultDisplay.Rotation;

            DisplayMetrics metrics = Resources.DisplayMetrics;
            bHeight = metrics.HeightPixels;

            if (surfaceOrientation == SurfaceOrientation.Rotation0 || surfaceOrientation == SurfaceOrientation.Rotation180)
            {
                bHeight = bHeight / 2;
            }
            bHeight = bHeight / 5;

            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);

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
            if (_currentLocation != null)
            {
                bool jump = Math.Abs(_currentLocation.Latitude - lat) > 1 || Math.Abs(_currentLocation.Longitude - lon) > 1;
                lat = _currentLocation.Latitude;
                lon = _currentLocation.Longitude;
                
                LatLng loc = new LatLng(lat, lon);

                CameraUpdate cameraUpdate = CameraUpdateFactory.NewLatLngZoom(loc, 18);
                
                if (_map != null)
                {
                    if (jump)
                    {
                        _map.MoveCamera(cameraUpdate);
                    }
                    else
                    {
                        _map.AnimateCamera(cameraUpdate);
                    }
                }

                if (_myOverlay != null)
                {
                    _myOverlay.Position = loc;
                }
                else
                {
                    BitmapDescriptor image = BitmapDescriptorFactory.FromResource(Resource.Drawable.gherkin);
                    GroundOverlayOptions groundOverlayOptions = new GroundOverlayOptions()
                    .Position(loc, bHeight/4, bHeight/4)
                    .InvokeImage(image);
                    _myOverlay = _map.AddGroundOverlay(groundOverlayOptions);
                }

                if (_enemyOverlay == null)
                {
                    BitmapDescriptor image = BitmapDescriptorFactory.FromResource(Resource.Drawable.question);
                    GroundOverlayOptions groundOverlayOptions = new GroundOverlayOptions()
                    .Position(new LatLng(lat + 0.000001, lon + 0.000001), bHeight / 4, bHeight / 4)
                    .InvokeImage(image);
                    _enemyOverlay = _map.AddGroundOverlay(groundOverlayOptions);
                    _enemyOverlay.Clickable = true;
                }
            }
        }

        private void OnGroundOverlayClick(object sender, GoogleMap.GroundOverlayClickEventArgs e)
        {
            GroundOverlay gover = e.GroundOverlay;
            if (gover.Id.Equals(_enemyOverlay.Id)) // The ID of a specific marker the user clicked on.
            {
                Toast.MakeText(this, "Moyanita pretenciosa!", ToastLength.Short).Show();
                var intent = new Intent(this, typeof(FightActivity));
                intent.PutExtra("Enemy", "Moyanita");
                StartActivity(intent);
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

        public void OnMapReady(GoogleMap googleMap)
        {
            if (_map == null)
            {
                _map = googleMap;
                _map.MapType = GoogleMap.MapTypeTerrain;
                _map.UiSettings.ZoomControlsEnabled = true;
                _map.UiSettings.CompassEnabled = true;
                _map.GroundOverlayClick += OnGroundOverlayClick;
            }
        }
    }
}