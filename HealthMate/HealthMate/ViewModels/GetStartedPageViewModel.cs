using CommunityToolkit.Mvvm.Input;
using HealthMate.Views;

namespace HealthMate.ViewModels;
public partial class GetStartedPageViewModel : BaseViewModel
{
    public GetStartedPageViewModel(INavigationService navigationService) : base(navigationService) { }

    [RelayCommand]
    private async Task GotoOnboarding()
    {
        await NavigationService.NavigateAsync($"{nameof(OnboardingPage)}");
    }
}
