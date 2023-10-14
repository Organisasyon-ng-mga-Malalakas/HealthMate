using HealthMate.Extensions;
using HealthMate.Models.Tables;
using System.Collections.ObjectModel;

namespace HealthMate.Models;
public class ScheduleGroup : ObservableCollection<Schedule>
{
    public DateTimeOffset ActualSchedule { get; set; }
    public string ScheduleHeader { get; }
    public ObservableCollection<Schedule> Schedule { get; }

    public ScheduleGroup(DateTimeOffset schedule, ObservableCollection<Schedule> schedules) : base(schedules)
    {
        ActualSchedule = schedule;
        ScheduleHeader = schedule.GetCorrectTimeStringFromUtc();
        Schedule = schedules;
    }
}