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

namespace NFCFighters.Services
{
    [Service]
    [IntentFilter(new[] { MenuSong, ActionStop })]
    class MusicSoundService : Service
    {
        public const string MenuSong = "BUTTONSFX";
        public const string ActionStop = "STOP";
        MediaPlayer _player = new MediaPlayer();

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            switch (intent.Action)
            {
                case MenuSong:
                    Stop();
                    Play(Resource.Raw.menu);
                    break;
                case ActionStop:
                    Stop();
                    break;
            }
            return StartCommandResult.RedeliverIntent;
        }

        public void Play(int resId)
        {
            _player = MediaPlayer.Create(this, resId);
            while (!_player.IsPlaying)
            {
                _player.Start();
            }
        }

        public void Stop()
        {
            _player.Stop();
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        public override void OnDestroy()
        {
            Stop();
            _player.Release();
            base.OnDestroy();
        }
    }
}