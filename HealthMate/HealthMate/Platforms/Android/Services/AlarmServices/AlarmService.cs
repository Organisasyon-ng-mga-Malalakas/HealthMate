using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

namespace HealthMate.Platforms.Android.Services.AlarmServices;
[Service]
public class AlarmService : Service
{
    public override IBinder OnBind(Intent intent)
    {
        return null;
    }

    [return: GeneratedEnum]
    public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
    {
        //WeakReferenceMessenger.Default.Send<object, string>(this, "AlarmUI");
        //WeakReferenceMessenger.Default.Send(this);
        //TriggerAlarmUI();

        var mainActivityIntent = new Intent(this, typeof(MainActivity));
        mainActivityIntent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);
        mainActivityIntent.PutExtra("openAlarmUI", true);
        StartActivity(mainActivityIntent);

        return StartCommandResult.Sticky;
    }

    private void TriggerAlarmUI()
    {
        var broadcastIntent = new Intent("HealthMate.OPEN_ALARM_UI");
        SendBroadcast(broadcastIntent);

        //var intent = new Intent(this, typeof(MainActivity));
        //intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);
        //StartActivity(intent);

    }
}