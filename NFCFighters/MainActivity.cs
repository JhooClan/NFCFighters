using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;

namespace NFCFighters
{
	[Activity (Label = "NFCFighters", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int countP = 0;

		protected override void OnCreate (Bundle savedInstanceState)
		{
            base.OnCreate (savedInstanceState);
			this.RequestWindowFeature(WindowFeatures.NoTitle);
            Settings settings = Settings.LoadSettings();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var surfaceOrientation = WindowManager.DefaultDisplay.Rotation;

            if (settings.isLeftHanded && !(surfaceOrientation == SurfaceOrientation.Rotation0 || 
                surfaceOrientation == SurfaceOrientation.Rotation180))
            {
                LinearLayout main = FindViewById<LinearLayout>(Resource.Id.mainLayout);
                LinearLayout ll1 = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
                LinearLayout ll2 = FindViewById<LinearLayout>(Resource.Id.linearLayout2);

                main.RemoveAllViews();
                main.AddView(ll2);
                main.AddView(ll1);
            }

            if (settings.isNightmode)
            {
                LinearLayout main = FindViewById<LinearLayout>(Resource.Id.mainLayout);
                main.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backgr_land_day));
            }


			// Get our button from the layout resource,
			// and attach an event to it
			Button bPlay = FindViewById<Button>(Resource.Id.buttonPlay);
			Button bOptions = FindViewById<Button>(Resource.Id.buttonOptions);
			Button bExit = FindViewById<Button>(Resource.Id.buttonExit);


            switch (settings.isColorConfig.ToString())
            {
                case "color1":
                    bPlay.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backg_button1));
                    bOptions.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backg_button1));
                    bExit.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backg_button1));
                    break;
                case "color2":
                    bPlay.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backg_button2));
                    bOptions.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backg_button2));
                    bExit.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backg_button2));
                    break;
                case "color3":
                    bPlay.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backg_button3));
                    bOptions.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backg_button3));
                    bExit.SetBackgroundDrawable(GetDrawable(Resource.Drawable.backg_button3));
                    break;
            }

            bPlay.Click += delegate
			{
				countP++;
				bPlay.Text = Resources.GetQuantityString(Resource.Plurals.numberOfClicks, countP, countP);
			};

			bOptions.Click += delegate
            {
                FinishActivity(Resource.Layout.Main);
                var intent = new Intent(this, typeof(SettingsActivity));
				StartActivity(intent);
			};

			bExit.Click += delegate
			{
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle(Resources.GetString(Resource.String.exit));
                alert.SetMessage(Resources.GetString(Resource.String.doexit));
                alert.SetPositiveButton(Resource.String.yes, (senderAlert, args) =>
                {
                    System.Environment.Exit(0);
                });
                alert.SetNegativeButton(Resource.String.no, (senderAlert, args) =>
                {
                    
                });

                Dialog dialog = alert.Create();
                dialog.Show();                
			};
		}        
    }
}
