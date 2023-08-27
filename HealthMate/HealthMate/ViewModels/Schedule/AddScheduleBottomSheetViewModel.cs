using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Views.Schedule;

namespace HealthMate.ViewModels.Schedule;
public partial class AddScheduleBottomSheetViewModel : BaseViewModel
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

    public AddScheduleBottomSheetViewModel() { }

    [RelayCommand]
    private async Task CloseBottomSheet(AddScheduleBottomSheet medicineScheduleBottomSheet)
    {
        await medicineScheduleBottomSheet.DismissAsync();
    }

    [RelayCommand]
    private async Task CreateSchedule(AddScheduleBottomSheet medicineScheduleBottomSheet)
    {
        await CloseBottomSheet(medicineScheduleBottomSheet);
    }
}