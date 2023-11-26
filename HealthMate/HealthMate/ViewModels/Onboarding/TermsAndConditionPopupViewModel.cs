using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;

namespace HealthMate.ViewModels.Onboarding;
public partial class TermsAndConditionPopupViewModel : BaseViewModel
{
    private readonly PopupService _popupService;

    public TermsAndConditionPopupViewModel(NavigationService navigationService, PopupService popupService) : base(navigationService)
    {
        _popupService = popupService;
    }

    [RelayCommand]
    private async Task Agreed()
    {
        await ClosePopup();
        await NavigationService.PushAsync("//Tabs");
    }

    [RelayCommand]
    private async Task Disagreed()
    {
        await ClosePopup();
    }

    private async Task ClosePopup()
    {
        await _popupService.ClosePopup();
    }
}
