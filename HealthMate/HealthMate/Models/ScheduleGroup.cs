using System.Collections.ObjectModel;

namespace HealthMate.Models;
public class ScheduleGroup : ObservableCollection<Tables.Schedule>
{
    public string GroupName { get; set; }
    public ScheduleGroup(DateTimeOffset groupName, ObservableCollection<Tables.Schedule> schedules) : base(schedules)
    {
        var timeOfDay = DateTime.Today.Add(groupName.TimeOfDay);
        GroupName = $"{timeOfDay:HH:mm tt}";
    }
}
//stock create inventory check for validatrion