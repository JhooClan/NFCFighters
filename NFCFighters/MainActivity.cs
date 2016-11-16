using Android.App;
using Android.Widget;
using Android.OS;

namespace NFCFighters
{
	[Activity (Label = "NFCFighters", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		int count = 0;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button bPlay = FindViewById<Button>(Resource.Id.buttonPlay);
			Button bOptions = FindViewById<Button>(Resource.Id.buttonOptions);
			Button bExit = FindViewById<Button>(Resource.Id.buttonExit);
			
			bPlay.Click += delegate {
				count++;
				bPlay.Text = Resources.GetQuantityString(Resource.Plurals.numberOfClicks, count, count);

				//button.Text = string.Format ("{0} clicks!", count++);
			};
		}
	}
}


