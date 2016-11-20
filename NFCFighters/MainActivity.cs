using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;
using Android.Util;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Support.V4.App;
using System.Threading;
using Android.Content.Res;
using System.Threading.Tasks;

namespace NFCFighters
{
	[Activity (Label = "NFCFighters", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
            base.OnCreate (savedInstanceState);
			this.RequestWindowFeature(WindowFeatures.NoTitle);
            Settings settings = Settings.LoadSettings();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

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


			// Get our button from the layout resource,
			// and attach an event to it
			Button bPlay = FindViewById<Button>(Resource.Id.buttonPlay);
			Button bSettings = FindViewById<Button>(Resource.Id.buttonSettings);
			Button bExit = FindViewById<Button>(Resource.Id.buttonExit);

            Drawable dPlay = GetDrawable(Resource.Drawable.play);
            Bitmap bmPlay = ((BitmapDrawable)dPlay).Bitmap;
            bmPlay = Bitmap.CreateScaledBitmap(bmPlay, bHeight, bHeight, false);
            dPlay = new BitmapDrawable(this.Resources, bmPlay);
            bPlay.SetCompoundDrawablesWithIntrinsicBounds(dPlay, null, null, null);
            
            Drawable dSet = GetDrawable(Resource.Drawable.settings);
            Bitmap bmSet = ((BitmapDrawable)dSet).Bitmap;
            bmSet = Bitmap.CreateScaledBitmap(bmSet, bHeight, bHeight, false);
            dSet = new BitmapDrawable(this.Resources, bmSet);
            bSettings.SetCompoundDrawablesWithIntrinsicBounds(dSet, null, null, null);
            
            Drawable dEx = GetDrawable(Resource.Drawable.exit);
            Bitmap bmEx = ((BitmapDrawable)dEx).Bitmap;
            bmEx = Bitmap.CreateScaledBitmap(bmEx, bHeight, bHeight, false);
            dEx = new BitmapDrawable(this.Resources, bmEx);
            bExit.SetCompoundDrawablesWithIntrinsicBounds(dEx, null, null, null);

            switch (settings.colorConfig)
            {
                case Color.COLOR_GREEN:
                    bPlay.SetBackgroundResource(Resource.Drawable.backg_button1);
                    bSettings.SetBackgroundResource(Resource.Drawable.backg_button1);
                    bExit.SetBackgroundResource(Resource.Drawable.backg_button1);
                    break;
                case Color.COLOR_RED:
                    bPlay.SetBackgroundResource(Resource.Drawable.backg_button2);
                    bSettings.SetBackgroundResource(Resource.Drawable.backg_button2);
                    bExit.SetBackgroundResource(Resource.Drawable.backg_button2);
                    break;
                case Color.COLOR_BLUE:
                    bPlay.SetBackgroundResource(Resource.Drawable.backg_button3);
                    bSettings.SetBackgroundResource(Resource.Drawable.backg_button3);
                    bExit.SetBackgroundResource(Resource.Drawable.backg_button3);
                    break;
            }

            bPlay.Click += delegate
            {
                var intent = new Intent(this, typeof(PlayActivity));
                StartActivity(intent);
            };

            bSettings.Click += delegate
            {
                Finish();
                var intent = new Intent(this, typeof(SettingsActivity));
				StartActivity(intent);
			};

			bExit.Click += delegate
			{
                Exit();
			};

            NotificationThread nt = new NotificationThread(this, Resources.GetStringArray(Resource.Array.notificationTitle), Resources.GetStringArray(Resource.Array.notificationContent));
            ThreadStart myThreadDelegate = new ThreadStart(nt.StartNotifications);
            Thread myThread = new Thread(myThreadDelegate);

            if (settings.notifications && !myThread.IsAlive)
            {
                myThread.Start();
            }
            else if (!settings.notifications && myThread.IsAlive)
            {
                myThread.Abort();
            }
        }

        private void Exit()
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
        }

        public override void OnBackPressed()
        {
            Exit();
        }
    }

    class NotificationThread
    {
        private Context context;
        private const int ButtonClickNotificationId = 1000;
        private string[] nTitle;
        private string[] nContent;
        private volatile bool cont;

        public NotificationThread(Context context, string[] nTitle, string[] nContent)
        {
            this.context = context;
            this.nTitle = nTitle;
            this.nContent = nContent;
        }
        private void ShowNotification(int id)
        {
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context)
            .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
            .SetContentTitle(nTitle[id])      // Set its title
            .SetContentText(nContent[id]) // The message to display.
            .SetSmallIcon(Resource.Mipmap.Icon);

            // Finally, publish the notification:
            NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.Notify(ButtonClickNotificationId, builder.Build());
        }
        public async void StartNotifications()
        {
            int count = 0;
            cont = true;
            while (cont)
            {
                foreach (string s in nTitle)
                {
                    ShowNotification(count);
                    await Task.Delay(10000);
                    count++;
                }
                count = 0;
            }
        }

        public void StopNotifications()
        {
            cont = false;
        }
    }
}
