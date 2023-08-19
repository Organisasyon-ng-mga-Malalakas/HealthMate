using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using HealthMate.Models;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;

namespace HealthMate.ViewModels;
public partial class SchedulePageViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<CalendarDays> calendarDays;

    [ObservableProperty]
    private ObservableCollection<string> months;

    [ObservableProperty]
    private CalendarDays selectedCalendarDay;

    [ObservableProperty]
    private string selectedMonth;

    public SchedulePageViewModel() { }

    protected override void OnNavigatedTo()
    {
        var dateNow = DateTime.Now;
        var dateTimeInfo = CultureInfo.CurrentCulture.DateTimeFormat;
        Months = new ObservableCollection<string>(dateTimeInfo.AbbreviatedMonthNames.Where(month => !string.IsNullOrWhiteSpace(month)));
        SelectedMonth = Months[dateNow.Month - 1];

        var daysInMonth = DateTime.DaysInMonth(dateNow.Year, dateNow.Month);
        CalendarDays = new ObservableCollection<CalendarDays>();
        for (var day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(dateNow.Year, dateNow.Month, day);
            CalendarDays.Add(new CalendarDays
            {
                Date = date.Day,
                Day = date.ToString("ddd")
            });
        }

        SelectedCalendarDay = CalendarDays[dateNow.Day];
    }

    public override void OnViewInitialized()
    {
        WeakReferenceMessenger.Default.Send(SelectedCalendarDay);
    }

    partial void OnSelectedMonthChanged(string value)
    {
        var parsedMonth = DateTime.ParseExact(value, "MMM", CultureInfo.InvariantCulture);
        var dateNow = DateTime.Now;
        var daysInMonth = DateTime.DaysInMonth(dateNow.Year, parsedMonth.Month);
        CalendarDays = new ObservableCollection<CalendarDays>();
        for (var day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(dateNow.Year, parsedMonth.Month, day);
            CalendarDays.Add(new CalendarDays
            {
                Date = date.Day,
                Day = date.ToString("ddd")
            });
        }

        SelectedCalendarDay = CalendarDays.First();
    }
}