using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content;

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
            CheckBox nightmode = FindViewById<CheckBox>(Resource.Id.nightmode);
            RadioGroup color = FindViewById<RadioGroup>(Resource.Id.color);
            cbLeftHanded.Checked = settings.isLeftHanded;
            nightmode.Checked = settings.isNightmode;

            switch (settings.isColorConfig.ToString())
            {
                case "color1":
                    break;
                case "color2":
                    break;
                case "color3":
                    break;
            }

            Button bSave = FindViewById<Button>(Resource.Id.bSetSave);

			bSave.Click += delegate
			{
				Save();
			};
		}

		void Save()
		{
			CheckBox cbLeftHanded = FindViewById<CheckBox>(Resource.Id.cbSetLeftHanded);
            CheckBox nightmode = FindViewById<CheckBox>(Resource.Id.nightmode);
            RadioGroup color = FindViewById<RadioGroup>(Resource.Id.color);
            RadioButton color1 = FindViewById<RadioButton>(Resource.Id.color1);
            RadioButton color2 = FindViewById<RadioButton>(Resource.Id.color2);
            RadioButton color3 = FindViewById<RadioButton>(Resource.Id.color3);
            
            Settings settings = Settings.LoadSettings();
            settings.isLeftHanded = cbLeftHanded.Checked;
            settings.isNightmode = nightmode.Checked;
            if (color1.Checked)
                settings.isColorConfig = "color1";
            if (color2.Checked)
                settings.isColorConfig = "color2";
            if (color3.Checked)
                settings.isColorConfig = "color3";

            Settings.SaveSettings(settings);
            
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
			alert.SetTitle(Resources.GetString(Resource.String.savesuccess));
			alert.SetMessage(Resources.GetString(Resource.String.configok));
			alert.SetNeutralButton(Resource.String.ok, (senderAlert, args) =>
			{
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            });

			Dialog dialog = alert.Create();
			dialog.Show();
		}
        
    }
}
