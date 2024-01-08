using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;

namespace HealthMate.ViewModels.Onboarding;
public partial class GetStartedPageViewModel(NavigationService navigationService) : BaseViewModel(navigationService)
{
	[RelayCommand]
	private void GotoOnboarding()
	{
		NavigationService.ChangeShellItem(1);
	}

	[RelayCommand]
	private void GotoAccountPage()
	{
		NavigationService.ChangeShellItem(2);
	}
}
