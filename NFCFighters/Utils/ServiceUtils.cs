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
using static Android.App.ActivityManager;

namespace NFCFighters.Utils
{
    class ServiceUtils
    {
        public static bool IsMyServiceRunning(Context ctx, string serviceName)
        {
            ActivityManager manager = (ActivityManager)ctx.GetSystemService(Context.ActivityService);
            return manager.GetRunningServices(int.MaxValue).Select(service => service.Service.ClassName).Any(service => service == serviceName);
        }
    }
}