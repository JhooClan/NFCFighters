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

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Log.Debug("NotificationService", "NotificationService started");
            context = Application.Context;
            nTitle = Resources.GetStringArray(Resource.Array.notificationTitle);
            nContent = Resources.GetStringArray(Resource.Array.notificationContent);
            StartNotifications();
            return StartCommandResult.Sticky;
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
            int count = 0;
            foreach (string s in nTitle)
            {
                ShowNotification(count);
                await Task.Delay(10000);
                count++;
            }
        }
    }
}