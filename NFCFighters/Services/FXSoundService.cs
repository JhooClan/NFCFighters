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
    [IntentFilter(new[] { ButtonSound })]
    class FXSoundService : Service
    {
        public const string ButtonSound = "BUTTONSFX";
        MediaPlayer _player;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            int resId = 0;
            switch (intent.Action)
            {
                case ButtonSound:
                    resId = Resource.Raw.button;
                    break;
            }
            _player = MediaPlayer.Create(this, resId);
            _player.Start();
            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }

        public override void OnDestroy()
        {
            while (_player.IsPlaying) { }
            _player.Release();
            base.OnDestroy();
        }
    }
}