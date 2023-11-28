using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;
using HealthMate.Views.SymptomChecker;

namespace HealthMate.ViewModels.SymptomChecker;
public partial class SymptomCheckerPageViewModel(NavigationService navigationService, PopupService popupService) : BaseViewModel(navigationService)
{
	[RelayCommand]
	private async Task OpenDisclaimerPopup()
	{
		await popupService.ShowPopup<DisclaimerPopup>();
		//await NavigationService.PushAsync(nameof(IllnessInfoPopup));
	}
}