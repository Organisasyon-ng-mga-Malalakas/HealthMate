using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;
using HealthMate.Views.Onboarding;

namespace HealthMate.ViewModels.Onboarding;
public partial class GetStartedPageViewModel : BaseViewModel
{
    public GetStartedPageViewModel(NavigationService navigationService) : base(navigationService)
    {

    }

    [RelayCommand]
    private async Task GotoOnboarding()
    {
        await NavigationService.PushAsync($"{nameof(OnboardingPage)}");
    }
}
