using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Views.Schedule;
using Realms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using ScheduleTable = HealthMate.Models.Tables.Schedule;

namespace HealthMate.ViewModels.Schedule;
public partial class SchedulePageViewModel : BaseViewModel
{
    private readonly BottomSheetService _bottomSheetService;
    private readonly PopupService _popupService;
    private readonly RealmService _realmService;

    [ObservableProperty]
    private bool isActionBtnVisible;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private ObservableCollection<ScheduleGroup> schedules;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Now;

    public SchedulePageViewModel(BottomSheetService bottomSheetService,
        PopupService popupService,
        RealmService realmService)
    {
        _bottomSheetService = bottomSheetService;
        _popupService = popupService;
        _realmService = realmService;
    }

    [RelayCommand]
    private async Task CreateSchedule()
    {
        await _bottomSheetService.OpenBottomSheet<AddScheduleBottomSheet>();
    }

    private void ListenForRealmChanges(IRealmCollection<ScheduleTable> sender, ChangeSet changes)
    {
        if (changes == null)
            return;

        foreach (var item in changes.InsertedIndices)
        {
            var formattedTimeHeader = $"{DateTime.Today.Add(sender[item].TimeToTake.TimeOfDay):HH:mm tt}";
            var correctGroup = Schedules.FirstOrDefault(_ => $"{DateTime.Parse(_.GroupName):HH:mm tt}" == formattedTimeHeader);
            if (correctGroup is not ScheduleGroup unwrappedGroup)
            {
                var schedule = new ObservableCollection<ScheduleTable> { sender[item] };
                Schedules.Add(new ScheduleGroup(sender[item].TimeToTake, schedule));
                return;
            }

            var indexOfCorrectGroup = Schedules.IndexOf(unwrappedGroup);
            Schedules[indexOfCorrectGroup].Add(sender[item]);
        }
    }

    public override void OnNavigatedFrom()
    {
        base.OnNavigatedFrom();
        Schedules.CollectionChanged -= OnSchedulesCollectionChanged;
    }

    public override async void OnNavigatedTo()
    {
        //IsLoading = true;
        Schedules = new ObservableCollection<ScheduleGroup>();
        Schedules.CollectionChanged += OnSchedulesCollectionChanged;
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

        //for (var index = 0; index < 25; index++)
        //    Schedules.Add(new ScheduleGroup(fakeSchedules[index].TimeToTake, new ObservableCollection<ScheduleTable>(fakeSchedules.Take(5))));
        #endregion

        var schedules = await _realmService.FindAll<ScheduleTable>();
        RealmChangesNotification = schedules.SubscribeForNotifications(ListenForRealmChanges);
        foreach (var item in schedules)
        {
            var listToAdd = schedules.Where(_ => _.TimeToTake == item.TimeToTake);
            if (listToAdd.Any())
                Schedules.Add(new ScheduleGroup(ToLocalTime(item.TimeToTake.DateTime), new ObservableCollection<ScheduleTable>(listToAdd)));
        }

        IsActionBtnVisible = Schedules.Any();
        //IsLoading = false;
    }

    private void OnSchedulesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        IsActionBtnVisible = Schedules.Any();
    }

    [RelayCommand]
    private async Task OpenMedsTakenPopup(ScheduleTable passedSchedule)
    {
        await _popupService.ShowPopup<MedsTakenPopup>(passedSchedule);
    }

    [RelayCommand]
    private async Task SelectedDateChanged(DateTime newDate)
    {
        //IsLoading = true;
        Schedules.Clear();
        var allSchedules = await _realmService.FindAll<ScheduleTable>();
        var schedules = allSchedules.ToList()
            .Where(_ => ToLocalTime(_.TimeToTake.DateTime).Date == newDate.Date);
        foreach (var item in schedules)
        {
            var listToAdd = schedules.Where(_ => _.TimeToTake == item.TimeToTake);
            if (listToAdd.Any())
                Schedules.Add(new ScheduleGroup(ToLocalTime(item.TimeToTake.DateTime), new ObservableCollection<ScheduleTable>(listToAdd)));
        }

        IsActionBtnVisible = Schedules.Any();
        //IsLoading = false;
    }

    private static DateTime ToLocalTime(DateTime dateTimeToConvert)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(dateTimeToConvert, TimeZoneInfo.Local);
    }
}