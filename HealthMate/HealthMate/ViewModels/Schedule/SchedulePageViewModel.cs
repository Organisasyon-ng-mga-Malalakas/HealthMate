using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Views.Schedule;
using Realms;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using InventoryTable = HealthMate.Models.Tables.Inventory;
using ScheduleModel = HealthMate.Models.Schedule;

namespace HealthMate.ViewModels.Schedule;
public partial class SchedulePageViewModel : BaseViewModel
{
    private readonly BottomSheetService _bottomSheetService;
    private readonly RealmService _realmService;

    [ObservableProperty]
    private ObservableCollection<CalendarDays> calendarDays;

    [ObservableProperty]
    private ObservableCollection<string> months;

    [ObservableProperty]
    private ObservableCollection<ScheduleModel> schedules;

    [ObservableProperty]
    private int selectedCalendarDayIndex;

    [ObservableProperty]
    private int selectedMonthIndex;

    public SchedulePageViewModel(BottomSheetService bottomSheetService, RealmService realmService)
    {
        _bottomSheetService = bottomSheetService;
        _realmService = realmService;
    }

    [RelayCommand]
    private async Task CreateSchedule()
    {
        await _bottomSheetService.OpenBottomSheet<AddScheduleBottomSheet>(this);
    }

    private void ListenForRealmChanges(IRealmCollection<InventoryTable> sender, ChangeSet changes)
    {
        if (changes == null)
            return;

        foreach (var item in changes.InsertedIndices)
        {
            //var medicationType = ((MedicationType)sender[item].MedicationType).ToString();
            //var correctGroup = Inventory.FirstOrDefault(_ => _.GroupName == medicationType);
            //if (correctGroup is not InventoryGroup unwrappedGroup)
            //{
            //    var inventory = new ObservableCollection<InventoryTable> { sender[item] };
            //    Inventory.Add(new InventoryGroup(medicationType, inventory));
            //    return;
            //}

            //var indexOfCorrectGroup = Inventory.IndexOf(unwrappedGroup);
            //Inventory[indexOfCorrectGroup].Add(sender[item]);
        }
    }

    public override async void OnNavigatedTo()
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

        //var fakeInventory = new Faker<Models.Tables.Inventory>()
        //    .RuleFor(p => p.BrandName, v => v.Name.FirstName())
        //    .RuleFor(p => p.MedicineName, v => v.Name.LastName())
        //    .RuleFor(p => p.Dosage, v => v.Random.Int(0, 500))
        //    .RuleFor(p => p.DosageUnit, v => v.Random.Int(0, 8))
        //    .RuleFor(p => p.Stock, v => v.Random.Int(1, 100))
        //    .RuleFor(p => p.MedicationType, v => v.Random.Int(0, 9))
        //    .RuleFor(p => p.Description, v => v.Lorem.Paragraph(5))
        //    .Generate(5);

        //var fakeSchedules = new Faker<ScheduleModel>()
        //    .RuleFor(p => p.State, v => v.Random.Enum<ScheduleState>())
        //    .RuleFor(p => p.TimeToTake, v => v.Date.Recent(0))
        //    .RuleFor(p => p.Inventory, v => fakeInventory[v.Random.Int(0, 4)])
        //    .Generate(5);
        var inventories = await _realmService.FindAll<InventoryTable>();
        RealmChangesNotification = inventories.SubscribeForNotifications(ListenForRealmChanges);
        Schedules = new ObservableCollection<ScheduleModel>();
        foreach (var item in inventories)
        {
            Schedules.Add(new ScheduleModel
            {
                Inventory = item,
                State = Enums.ScheduleState.Taken,
                TimeToTake = DateTime.Now
            });
        }
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