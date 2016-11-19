using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace NFCFighters
{
	[Activity(Label = "Settings", ScreenOrientation = ScreenOrientation.Portrait)]
	public class SettingsActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            Settings settings = Settings.LoadSettings();

            SetContentView(Resource.Layout.Settings);
            CheckBox cbLeftHanded = FindViewById<CheckBox>(Resource.Id.cbSetLeftHanded);
            //CheckBox cbColorBlind = FindViewById<CheckBox>(Resource.Id.cbSetColorBlind);
            cbLeftHanded.Checked = settings.isLeftHanded;
            //cbColorBlind.Checked = settings.isColorBlind;

            Button bSave = FindViewById<Button>(Resource.Id.bSetSave);

			bSave.Click += delegate
			{
				Save();
			};
		}

		void Save()
		{
			CheckBox cbLeftHanded = FindViewById<CheckBox>(Resource.Id.cbSetLeftHanded);
			//CheckBox cbColorBlind = FindViewById<CheckBox>(Resource.Id.cbSetColorBlind);

            Settings settings = Settings.LoadSettings();
            settings.isLeftHanded = cbLeftHanded.Checked;
            //settings.isColorBlind = cbColorBlind.Checked;
            Settings.SaveSettings(settings);

            AlertDialog.Builder alert = new AlertDialog.Builder(this);
			alert.SetTitle(Resources.GetString(Resource.String.savesuccess));
			alert.SetMessage(Resources.GetString(Resource.String.configok));
			alert.SetNeutralButton(Resource.String.ok, (senderAlert, args) =>
			{
				
			});

			Dialog dialog = alert.Create();
			dialog.Show();
		}
	}
}
