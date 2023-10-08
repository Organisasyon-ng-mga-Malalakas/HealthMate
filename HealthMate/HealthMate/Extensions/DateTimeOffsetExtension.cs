namespace HealthMate.Extensions;
public static class DateTimeOffsetExtension
{
    public static string GetCorrectTimeStringFromUtc(this DateTimeOffset dateTimeOffset)
    {
        var utctime = DateTime.Today.Add(dateTimeOffset.TimeOfDay);
        var timeOfDay = DateTime.SpecifyKind(utctime, DateTimeKind.Utc);
        var correctTime = TimeZoneInfo.ConvertTimeFromUtc(timeOfDay, TimeZoneInfo.Local);
        return $"{correctTime:hh:mm tt}";
    }
}