using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace HealthMate.Models;

public class CalendarDays : ObservableObject
{
    [JsonProperty("date")]
    public int Date { get; set; }
    [JsonProperty("day")]
    public string Day { get; set; }

    public override bool Equals(object obj)
    {
        return obj switch
        {
            CalendarDays calendarDay => Date == calendarDay.Date && Day == calendarDay.Day,
            _ => false
        };
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Date, Day);
    }
}