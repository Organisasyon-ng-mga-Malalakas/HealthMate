using HealthMate.Enums;
using System.Globalization;

namespace HealthMate.Converters;

public class MedicationTypeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var rawInt = (int)value;
        var enumRepresentation = (MedicationType)rawInt;
        return enumRepresentation.GetDisplayUnit();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}