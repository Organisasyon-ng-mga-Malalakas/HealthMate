using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;
using HealthMate.Views.SymptomChecker;

namespace HealthMate.ViewModels.SymptomChecker;
public partial class SymptomCheckerPageViewModel : BaseViewModel
{
    private readonly PopupService _popupService;

    public SymptomCheckerPageViewModel(PopupService popupService)
    {
        _popupService = popupService;
    }

    [RelayCommand]
    private async Task OpenDisclaimerPopup()
    {
        await _popupService.ShowPopup<DisclaimerPopup>();
    }
}
