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
            CheckBox cbLeftHanded = FindViewById<CheckBox>(Resource.Id.cbInvert);
            CheckBox nightmode = FindViewById<CheckBox>(Resource.Id.nightmode);
            RadioGroup color = FindViewById<RadioGroup>(Resource.Id.color);
            cbLeftHanded.Checked = settings.invertControls;
            nightmode.Checked = settings.nightmode;

            switch (settings.colorConfig)
            {
                case Color.COLOR_RED:
                    RadioButton color2 = FindViewById<RadioButton>(Resource.Id.color2);
                    color2.Checked = true;
                    break;
                case Color.COLOR_BLUE:
                    RadioButton color3 = FindViewById<RadioButton>(Resource.Id.color3);
                    color3.Checked = true;
                    break;
                default:
                    RadioButton color1 = FindViewById<RadioButton>(Resource.Id.color1);
                    color1.Checked = true;
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
			CheckBox cbLeftHanded = FindViewById<CheckBox>(Resource.Id.cbInvert);
            CheckBox nightmode = FindViewById<CheckBox>(Resource.Id.nightmode);
            RadioGroup color = FindViewById<RadioGroup>(Resource.Id.color);
            RadioButton color1 = FindViewById<RadioButton>(Resource.Id.color1);
            RadioButton color2 = FindViewById<RadioButton>(Resource.Id.color2);
            RadioButton color3 = FindViewById<RadioButton>(Resource.Id.color3);
            
            Settings settings = Settings.LoadSettings();
            settings.invertControls = cbLeftHanded.Checked;
            settings.nightmode = nightmode.Checked;
            if (color1.Checked)
                settings.colorConfig = Color.COLOR_GREEN;
            else if (color2.Checked)
                settings.colorConfig = Color.COLOR_RED;
            else if (color3.Checked)
                settings.colorConfig = Color.COLOR_BLUE;

            Settings.SaveSettings(settings);
            
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
			alert.SetTitle(Resources.GetString(Resource.String.savesuccess));
			alert.SetMessage(Resources.GetString(Resource.String.configok));
			alert.SetNeutralButton(Resource.String.ok, (senderAlert, args) =>
			{
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
            });

			Dialog dialog = alert.Create();
			dialog.Show();
		}

        public override void OnBackPressed()
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
    }
}
