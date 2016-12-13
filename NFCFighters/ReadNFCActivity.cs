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
using Android.Nfc;

using NFCFighters.Models;

namespace NFCFighters
{
    [Activity(Label = "ReadNFCActivity")]
    class ReadNFCActivity : Activity
    {
        private NfcAdapter nfcAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            Settings settings = Settings.LoadSettings();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ReadNFC);
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

            nfcAdapter = NfcAdapter.GetDefaultAdapter(this);
            if (nfcAdapter == null)
            {
                Toast.MakeText(this, "Tu dispositivo no soporta NFC", ToastLength.Short).Show();
            }

            if (!nfcAdapter.IsEnabled)
            {
                Toast.MakeText(this, "NFC Desconectado", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "NFC Conectado", ToastLength.Short).Show();
            }

        }
    }
}