using HealthMate.Templates;
using HealthMate.ViewModels.Schedule;

namespace HealthMate.Views.Schedule;

public partial class SchedulePage : BasePage<SchedulePageViewModel>
{
    public SchedulePage(SchedulePageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
        //Schedules.DataSource.GroupDescriptors.Add(new GroupDescriptor
        //{
        //    KeySelector = (object scheduleTable) =>
        //    {
        //        var schedule = scheduleTable as ScheduleTable;
        //        var utcTime = DateTime.Today.Add(schedule.TimeToTake.TimeOfDay);
        //        var timeOfDay = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
        //        var correctTime = TimeZoneInfo.ConvertTimeFromUtc(timeOfDay, TimeZoneInfo.Local);
        //        return $"{correctTime:hh:mm tt}";
        //    }
        //});
    }
}