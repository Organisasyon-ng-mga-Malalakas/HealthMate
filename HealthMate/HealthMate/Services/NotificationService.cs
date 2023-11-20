using Fody;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace HealthMate.Services;

public class NotificationService
{
    private readonly INotificationService _notificationService;
    private static readonly string[] notificationTitles = new string[15]
    {
        "Health Alert! 💊", "Pill Time! 🕘", "Medication Reminder 🚨", "Wellness Check-In 💙", "Medicine O'Clock 🕰️",
        "Time for Health 🩺", "Your Meds Await! 💌", "Care Reminder 💡", "Daily Dose Time ⏳", "Vital Health Step 🚶‍♂️",
        "Healthy Habit Alert 🍏", "Prescription Time 💼", "Healing Hour ⌛", "Your Wellness Alarm ⏰", "Medicine Moment 🧭",
    };
    private static readonly string[] notificationSubtitles = new string[15]
    {
        "Please take your medication now.", "It's that time again!", "Stay on the path to wellness.", "For your health and happiness.", "Keep up the good work!",
        "A friendly nudge for your health.", "Time to take a health break.", "Your prescription for a good day.", "Let's keep you feeling your best!", "A step towards better health.",
        "Maintain your health routine.", "Ensuring your wellbeing.", "It's time to feel good!", "Your health is important.", "A moment for your wellbeing."
    };

    public NotificationService()
    {
        _notificationService = LocalNotificationCenter.Current;
    }

    [ConfigureAwait(false)]
    public async Task<bool> AskNotificationPermissionAsync()
    {
        var isNotificationsEnabled = await _notificationService.AreNotificationsEnabled();
        if (isNotificationsEnabled)
            return true;

        var isNotificationEnabled = await _notificationService.RequestNotificationPermission();
        return isNotificationEnabled;
    }

    [ConfigureAwait(false)]
    public async Task ScheduleNotification(string description, DateTime notifiyTime)
    {
        var randomIndex = Random.Shared.Next(14);
        var notificationIcon = new AndroidIcon { ResourceName = "notif_icon" };
        var notificationRequest = new NotificationRequest
        {
            Android = new AndroidOptions
            {
                ChannelId = "com.group10.healthmate",
                IconLargeName = notificationIcon,
                IconSmallName = notificationIcon,
                IsGroupSummary = true,
                Priority = AndroidPriority.Max,
                VibrationPattern = [200, 300, 200, 300, 200, 300],
                VisibilityType = AndroidVisibilityType.Public
            },
            CategoryType = NotificationCategoryType.Alarm,
            Description = description,
            Group = "com.group10.healthmate",
            //Image = new NotificationImage
            //{
            //    FilePath
            //},
            NotificationId = Guid.NewGuid().GetHashCode(),
            Schedule = new NotificationRequestSchedule
            {
                Android = new AndroidScheduleOptions
                {
                    AlarmType = AndroidAlarmType.RtcWakeup,
                },
                NotifyTime = notifiyTime,
                RepeatType = NotificationRepeat.No
            },
            Subtitle = notificationSubtitles[randomIndex],
            Title = notificationTitles[randomIndex]
        };

        await _notificationService.Show(notificationRequest);
    }
}
