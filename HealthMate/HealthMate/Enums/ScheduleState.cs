using HealthMate.Constants;

namespace HealthMate.Enums;

public enum ScheduleState
{
    Taken, Missed, Pending
}

public static partial class Extensions
{
    public static Color GetOverallColor(this ScheduleState scheduleState)
    {
        return scheduleState switch
        {
            ScheduleState.Taken => Color.FromArgb("22C55E"),
            ScheduleState.Missed => Color.FromArgb("EF4444"),
            ScheduleState.Pending => (Color)Application.Current.Resources["Pink"],
            _ => throw new NotImplementedException(),
        };
    }

    public static string GetIcon(this ScheduleState scheduleState)
    {
        return scheduleState switch
        {
            ScheduleState.Taken => FontAwesomeIcons.CircleCheck,
            ScheduleState.Missed => FontAwesomeIcons.CircleX,
            ScheduleState.Pending => FontAwesomeIcons.AlarmClock,
            _ => throw new NotImplementedException(),
        };
    }

    public static string GetIconFontFamily(this ScheduleState scheduleState)
    {
        return scheduleState switch
        {
            ScheduleState.Taken or ScheduleState.Missed => (string)Application.Current.Resources["FASolid"],
            ScheduleState.Pending => (string)Application.Current.Resources["FALight"],
            _ => throw new NotImplementedException(),
        };
    }
}