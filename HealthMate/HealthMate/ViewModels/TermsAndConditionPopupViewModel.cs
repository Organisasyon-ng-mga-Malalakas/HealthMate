using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;
using HealthMate.Views;

namespace HealthMate.ViewModels;
public partial class TermsAndConditionPopupViewModel : BaseViewModel
{
    private readonly PopupService _popupService;

    public TermsAndConditionPopupViewModel(INavigationService navigationService, PopupService popupService) : base(navigationService)
    {
        _popupService = popupService;
    }

    [RelayCommand]
    private async Task Agreed()
    {
        await ClosePopup();
        await NavigationService.NavigateAsync($"../{nameof(HomePage)}");
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
