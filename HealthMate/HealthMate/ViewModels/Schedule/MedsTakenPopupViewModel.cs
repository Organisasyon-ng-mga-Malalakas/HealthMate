using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;
using ScheduleTable = HealthMate.Models.Tables.Schedule;

namespace HealthMate.ViewModels.Schedule;
public partial class MedsTakenPopupViewModel : BaseViewModel
{
    private readonly PopupService _popupService;

    [ObservableProperty]
    private ScheduleTable passedSchedule;

    public MedsTakenPopupViewModel(PopupService popupService)
    {
        _popupService = popupService;
    }

    [RelayCommand]
    private async Task ClosePopup()
    {
        await _popupService.ClosePopup();
    }

    [RelayCommand]
    private async Task MedsTaken()
    {
        await ClosePopup();
    }
}