using HealthMate.Extensions;
using HealthMate.Models.Tables;
using System.Collections.ObjectModel;

namespace HealthMate.Models;
public class ScheduleGroup(DateTimeOffset schedule, ObservableCollection<Schedule> schedules) : ObservableCollection<Schedule>(schedules)
{
	public DateTimeOffset ActualSchedule { get; set; } = schedule;
	public string ScheduleHeader { get; } = schedule.GetCorrectTimeStringFromUtc();
	public ObservableCollection<Schedule> Schedule { get; } = schedules;
}