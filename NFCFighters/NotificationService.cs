using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Support.V4.App;
using System.Threading.Tasks;

namespace NFCFighters
{
    [Service]
    class NotificationService : Service
    {
        private Context context;
        private const int ButtonClickNotificationId = 1000;
        private string[] nTitle;
        private string[] nContent;
        private volatile bool cont;
        private Task notiTask;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Debug("NotificationService", "NotificationService started");
            context = Application.Context;
            nTitle = Resources.GetStringArray(Resource.Array.notificationTitle);
            nContent = Resources.GetStringArray(Resource.Array.notificationContent);
            cont = true;
            notiTask = new Task(() => StartNotifications());
            notiTask.Start();
            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            cont = false;
            notiTask.Wait();
            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
        
        private void ShowNotification(int id)
        {
            NotificationCompat.Builder builder = new NotificationCompat.Builder(context)
            .SetAutoCancel(true)                    // Dismiss from the notif. area when clicked
            .SetContentTitle(nTitle[id])      // Set its title
            .SetContentText(nContent[id]) // The message to display.
            .SetSmallIcon(Resource.Mipmap.Icon);

            // Finally, publish the notification:
            NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
            notificationManager.Notify(ButtonClickNotificationId, builder.Build());
        }
        public async void StartNotifications()
        {
            while(cont)
            {
                int count = 0;
                foreach (string s in nTitle)
                {
                    if(cont == false)
                    {
                        break;
                    }
                    ShowNotification(count);
                    await Task.Delay(10000);
                    count++;
                }
            }
        }
    }
}