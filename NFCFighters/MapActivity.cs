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

namespace NFCFighters
{
    [Activity(Label = "MapActivity")]
    public class MapActivity : Activity
    {
        MapFragment mapFragment;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.Map);

            // Create your application here

            LatLng location = new LatLng(38.995759, -1.265046);
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(18);
            builder.Bearing(155);
            builder.Tilt(65);
            CameraPosition cameraPosition = builder.Build();
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            GoogleMap mapa = mapFragment.Map;
            if (mapa != null)
            {
                mapa.MapType = GoogleMap.MapTypeTerrain;
                mapa.UiSettings.ZoomControlsEnabled = true;
                mapa.UiSettings.CompassEnabled = true;
                mapa.MoveCamera(cameraUpdate);

            }
        }
    }
}