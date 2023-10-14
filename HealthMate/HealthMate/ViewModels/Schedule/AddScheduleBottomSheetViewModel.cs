using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Enums;
using HealthMate.Services;
using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using InventoryTable = HealthMate.Models.Tables.Inventory;
using ScheduleTable = HealthMate.Models.Tables.Schedule;

namespace HealthMate.ViewModels.Schedule;
public partial class AddScheduleBottomSheetViewModel : BaseViewModel
{
    private readonly BottomSheetService _bottomSheetService;
    private readonly RealmService _realmService;

    [ObservableProperty]
    private ObservableCollection<InventoryTable> medicines;

    [ObservableProperty]
    private DateTime minimumDate = DateTime.Now;

    [ObservableProperty]
    private string notes;

    [ObservableProperty]
    private DateTime notificationDate = DateTime.Now;

    [ObservableProperty]
    private TimeSpan notificationTime = DateTime.Now.TimeOfDay.Add(TimeSpan.FromMinutes(1));

    [ObservableProperty]
    [Required]
    [Range(0.1, double.MaxValue)]
    private double quantity;

    [ObservableProperty]
    [Required]
    private InventoryTable selectedMedicine;

    public AddScheduleBottomSheetViewModel(BottomSheetService bottomSheetService, RealmService realmService)
    {
        _bottomSheetService = bottomSheetService;
        _realmService = realmService;
    }

    [RelayCommand]
    private async Task CloseBottomSheet()
    {
        await _bottomSheetService.CloseBottomSheet();
    }

    [RelayCommand]
    private async Task CreateSchedule()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        var cleanDateAndTime = new DateTime(NotificationDate.Year,
            NotificationDate.Month,
            NotificationDate.Day,
            NotificationTime.Hours,
            NotificationTime.Minutes,
            NotificationTime.Seconds);

        await _realmService.Upsert(new ScheduleTable
        {
            ScheduleId = ObjectId.GenerateNewId(),
            ScheduleState = (int)ScheduleState.Pending,
            Inventory = SelectedMedicine,
            Quantity = Quantity,
            TimeToTake = new DateTimeOffset(cleanDateAndTime)
        });

        await CloseBottomSheet();
    }

    public override async void OnNavigatedTo()
    {
        var medicines = await _realmService.Find<InventoryTable>(_ => !_.IsDeleted);
        Medicines = new ObservableCollection<InventoryTable>(medicines);
    }
}