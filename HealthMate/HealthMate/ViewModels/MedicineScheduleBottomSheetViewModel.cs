using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Views;

namespace HealthMate.ViewModels;
public partial class MedicineScheduleBottomSheetViewModel : BaseViewModel
{
    [ObservableProperty]
    private string dosage;

    [ObservableProperty]
    private string dosageQty;

    [ObservableProperty]
    private string medicineName;

    [ObservableProperty]
    private string notes;

    [ObservableProperty]
    private DateTime notificationDate;

    [ObservableProperty]
    private TimeSpan notificationTime;

    public MedicineScheduleBottomSheetViewModel()
    {

    }

    [RelayCommand]
    private async Task CloseBottomSheet(MedicineScheduleBottomSheet medicineScheduleBottomSheet)
    {
        await medicineScheduleBottomSheet.DismissAsync();
    }

    [RelayCommand]
    private async Task CreateSchedule(MedicineScheduleBottomSheet medicineScheduleBottomSheet)
    {
        await CloseBottomSheet(medicineScheduleBottomSheet);
    }
}