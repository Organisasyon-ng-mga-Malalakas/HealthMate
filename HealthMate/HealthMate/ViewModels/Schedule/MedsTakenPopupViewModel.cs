using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;
using ScheduleTable = HealthMate.Models.Tables.Schedule;

namespace HealthMate.ViewModels.Schedule;
public partial class MedsTakenPopupViewModel : BaseViewModel
{
    private readonly PopupService _popupService;
    private readonly RealmService _realmService;

    [ObservableProperty]
    private ScheduleTable passedSchedule;

    public MedsTakenPopupViewModel(PopupService popupService, RealmService realmService)
    {
        _popupService = popupService;
        _realmService = realmService;
    }

    [RelayCommand]
    private async Task ClosePopup()
    {
        await _popupService.ClosePopup();
    }

    [RelayCommand]
    private async Task MedsTaken()
    {
        await _realmService.Write(() => PassedSchedule.Inventory.Stock -= PassedSchedule.Quantity);
        await ClosePopup();
    }
}