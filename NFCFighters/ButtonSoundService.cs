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

namespace NFCFighters
{
    [Service]
    class ButtonSoundService : Service
    {
        MediaPlayer _player;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Toast.MakeText(ApplicationContext, Resource.String.app_name, ToastLength.Short);
            _player = MediaPlayer.Create(this, Resource.Raw.button);
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