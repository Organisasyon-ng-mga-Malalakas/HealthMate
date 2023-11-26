using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HealthMate.Enums;
using HealthMate.Extensions;
using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Views.Schedule;
using Realms;
using System.Collections.ObjectModel;
using System.Globalization;
using ScheduleTable = HealthMate.Models.Tables.Schedule;

namespace HealthMate.ViewModels.Schedule;
public partial class SchedulePageViewModel : BaseViewModel
{
    private readonly BottomSheetService _bottomSheetService;
    private readonly PopupService _popupService;
    private readonly RealmService _realmService;

    [ObservableProperty]
    private bool isEmptyViewVisible;

    [ObservableProperty]
    private ObservableCollection<ScheduleGroup> schedules;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Now;

    [ObservableProperty]
    private ObservableCollection<string> months;

    [ObservableProperty]
    private string selectedMonth;

    [ObservableProperty]
    private CalendarDays selectedDay;

    [ObservableProperty]
    private ObservableCollection<CalendarDays> days;

    public SchedulePageViewModel(BottomSheetService bottomSheetService,
        NavigationService navigationService,
        PopupService popupService,
        RealmService realmService) : base(navigationService)
    {
        _bottomSheetService = bottomSheetService;
        _popupService = popupService;
        _realmService = realmService;

        //Task.Run(OnNavigatedTos);
    }

    [RelayCommand]
    private async Task CreateSchedule()
    {
        await _bottomSheetService.OpenBottomSheet<AddScheduleBottomSheet>();
        //var fakeInventory = new Faker<InventoryTable>()
        //    .RuleFor(p => p.BrandName, v => v.Name.FirstName())
        //    .RuleFor(p => p.MedicineName, v => v.Name.LastName())
        //    .RuleFor(p => p.Dosage, v => v.Random.Int(0, 500))
        //    .RuleFor(p => p.DosageUnit, v => v.Random.Int(0, 8))
        //    .RuleFor(p => p.Stock, v => v.Random.Int(1, 100))
        //    .RuleFor(p => p.MedicationType, v => v.Random.Int(0, 9))
        //    .RuleFor(p => p.Description, v => v.Lorem.Paragraph(5))
        //    .Generate(1);

        //var fakeSchedules = new Faker<ScheduleTable>()
        //    .RuleFor(p => p.ScheduleState, v => (int)v.Random.Enum<ScheduleState>())
        //    .RuleFor(p => p.TimeToTake, v => v.Date.Recent(0))
        //    .RuleFor(p => p.Inventory, v => v.PickRandom(fakeInventory))
        //    .RuleFor(p => p.TimeToTake, v => v.PickRandom(
        //        new DateTimeOffset(2020, 1, 1, 5, 30, 0, TimeSpan.Zero),
        //        new DateTimeOffset(2020, 1, 1, 2, 40, 0, TimeSpan.Zero),
        //        new DateTimeOffset(2020, 1, 1, 3, 50, 0, TimeSpan.Zero),
        //        new DateTimeOffset(2020, 1, 1, 17, 29, 0, TimeSpan.Zero),
        //        new DateTimeOffset(2020, 1, 1, 5, 30, 0, TimeSpan.Zero)))
        //    .Generate(1);

        //Schedules.Add(fakeSchedules[0]);
    }

    private void ListenForRealmChanges(IRealmCollection<ScheduleTable> sender, ChangeSet changes)
    {
        if (changes == null)
            return;

        if (changes.InsertedIndices.Any())
            foreach (var item in changes.InsertedIndices)
            {
                var scheduleHeader = sender.ElementAt(item).TimeToTake.GetCorrectTimeStringFromUtc();
                var correctGroup = Schedules.FirstOrDefault(_ => _.ScheduleHeader == scheduleHeader);
                if (correctGroup is not ScheduleGroup unwrappedGroup)
                {
                    var schedule = new ObservableCollection<ScheduleTable> { sender.ElementAt(item) };
                    Schedules.Add(new ScheduleGroup(sender.ElementAt(item).TimeToTake, schedule));
                    IsEmptyViewVisible = !Schedules.Any();
                    return;
                }

                var indexOfCorrectGroup = Schedules.IndexOf(unwrappedGroup);
                Schedules[indexOfCorrectGroup].Add(sender[item]);
                IsEmptyViewVisible = !Schedules.Any();
            }
    }

    public override void OnNavigatedFrom()
    {
        base.OnNavigatedFrom();
    }

