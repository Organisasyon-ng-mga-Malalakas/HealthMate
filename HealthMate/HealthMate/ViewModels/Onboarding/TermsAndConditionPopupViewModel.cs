using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;

namespace HealthMate.ViewModels.Onboarding;
public partial class TermsAndConditionPopupViewModel(NavigationService navigationService, PopupService popupService) : BaseViewModel(navigationService)
{
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
		await popupService.ClosePopup();
	}
}
