using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Media;

namespace NFCFighters.Services
{
    [Service]
    [IntentFilter(new[] { Initialize, ActionStop, ActionResume, ActionPause, MenuTheme, BossTheme })]
    class MusicSoundService : Service
    {
        public const string Initialize = "INIT";
        public const string ActionStop = "STOP";
        public const string ActionResume = "RESUME";
        public const string ActionPause = "PAUSE";
        public const string MenuTheme = "MENUTHEME";
        public const string BossTheme = "BOSSTHEME";
        MediaPlayer _player;
        int currentPlaying;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            switch (intent.Action)
            {
                case Initialize:
                    _player = new MediaPlayer();
                    currentPlaying = -1;
                    break;
                case ActionStop:
                    Stop();
                    break;
                case ActionResume:
                    Resume();
                    break;
                case ActionPause:
                    Pause();
                    break;
                case MenuTheme:
                    Play(Resource.Raw.menu);
                    break;
                case BossTheme:
                    Play(Resource.Raw.boss);
                    break;
            }
            return StartCommandResult.RedeliverIntent;
        }

        public void Play(int resId)
        {
            if (currentPlaying != resId)
            {
                _player.Stop();
                _player.Reset();
                _player.SetAudioStreamType(Stream.Music);
                _player.SetDataSource(this, Android.Net.Uri.Parse("android.resource://" + this.PackageName + "/" + resId));
                _player.Looping = true;
                currentPlaying = resId;
                _player.Prepare();
            }
            _player.Start();
        }

        public void Resume()
        {
            _player.Start();
        }

        public void Pause()
        {
            _player.Pause();
        }

        public void Stop()
        {
            _player.Stop();
            _player.Reset();
            currentPlaying = -1;
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