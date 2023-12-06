using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;
using HealthMate.Views.Accounts;

namespace HealthMate.ViewModels.Onboarding;
public partial class TermsAndConditionPopupViewModel(NavigationService navigationService, PopupService popupService) : BaseViewModel(navigationService)
{
	[RelayCommand]
	private async Task Agreed()
	{
		await ClosePopup();
		//NavigationService.ChangeShellItem(2);
		await NavigationService.PushAsync(nameof(AccountPage));
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
