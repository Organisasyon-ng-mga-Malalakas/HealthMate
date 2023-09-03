using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;
using HealthMate.Views.Schedule;
using Realms;
using System.Collections.ObjectModel;
using ScheduleTable = HealthMate.Models.Tables.Schedule;

namespace HealthMate.ViewModels.Schedule;
public partial class SchedulePageViewModel : BaseViewModel
{
    private readonly BottomSheetService _bottomSheetService;
    private readonly PopupService _popupService;
    private readonly RealmService _realmService;

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private ObservableCollection<ScheduleTable> schedules;

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

        foreach (var item in changes.InsertedIndices)
            Schedules.Add(sender[item]);
    }

    public override void OnNavigatedFrom()
    {
        base.OnNavigatedFrom();
    }

    public override async void OnNavigatedTo()
    {
        //IsLoading = true;
        Schedules ??= new ObservableCollection<ScheduleTable>();
        var schedules = await _realmService.FindAll<ScheduleTable>();
        RealmChangesNotification = schedules.SubscribeForNotifications(ListenForRealmChanges);

        var realmSchedulesList = schedules.ToList();
        var schedulesList = Schedules.ToList();
        var itemsNotInSchedule = realmSchedulesList.Where(realm => !schedulesList.Any(schedules => schedules.ScheduleId == realm.ScheduleId));
        foreach (var item in itemsNotInSchedule)
            Schedules.Add(item);

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
        //    Schedules.Add(item);
        #endregion

        //IsLoading = false;
    }

    [RelayCommand]
    private async Task OpenMedsTakenPopup(Syncfusion.Maui.ListView.ItemTappedEventArgs args)
    {
        var schedule = (ScheduleTable)args.DataItem;
        await _popupService.ShowPopup<MedsTakenPopup>(schedule);
    }

    [RelayCommand]
    private async Task SelectedDateChanged(DateTime newDate)
    {
        //IsLoading = true;
        var allSchedules = await _realmService.FindAll<ScheduleTable>();
        var schedules = allSchedules.ToList()
            .Where(_ => TimeZoneInfo.ConvertTimeFromUtc(_.TimeToTake.DateTime, TimeZoneInfo.Local).Date == newDate.Date)
            .ToList();

        var itemsToRemove = Schedules.Except(schedules).ToList();
        var itemsToAdd = schedules.Except(Schedules).ToList();

        foreach (var item in itemsToRemove)
            Schedules.Remove(item);

        foreach (var item in itemsToAdd)
            Schedules.Add(item);
        //IsLoading = false;
    }
}
//todo: depende sa gamot, palitan yung mga dosages