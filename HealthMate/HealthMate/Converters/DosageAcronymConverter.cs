using HealthMate.Enums;
using System.Globalization;

namespace HealthMate.Converters;

public class DosageAcronymConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var rawInt = (int)value;
        var enumRepresentation = (Dosage)rawInt;
        return enumRepresentation.GetAcronym();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}