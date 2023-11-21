using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;

namespace HealthMate;

[Activity(Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize |
    ConfigChanges.Orientation |
    ConfigChanges.UiMode |
    ConfigChanges.ScreenLayout |
    ConfigChanges.SmallestScreenSize |
    ConfigChanges.Density,
    ScreenOrientation = ScreenOrientation.Portrait,
    ShowForAllUsers = true)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Handle the intent if the activity is launched from the background
        SetShowWhenLocked(true);
        SetTurnScreenOn(true);
    }
}

[BroadcastReceiver(Enabled = true, Exported = false)]
public class AlarmBroadcastReceiver : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        var fullScreenIntent = new Intent(context, typeof(MainActivity));
        fullScreenIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTop);
        var fullScreenPendingIntent = PendingIntent.GetActivity(context, 0, fullScreenIntent, PendingIntentFlags.Immutable);

        var mBuilder = new NotificationCompat.Builder(context, "69420")
            .SetContentTitle("Trainify")
            .SetAutoCancel(true)
            .SetContentTitle("A train has arrived!")
            .SetContentText("The train arrived at your chosen station")
            .SetPriority((int)NotificationPriority.Max)
            .SetVibrate(new long[0])
            .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
            .SetVisibility((int)NotificationVisibility.Public)
            .SetSmallIcon(Resource.Drawable.common_google_signin_btn_icon_dark)
            .SetShowWhen(true)
            .SetFullScreenIntent(fullScreenPendingIntent, true);

        var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var importance = global::Android.App.NotificationImportance.Max;
            var notificationChannel = new NotificationChannel("69420", "title", importance);
            notificationChannel.EnableLights(true);
            notificationChannel.EnableVibration(true);
            notificationChannel.SetShowBadge(true);
            notificationChannel.Importance = NotificationImportance.High;
            notificationChannel.LockscreenVisibility = NotificationVisibility.Public;
            notificationChannel.ShouldVibrate();
            notificationChannel.ShouldShowLights();
            notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });

            if (notificationManager != null)
            {
                mBuilder.SetChannelId("ChannelId");
                notificationManager.CreateNotificationChannel(notificationChannel);
            }
        }

        notificationManager.Notify(Guid.NewGuid().GetHashCode(), mBuilder.Build());
    }
}