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

            int countMP = 0;

            Intent bss = new Intent(ApplicationContext, typeof(FXSoundService));
            bss.SetAction(FXSoundService.VolumeSound);
            StartService(bss);
            bss.SetAction(FXSoundService.ButtonSound);

            Button bClick = FindViewById<Button>(Resource.Id.clickBt);

            bClick.Click += delegate
            {
                StartService(bss);
                countMP++;
                bClick.Text = Resources.GetQuantityString(Resource.Plurals.numberOfClicks, countMP, countMP);
                if (countMP == 85)
                {
                    Intent mss = new Intent(ApplicationContext, typeof(MusicSoundService));
                    mss.SetAction(MusicSoundService.BossTheme);
                    StartService(mss);
                }
            };
        }
    }
}