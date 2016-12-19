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
using Android.Nfc.Tech;

using NFCFighters.Models;

namespace NFCFighters
{
    [Activity(Label = "ReadNFCActivity")]
    class ReadNFCActivity : Activity
    {
        private NfcAdapter nfcAdapter;
        private TextView info;
        private const int MESSAGE_SENT = 1;
        private PendingIntent mNfcPendingIntent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            Settings settings = Settings.LoadSettings();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ReadNFC);
            // Create your application here
            var surfaceOrientation = WindowManager.DefaultDisplay.Rotation;

            info = FindViewById<TextView>(Resource.Id.nfcInfo);

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
                mNfcPendingIntent = PendingIntent.GetActivity(this,
                        0, new Intent(this, typeof(ReadNFCActivity))
                                .AddFlags(ActivityFlags.SingleTop), 0);
            }

        }

        [Android.Runtime.Register("onNewIntent", "(Landroid/content/Intent;)V", "GetOnNewIntent_Landroid_content_Intent_Handler")]
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            Toast.MakeText(this, "Yeeee", ToastLength.Short).Show();
            var tag = (Tag)intent.GetParcelableExtra(NfcAdapter.ExtraTag);

            string[] techList = tag.GetTechList();
            info.Text = "";
            foreach (string tech in techList)
            {
                info.Text += "\n\t" + tech;
            }

            //if (tag.GetTechList().Any(s => s == ""))
            //{

            //}

            var kosovo = NfcBarcodeType.Kovio;

            NfcA nfca = NfcA.Get(tag);
            NdefFormatable ndeff = NdefFormatable.Get(tag);
            IsoDep iso = IsoDep.Get(tag);

            try
            {
                nfca.Connect();
                short s = nfca.Sak;
                byte[] a = nfca.GetAtqa();
                string atqa = Encoding.ASCII.GetString(a);
                info.Text += "\nSAK = " + s + "\nATQA = " + atqa;
                nfca.Close();
            } catch (Exception e)
            {
                Toast.MakeText(this, e.Message, ToastLength.Short).Show();
            }

            try
            {
                iso.Connect();

                iso.Close();
            }
            catch (Exception e)
            {
                Toast.MakeText(this, e.Message, ToastLength.Short).Show();
            }

            try
            {
                ndeff.Connect();
                info.Text += "\nType: " + ndeff.GetType();
                info.Text += "\nConnected: " + ndeff.IsConnected;
                var rawMsgs = intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
                if (rawMsgs == null)
                {
                    Toast.MakeText(this, "/dev/null", ToastLength.Short).Show();
                    return;
                }
                List<NdefMessage> msgs = new List<NdefMessage>();
                foreach (IParcelable rawMsg in rawMsgs)
                {
                    msgs.Add(rawMsg as NdefMessage);
                }
                List<NdefRecord> records = new List<NdefRecord>();

                foreach (NdefMessage msg in msgs)
                {
                    records.Concat(msg.GetRecords());
                }
                
                foreach (NdefRecord record in records)
                {
                    info.Text += "\n\t" + Encoding.ASCII.GetString(record.GetPayload());
                }
                ndeff.Close();
            }
            catch (Exception e)
            {
                Toast.MakeText(this, e.Message, ToastLength.Short).Show();
            }

            /*
            if (ndefMessage == null)
            {
                info.Text = "The tag is empty !";
                return;
            }

            /*
            var rawMsgs = intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
            if (rawMsgs == null) {
                Toast.MakeText(this, "/dev/null", ToastLength.Short).Show();
                return;
            }
            List<NdefMessage> msgs = new List<NdefMessage>();
            foreach (IParcelable rawMsg in rawMsgs)
            {
                msgs.Add(rawMsg as NdefMessage);
            }
            List<NdefRecord> records = new List<NdefRecord>();
            NdefRecord[] records = ndefMessage.GetRecords();

            //foreach (NdefMessage msg in msgs)
            //{
            //    records.Concat(msg.GetRecords());
            //}

            List<string> strings = new List<string>();
            foreach (NdefRecord record in records)
            {
                info.Text += "\n\t" + Encoding.ASCII.GetString(record.GetPayload());
            }
            */
        }

        protected override void OnResume()
        {            
            base.OnResume();
            nfcAdapter.EnableForegroundDispatch(this, mNfcPendingIntent, null, null);
        }

        protected override void OnPause()
        {
            base.OnPause();
            nfcAdapter.DisableForegroundDispatch(this);
        }
    }
}