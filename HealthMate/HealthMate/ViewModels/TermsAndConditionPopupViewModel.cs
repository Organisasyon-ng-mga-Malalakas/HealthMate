using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;

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
        await _popupService.ClosePopup();
    }

    [RelayCommand]
    private async Task Disagreed()
    {
        await _popupService.ClosePopup();
    }
}
