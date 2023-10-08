using CommunityToolkit.Mvvm.Messaging;
using HealthMate.Models;
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

        WeakReferenceMessenger.Default.Register<string>(this, ScrollMonthCollectionView);
        WeakReferenceMessenger.Default.Register<CalendarDays>(this, ScrollDaysCollectionView);
    }

    private async void ScrollDaysCollectionView(object recipient, CalendarDays message)
    {
        await Task.Delay(650);
        //MainThread.BeginInvokeOnMainThread(() => DayCollectionView.ScrollTo(message, position: ScrollToPosition.Center, animate: true));
        DayCollectionView.ScrollTo(message, position: ScrollToPosition.Center, animate: true);

    }

    private async void ScrollMonthCollectionView(object recipient, string message)
    {
        await Task.Delay(650);
        //MainThread.BeginInvokeOnMainThread(() => MonthCollectionView.ScrollTo(message, position: ScrollToPosition.Center, animate: true));
        MonthCollectionView.ScrollTo(message, position: ScrollToPosition.Center, animate: true);
    }
}