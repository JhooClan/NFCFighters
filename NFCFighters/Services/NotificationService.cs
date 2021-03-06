using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using System.Threading.Tasks;
using System.Threading;

namespace NFCFighters.Services
{
    [Service]
    class NotificationService : Service
    {
        private Context context;
        private const int NotificationId = 1000;
        private string[] nTitle;
        private string[] nContent;
        private Task notiTask;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Debug("NotificationService", "NotificationService started");
            context = Application.Context;
            nTitle = Resources.GetStringArray(Resource.Array.notificationTitle);
            nContent = Resources.GetStringArray(Resource.Array.notificationContent);
            notiTask = Task.Factory.StartNew(() => StartNotifications(tokenSource.Token), tokenSource.Token);
            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
        
        private void ShowNotification(int id)
        {
            Intent resultIntent = new Intent(this, typeof(MainActivity));

            TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);
            stackBuilder.AddParentStack(Java.Lang.Class.FromType(typeof(MainActivity)));
            stackBuilder.AddNextIntent(resultIntent);

            // Create the PendingIntent with the back stack:            
            PendingIntent resultPendingIntent =
                stackBuilder.GetPendingIntent(0, PendingIntentFlags.UpdateCurrent);

            Notification.Builder builder = new Notification.Builder(context)
            .SetAutoCancel(false)
            .SetContentIntent(resultPendingIntent)
            .SetSmallIcon(Resource.Mipmap.Icon)
            .SetContentTitle(nTitle[id])      // Set its title
            .SetContentText(nContent[id]) // The message to display.
            .SetStyle(new Notification.BigTextStyle()
                .BigText(nContent[id]));

            Notification notif = builder.Build();

            NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.Notify(NotificationId, notif);
        }

        public async void StartNotifications(CancellationToken ct)
        {
            bool cont = true;
            while(cont)
            {
                int count = 0;
                foreach (string s in nTitle)
                {
                    if(ct.IsCancellationRequested)
                    {
                        cont = false;
                        NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                        notificationManager.Cancel(NotificationId);
                        break;
                    }
                    ShowNotification(count);
                    await Task.Delay(25000);
                    count++;
                }
            }
        }
    }
}