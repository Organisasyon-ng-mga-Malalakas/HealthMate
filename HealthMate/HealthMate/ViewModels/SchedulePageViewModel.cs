using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HealthMate.Models;
using HealthMate.Views;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;

namespace HealthMate.ViewModels;
public partial class SchedulePageViewModel : BaseViewModel
{
    private readonly MedicineScheduleBottomSheetViewModel _medicineScheduleBottomSheetViewModel;

    [ObservableProperty]
    private ObservableCollection<CalendarDays> calendarDays;

    [ObservableProperty]
    private ObservableCollection<string> months;

    [ObservableProperty]
    private int selectedCalendarDayIndex;

    [ObservableProperty]
    private int selectedMonthIndex;

    public SchedulePageViewModel(MedicineScheduleBottomSheetViewModel medicineScheduleBottomSheetViewModel)
    {
        _medicineScheduleBottomSheetViewModel = medicineScheduleBottomSheetViewModel;
    }

    [RelayCommand]
    private void CreateSchedule()
    {
        var medicineSchedule = new MedicineScheduleBottomSheet(_medicineScheduleBottomSheetViewModel);
        medicineSchedule.Showing += (_, _) => medicineSchedule.Controller.Behavior.DisableShapeAnimations();
        medicineSchedule.ShowAsync(true);
    }

    protected override async void OnNavigatedTo()
    {
        #region Setup months
        var dateNow = DateTime.Now;
        var dateTimeInfo = CultureInfo.CurrentCulture.DateTimeFormat;
        Months = new ObservableCollection<string>(dateTimeInfo.AbbreviatedMonthNames.Where(month => !string.IsNullOrWhiteSpace(month)));
        SelectedMonthIndex = dateNow.Month - 1;
        #endregion

        #region Setup calendar days
        var daysInMonth = DateTime.DaysInMonth(dateNow.Year, dateNow.Month);
        CalendarDays = new ObservableCollection<CalendarDays>();
        for (var day = 1; day <= daysInMonth; day++)
        {
            var date = new DateTime(dateNow.Year, dateNow.Month, day);
            CalendarDays.Add(new CalendarDays
            {
                Date = date.Day,
                Day = date.ToString("ddd"),
            });
        }

        SelectedCalendarDayIndex = dateNow.Day - 1;
        #endregion

        await Task.Delay(2000);
        WeakReferenceMessenger.Default.Send(Months[SelectedMonthIndex]);
        WeakReferenceMessenger.Default.Send(CalendarDays[SelectedCalendarDayIndex]);
    }

    partial void OnSelectedMonthIndexChanged(int value)
    {
        var parsedMonth = DateTime.ParseExact(Months[value], "MMM", CultureInfo.InvariantCulture);
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

        SelectedCalendarDayIndex = 0;
        WeakReferenceMessenger.Default.Send(Months[SelectedMonthIndex]);
        WeakReferenceMessenger.Default.Send(CalendarDays[SelectedCalendarDayIndex]);
    }

    [RelayCommand]
    private void SelectCalendarDay(CalendarDays calendarDay)
    {
        SelectedCalendarDayIndex = CalendarDays.IndexOf(calendarDay);
        WeakReferenceMessenger.Default.Send(calendarDay);
    }

    [RelayCommand]
    private void SelectMonth(string month)
    {
        SelectedMonthIndex = Months.IndexOf(month);
    }
}