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
		int countO = 0;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			this.RequestWindowFeature(WindowFeatures.NoTitle);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);
			//orientationListener = new MyOrientationListener(this);

			// Get our button from the layout resource,
			// and attach an event to it
			Button bPlay = FindViewById<Button>(Resource.Id.buttonPlay);
			Button bOptions = FindViewById<Button>(Resource.Id.buttonOptions);
			Button bExit = FindViewById<Button>(Resource.Id.buttonExit);
			
			bPlay.Click += delegate
			{
				countP++;
				bPlay.Text = Resources.GetQuantityString(Resource.Plurals.numberOfClicks, countP, countP);
			};

			bOptions.Click += delegate
			{
				countO++;
				bOptions.Text = Resources.GetQuantityString(Resource.Plurals.numberOfClicks, countO, countO);
			};

			bExit.Click += delegate
			{
				System.Environment.Exit(0);
			};
		}
	}
}