    public override async void OnNavigatedTo()
    {
        if (Months == null)
        {
            var months = DateTimeFormatInfo.CurrentInfo.MonthNames
                .Where(_ => !string.IsNullOrWhiteSpace(_))
                .Select(_ => _[..3]);
            Months ??= new ObservableCollection<string>(months);
            SelectedMonth = Months[DateTime.Now.Month - 1];
            WeakReferenceMessenger.Default.Send(SelectedMonth);
        }

        if (Days == null)
        {
            Days ??= [];
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var daysInMonth = DateTime.DaysInMonth(year, month);
            for (var day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(year, month, day);
                var dayOfWeek = date.DayOfWeek.ToString()[..3];
                Days.Add(new CalendarDays
                {
                    Date = day,
                    Day = dayOfWeek
                });
            }

            SelectedDay = Days[DateTime.Now.Day - 1];
            WeakReferenceMessenger.Default.Send(SelectedDay);
        }

        var selectedDate = new DateTime(DateTime.Now.Year,
            Months.IndexOf(SelectedMonth) + 1,
            Days.IndexOf(SelectedDay) + 1);

        Schedules ??= [];
        var schedules = await _realmService.FindAll<ScheduleTable>();
        RealmChangesNotification = schedules.SubscribeForNotifications(ListenForRealmChanges);

        var realmSchedulesList = schedules
            .ToList()
            .Where(_ => TimeZoneInfo.ConvertTimeFromUtc(_.TimeToTake.DateTime, TimeZoneInfo.Local).Date == selectedDate.Date);
        foreach (var newSchedule in realmSchedulesList)
        {
            var groupName = newSchedule.TimeToTake.GetCorrectTimeStringFromUtc();
            var existingGroup = Schedules.FirstOrDefault(g => g.ScheduleHeader == groupName);
            if (existingGroup == null)
            {
                // Group doesn't exist, create a new group and add the item
                var newGroup = new ScheduleGroup(newSchedule.TimeToTake, [newSchedule]);
                Schedules.Add(newGroup);
            }
            else
                // Group exists, check if item already exists within the group based on InventoryId
                if (!existingGroup.Any(item => item.ScheduleId == newSchedule.ScheduleId))
                // Item doesn't exist, add it to the group
                existingGroup.Add(newSchedule);
        }

        var missedMeds = Schedules.Where(x => x.ActualSchedule.ToLocalTime().DateTime < DateTime.Now)
            .SelectMany(_ => _.Schedule);

        foreach (var meds in missedMeds)
            if ((ScheduleState)meds.ScheduleState == ScheduleState.Pending)
                await _realmService.Write(() => meds.ScheduleState = (int)ScheduleState.Missed);

        #region Faker
        //var fakeInventory = new Faker<InventoryTable>()
        //.RuleFor(p => p.BrandName, v => v.Name.FirstName())
        //.RuleFor(p => p.MedicineName, v => v.Name.LastName())
        //.RuleFor(p => p.Dosage, v => v.Random.Int(0, 500))
        //.RuleFor(p => p.DosageUnit, v => v.Random.Int(0, 8))
        //.RuleFor(p => p.Stock, v => v.Random.Int(1, 100))
        //.RuleFor(p => p.MedicationType, v => v.Random.Int(0, 9))
        //.RuleFor(p => p.Description, v => v.Lorem.Paragraph(5))
        //.Generate(25);

        //var fakeSchedules = new Faker<ScheduleTable>()
        //    .RuleFor(p => p.ScheduleState, v => (int)v.Random.Enum<ScheduleState>())
        //    .RuleFor(p => p.TimeToTake, v => v.Date.Recent(0))
        //    .RuleFor(p => p.Inventory, v => v.PickRandom(fakeInventory))
        //    .RuleFor(p => p.TimeToTake, v => v.PickRandom(
        //        new DateTimeOffset(2020, 1, 1, 5, 30, 0, TimeSpan.Zero),
        //        new DateTimeOffset(2020, 1, 1, 2, 40, 0, TimeSpan.Zero),
        //        new DateTimeOffset(2020, 1, 1, 3, 50, 0, TimeSpan.Zero),
        //        new DateTimeOffset(2020, 1, 1, 17, 29, 0, TimeSpan.Zero),
        //        new DateTimeOffset(2020, 1, 1, 5, 30, 0, TimeSpan.Zero)))
        //    .Generate(25);

        //foreach (var item in fakeSchedules)
        //    Schedules.Add(new ScheduleGroup(item.TimeToTake, new ObservableCollection<ScheduleTable>(fakeSchedules)));
        #endregion

        IsEmptyViewVisible = !Schedules.Any();
    }

    [RelayCommand]
    public async Task OpenScheduleInfo(ScheduleTable schedule)
    {
        if ((ScheduleState)schedule.ScheduleState is ScheduleState.Missed)
            await _popupService.ShowPopup<MedsMissedPopup>();
        else
            await _popupService.ShowPopup<ScheduleInfoPopup>(schedule);
    }

    partial void OnSelectedDayChanged(CalendarDays oldValue, CalendarDays newValue)
    {
        if (oldValue != null)
            ChangeSelectedDate(newValue.Date, false).ConfigureAwait(false);
    }

    partial void OnSelectedMonthChanged(string oldValue, string newValue)
    {
        if (!string.IsNullOrWhiteSpace(oldValue))
            ChangeSelectedDate(Months.IndexOf(newValue) + 1, true).ConfigureAwait(false);
    }

    private async Task ChangeSelectedDate(int calendrical, bool isMonth)
    {
        var allSchedules = await _realmService.FindAll<ScheduleTable>();
        var monthIndex = Months.IndexOf(SelectedMonth) + 1;
        var dayIndex = Days.IndexOf(SelectedDay) + 1;

        var newDate = new DateTime(DateTime.Now.Year,
            isMonth ? calendrical : Months.IndexOf(SelectedMonth) + 1,
            isMonth ? Days.IndexOf(SelectedDay) + 1 : calendrical);

        var schedules = allSchedules.ToList()
            .Where(_ => TimeZoneInfo.ConvertTimeFromUtc(_.TimeToTake.DateTime, TimeZoneInfo.Local).Date == newDate.Date)
            .ToList();

        Schedules.Clear();
        if (schedules is List<ScheduleTable> actualSchedules && actualSchedules.Any())
            foreach (var item in actualSchedules)
                Schedules.Add(new ScheduleGroup(item.TimeToTake, [item]));

        IsEmptyViewVisible = !Schedules.Any();
    }
}
//todo: depende sa gamot, palitan yung mga dosages