namespace HealthMate.Extensions;
public static class DateTimeOffsetExtension
{
    public static string GetCorrectTimeStringFromUtc(this DateTimeOffset dateTimeOffset)
    {
        var correctTime = dateTimeOffset.DateTime.ToLocalTime();
        return $"{correctTime:hh:mm tt}";
    }
}