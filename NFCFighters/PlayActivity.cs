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
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Util;

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
                main.SetBackgroundResource(Resource.Drawable.backgr_land_day);
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
            int countSP = 0, countMP = 0;
            bSP.Click += delegate
            {
                countSP++;
                bSP.Text = Resources.GetQuantityString(Resource.Plurals.numberOfClicks, countSP, countSP);
            };

            bMP.Click += delegate
            {
                countMP++;
                bMP.Text = Resources.GetQuantityString(Resource.Plurals.numberOfClicks, countMP, countMP);
            };

            bBack.Click += delegate
            {
                //var intent = new Intent(this, typeof(SettingsActivity));
                //StartActivity(intent);
                Finish();
            };
        }
    }
}