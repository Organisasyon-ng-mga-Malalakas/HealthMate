using HealthMate.Enums;
using System.Globalization;

namespace HealthMate.Converters;
public class ScheduleStateToStingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var rawInt = (int)value;
        var enumRepresentation = (ScheduleState)rawInt;
        return enumRepresentation switch
        {
            ScheduleState.Taken => "Meds Taken",
            ScheduleState.Missed => "Meds Missed",
            ScheduleState.Pending => "",
            _ => throw new NotImplementedException(),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ScheduleStateToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var rawInt = (int)value;
        var enumRepresentation = (ScheduleState)rawInt;
        return enumRepresentation.GetOverallColor();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ScheduleStateToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var rawInt = (int)value;
        var enumRepresentation = (ScheduleState)rawInt;
        return enumRepresentation.GetIcon();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}