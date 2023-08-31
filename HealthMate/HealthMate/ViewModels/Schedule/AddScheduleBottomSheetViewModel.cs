using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Enums;
using HealthMate.Services;
using System.Collections.ObjectModel;
using InventoryTable = HealthMate.Models.Tables.Inventory;

namespace HealthMate.ViewModels.Schedule;
public partial class AddScheduleBottomSheetViewModel : BaseViewModel
{
    private readonly BottomSheetService _bottomSheetService;
    private readonly RealmService _realmService;

    [ObservableProperty]
    private string dosage;

    [ObservableProperty]
    private double dosageQty;

    [ObservableProperty]
    private ObservableCollection<InventoryTable> medicines;

    [ObservableProperty]
    private string notes;

    [ObservableProperty]
    private DateTime notificationDate;

    [ObservableProperty]
    private TimeSpan notificationTime;

    [ObservableProperty]
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
        await CloseBottomSheet();
    }

    public override async void OnNavigatedTo()
    {
        var medicines = await _realmService.FindAll<InventoryTable>();
        Medicines = new ObservableCollection<InventoryTable>(medicines);
    }

    partial void OnSelectedMedicineChanged(InventoryTable value)
    {
        DosageQty = value.DosageUnit;
        Dosage = ((Dosage)value.DosageUnit).ToString();
    }
}