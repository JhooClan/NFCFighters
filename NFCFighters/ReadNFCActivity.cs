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
using Java.Nio.Charset;

namespace NFCFighters
{
    [Activity(Label = "ReadNFCActivity")]
    class ReadNFCActivity : Activity
    {
        private NfcAdapter nfcAdapter;
        private TextView info;
        private const int MESSAGE_SENT = 1;
        private PendingIntent mNfcPendingIntent;
        private bool _inWriteMode = true;

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
            Toast.MakeText(this, "Leyendo NFC", ToastLength.Short).Show();
            var tag = intent.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;

            /*if (_inWriteMode)
            {
                _inWriteMode = false;

                if (tag == null)
                {
                    return;
                }

                // These next few lines will create a payload (consisting of a string)
                // and a mimetype. NFC record are arrays of bytes. 
                var payload = Encoding.ASCII.GetBytes("YeeSaurio");
                var mimeBytes = Encoding.ASCII.GetBytes("application/NFCFighters");
                var apeRecord = new NdefRecord(NdefRecord.TnfMimeMedia, mimeBytes, new byte[0], payload);
                var ndefMessage = new NdefMessage(new[] { apeRecord });

                if (!TryAndWriteToTag(tag, ndefMessage))
                {
                    // Maybe the write couldn't happen because the tag wasn't formatted?
                    TryAndFormatTagWithMessage(tag, ndefMessage);
                }
            }

            /*string[] techList = tag.GetTechList();
            info.Text = "";
            foreach (string tech in techList)
            {
                info.Text += "\n\t" + tech;
            }*/            

            NfcA nfca = NfcA.Get(tag);
            NdefFormatable ndeff = NdefFormatable.Get(tag);
            IsoDep iso = IsoDep.Get(tag);
            MifareUltralight mifU = MifareUltralight.Get(tag);
            

            /*try
            {
                nfca.Connect();
                short s = nfca.Sak;
                byte[] a = nfca.GetAtqa();
                string atqa = Encoding.ASCII.GetString(a);
                info.Text += "\nSAK = " + s + "\nATQA = " + atqa;
                nfca.Close();
            } catch (Exception e)
            {
                Toast.MakeText(this, "NfcA" + e.Message, ToastLength.Short).Show();
            }*/

            /*try
            {
                iso.Connect();

                iso.Close();
            }
            catch (Exception e)
            {
                Toast.MakeText(this, "IsoDep" + e.Message, ToastLength.Short).Show();
            }*/

            /*try
            {
                ndeff.Connect();
                //info.Text += "\nType: " + ndeff.GetType();
                //info.Text += "\nConnected: " + ndeff.IsConnected;
                var rawMsgs = intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
                var msg = (NdefMessage)rawMsgs[0];
                var record = msg.GetRecords()[0];
                
                info.Text += "\n\tNdefFormatted: " + Encoding.ASCII.GetString(record.GetPayload());
                ndeff.Close();
            }
            catch (Exception e)
            {
                Toast.MakeText(this, "NdefFormatted " + e.Message, ToastLength.Short).Show();
            }*/

            try
            {
                mifU.Connect();
                byte[] mPag = mifU.ReadPages(12);
                StringBuilder aux = new StringBuilder();
                String cont_mpag = "";
                for (int i=0; i < mPag.Length; i++)
                {
                    aux.Append(mPag[i]);
                }
                cont_mpag = aux.ToString();
                //info.Text = Charset.AvailableCharsets().ToString();
                //var mifM = new String(cont_mpag, Charset.ForName("US-ASCII"));
                string mifM = Encoding.ASCII.GetString(mPag);
                info.Text += "\nTu personaje es " + mifM;
                mifU.Close();
            }
            catch (Exception e)
            {
                //Toast.MakeText(this, "MifareUltralight" + e.Message, ToastLength.Short).Show();
            }
        }

        private bool TryAndWriteToTag(Tag tag, NdefMessage ndefMessage)
        {

            // This object is used to get information about the NFC tag as 
            // well as perform operations on it.
            var ndef = Ndef.Get(tag);
            if (ndef != null)
            {
                ndef.Connect();

                // Once written to, a tag can be marked as read-only - check for this.
                if (!ndef.IsWritable)
                {
                    Toast.MakeText(this, "Tag is read-only.", ToastLength.Short).Show();
                }

                // NFC tags can only store a small amount of data, this depends on the type of tag its.
                var size = ndefMessage.ToByteArray().Length;
                if (ndef.MaxSize < size)
                {
                    Toast.MakeText(this, "Tag doesn't have enough space.", ToastLength.Short).Show();
                }

                ndef.WriteNdefMessage(ndefMessage);
                //info.Text = ndefMessage.ToString();
                Toast.MakeText(this, "Succesfully wrote tag.", ToastLength.Short).Show();
                return true;
            }

            return false;
        }

        private bool TryAndFormatTagWithMessage(Tag tag, NdefMessage ndefMessage)
        {
            var format = NdefFormatable.Get(tag);
            if (format == null)
            {
                Toast.MakeText(this, "Tag does not appear to support NDEF format.", ToastLength.Short).Show();
            }
            else
            {
                try
                {
                    format.Connect();
                    format.Format(ndefMessage);
                    Toast.MakeText(this, "Tag successfully written.", ToastLength.Short).Show();
                    return true;
                }
                catch (Exception ioex)
                {
                    var msg = "There was an error trying to format the tag: ";
                    Toast.MakeText(this, msg + ioex, ToastLength.Short).Show();
                }
            }
            return false;
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