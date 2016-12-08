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
using Android.Util;
using Android.Media;
using System.Threading.Tasks;

namespace NFCFighters.Services
{
    [Service]
    [IntentFilter(new[] { VolumeSound, ButtonSound })]
    class FXSoundService : Service
    {
        public const string VolumeSound = "SFXVOLUME";
        public const string ButtonSound = "BUTTONSFX";

        private float volume;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            switch (intent.Action)
            {
                case VolumeSound:
                    volume = Settings.LoadSettings().sounds;
                    break;
                case ButtonSound:
                    Task.Factory.StartNew(() => Play(Resource.Raw.button));
                    break;
            }
            return StartCommandResult.NotSticky;
        }

        public void Play(int resId)
        {
            MediaPlayer _player = MediaPlayer.Create(this, resId);
            _player.SetVolume(volume, volume);
            _player.Start();
            while (_player.IsPlaying) { }
            _player.Reset();
            _player.Release();
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
    }
}