
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;

namespace NFCFighters
{
	[Activity(Label = "Settings")]
	public class SettingsActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Settings);


			Button bSave = FindViewById<Button>(Resource.Id.bSetSave);

			bSave.Click += delegate
			{
				Save();
			};
		}

		void Save()
		{
			CheckBox cbLeftHanded = FindViewById<CheckBox>(Resource.Id.cbSetLeftHanded);
			CheckBox cbColorBlind = FindViewById<CheckBox>(Resource.Id.cbSetColorBlind);

			var settings = new Setting(cbLeftHanded.Checked, cbColorBlind.Checked);
			var json = JsonConvert.SerializeObject(settings);

			string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			string filePath = Path.Combine(path, "settings.json");

			using (var file = File.Open(filePath, FileMode.Create, FileAccess.Write))
			using (var strm = new StreamWriter(file))
			{
				strm.Write(json);
			}

			AlertDialog.Builder alert = new AlertDialog.Builder(this);
			alert.SetTitle(Resources.GetString(Resource.String.savesuccess));
			alert.SetMessage(Resources.GetString(Resource.String.restart));
			alert.SetNeutralButton(Resource.String.ok, (senderAlert, args) =>
			{
				Toast.MakeText(this, "", ToastLength.Short).Show();
			});

			Dialog dialog = alert.Create();
			dialog.Show();
		}
	}
}
