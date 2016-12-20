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
using Android.Graphics;
using Camera = Android.Hardware.Camera;
using NFCFighters.Services;

namespace NFCFighters
{
    [Activity(Label = "FightActivity")]
    class FightActivity : Activity, TextureView.ISurfaceTextureListener
    {
        Camera _camera;
        TextureView _textureView;

        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            _camera = Camera.Open();

            _textureView.LayoutParameters =
                  new FrameLayout.LayoutParams(width, height);

            try
            {
                _camera.SetPreviewTexture(surface);
                _camera.SetDisplayOrientation(90);
                _camera.StartPreview();
            }
            catch (Java.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
        {
            _camera.StopPreview();
            _camera.Release();

            return true;
        }

        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            _textureView.LayoutParameters =
                   new FrameLayout.LayoutParams(width, height);

            try
            {
                _camera.SetPreviewTexture(surface);
                _camera.SetDisplayOrientation(90);
                _camera.StartPreview();
            }
            catch (Java.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
            try
            {
                _camera.SetPreviewTexture(surface);
                _camera.SetDisplayOrientation(90);
                _camera.StartPreview();
            }
            catch (Java.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.RequestWindowFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.Fight);

            _textureView = FindViewById<TextureView>(Resource.Id.textureView1);
            _textureView.SurfaceTextureListener = this;

            string enemy = Intent.GetStringExtra("Enemy") ?? "Enemy not available";

            int countMP = 0;

            Intent bss = new Intent(ApplicationContext, typeof(FXSoundService));
            bss.SetAction(FXSoundService.VolumeSound);
            StartService(bss);
            bss.SetAction(FXSoundService.ButtonSound);

            ImageView imgP = FindViewById<ImageView>(Resource.Id.playerImg);
            //bClick.Text = enemy;
            imgP.SetImageResource(Resource.Drawable.gherkin_back);
            imgP.Clickable = true;

            ImageView imgV = FindViewById<ImageView>(Resource.Id.enemyImg);
            switch (enemy) {
                case "Moyanita":
                    imgV.SetImageResource(Resource.Drawable.moyanita);
                    break;
            }

            ImageView eLife = FindViewById<ImageView>(Resource.Id.enemyLife);
            eLife.SetImageResource(Resource.Drawable.life100);

            imgP.Click += delegate
            {
                StartService(bss);
                countMP++;
                //bClick.Text = Resources.GetQuantityString(Resource.Plurals.numberOfClicks, countMP, countMP);
                if (countMP == 25)
                {
                    eLife.SetImageResource(Resource.Drawable.life75);
                }
                if (countMP == 50)
                {
                    eLife.SetImageResource(Resource.Drawable.life50);
                }
                if (countMP == 75)
                {
                    eLife.SetImageResource(Resource.Drawable.life25);
                }
                if (countMP == 85)
                {
                    Intent mss = new Intent(ApplicationContext, typeof(MusicSoundService));
                    mss.SetAction(MusicSoundService.BossTheme);
                    StartService(mss);
                }
                if (countMP == 100)
                {
                    Finish();
                }
            };
        }
    }
}