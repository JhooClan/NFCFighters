using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Util;

using NFCFighters.Utils;
using NFCFighters.Services;
using NFCFighters.Models;

namespace NFCFighters
{
    [Activity(Label = "PlayActivity")]
    public class PlayActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            Settings settings = Settings.LoadSettings();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Play);
            // Create your application here

            var surfaceOrientation = WindowManager.DefaultDisplay.Rotation;

            DisplayMetrics metrics = Resources.DisplayMetrics;
            int bHeight = metrics.HeightPixels;

            if (surfaceOrientation == SurfaceOrientation.Rotation0 || surfaceOrientation == SurfaceOrientation.Rotation180)
            {
                bHeight = bHeight / 2;
            }
            bHeight = bHeight / 5;

            if (settings.invertControls && !(surfaceOrientation == SurfaceOrientation.Rotation0 ||
                surfaceOrientation == SurfaceOrientation.Rotation180))
            {
                LinearLayout main = FindViewById<LinearLayout>(Resource.Id.mainLayout);
                LinearLayout ll1 = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
                LinearLayout ll2 = FindViewById<LinearLayout>(Resource.Id.linearLayout2);

                main.RemoveAllViews();
                main.AddView(ll2);
                main.AddView(ll1);
            }

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

            Button bSP = FindViewById<Button>(Resource.Id.buttonSP);
            Drawable dSP = GetDrawable(Resource.Drawable.singleplayer);
            Bitmap bmSP = ((BitmapDrawable)dSP).Bitmap;
            bmSP = Bitmap.CreateScaledBitmap(bmSP, bHeight, bHeight, false);
            dSP = new BitmapDrawable(this.Resources, bmSP);
            bSP.SetCompoundDrawablesWithIntrinsicBounds(dSP, null, null, null);

            Button bMP = FindViewById<Button>(Resource.Id.buttonMP);
            Drawable dMP = GetDrawable(Resource.Drawable.multiplayer);
            Bitmap bmMP = ((BitmapDrawable)dMP).Bitmap;
            bmMP = Bitmap.CreateScaledBitmap(bmMP, bHeight, bHeight, false);
            dMP = new BitmapDrawable(this.Resources, bmMP);
            bMP.SetCompoundDrawablesWithIntrinsicBounds(dMP, null, null, null);

            Button bBack = FindViewById<Button>(Resource.Id.buttonBack);
            Drawable dBack = GetDrawable(Resource.Drawable.backarrow);
            Bitmap bmBack = ((BitmapDrawable)dBack).Bitmap;
            bmBack = Bitmap.CreateScaledBitmap(bmBack, bHeight, bHeight, false);
            dBack = new BitmapDrawable(this.Resources, bmBack);
            bBack.SetCompoundDrawablesWithIntrinsicBounds(dBack, null, null, null);

            switch (settings.colorConfig)
            {
                case Color.COLOR_GREEN:
                    bSP.SetBackgroundResource(Resource.Drawable.backg_button1);
                    bMP.SetBackgroundResource(Resource.Drawable.backg_button1);
                    bBack.SetBackgroundResource(Resource.Drawable.backg_button1);
                    break;
                case Color.COLOR_RED:
                    bSP.SetBackgroundResource(Resource.Drawable.backg_button2);
                    bMP.SetBackgroundResource(Resource.Drawable.backg_button2);
                    bBack.SetBackgroundResource(Resource.Drawable.backg_button2);
                    break;
                case Color.COLOR_BLUE:
                    bSP.SetBackgroundResource(Resource.Drawable.backg_button3);
                    bMP.SetBackgroundResource(Resource.Drawable.backg_button3);
                    bBack.SetBackgroundResource(Resource.Drawable.backg_button3);
                    break;
            }

            Intent bss = new Intent(ApplicationContext, typeof(FXSoundService));
            bss.SetAction(FXSoundService.VolumeSound);
            StartService(bss);
            bss.SetAction(FXSoundService.ButtonSound);

            bSP.Click += delegate
            {
                StartService(bss);
                var intent = new Intent(this, typeof(LocalizationActivity));
                StartActivity(intent);
            };

            bMP.Click += delegate
            {
                StartService(bss);
                var intent = new Intent(this, typeof(ReadNFCActivity));
                StartActivity(intent);
            };

            bBack.Click += delegate
            {
                StartService(bss);
                Finish();
            };
        }

        protected override void OnPause()
        {
            base.OnPause();
            Intent mss = new Intent(ApplicationContext, typeof(MusicSoundService));
            mss.SetAction(MusicSoundService.ActionPause);
            StartService(mss);
        }

        protected override void OnResume()
        {
            base.OnResume();
            Intent mss = new Intent(ApplicationContext, typeof(MusicSoundService));
            mss.SetAction(MusicSoundService.ActionResume);
            StartService(mss);
        }
    }
}