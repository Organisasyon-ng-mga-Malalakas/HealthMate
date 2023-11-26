using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;

namespace HealthMate.ViewModels.Schedule;
public partial class MedsMissedPopupViewModel : BaseViewModel
{
    private readonly PopupService _popupService;

    public MedsMissedPopupViewModel(NavigationService navigationService, PopupService popupService) : base(navigationService)
    {
        _popupService = popupService;
    }

    [RelayCommand]
    public async Task ClosePopup()
    {
        await _popupService.ClosePopup();
    }
}